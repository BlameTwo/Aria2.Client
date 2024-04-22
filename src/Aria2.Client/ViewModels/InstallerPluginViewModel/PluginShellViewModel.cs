using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using BtSearch.Loader;
using BtSearch.Loader.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.ApplicationModel.Activation;
using Windows.System;

namespace Aria2.Client.ViewModels.InstallerPluginViewModel;

public sealed partial class PluginShellViewModel : ObservableObject
{
    public PluginShellViewModel(IDialogManager dialogManager,ITipShow tipShow)
    {
        cts = new();
        DialogManager = dialogManager;
        TipShow = tipShow;
    }
    CancellationTokenSource cts = null;

    [ObservableProperty]
    string _FilePath;

    [ObservableProperty]
    Aria2PluginEntity _PluginEntity;

    [ObservableProperty]
    BitmapImage _IconBmp;

    [ObservableProperty]
    PluginHostModel _PluginModel;

    [ObservableProperty]
    int _MaxFiles;

    [ObservableProperty]
    int _NowFiles;

    [ObservableProperty]
    double _ProcessRatio;

    [ObservableProperty]
    string _TipMessage;

    [ObservableProperty]
    InstallType _InstallType;

    /// <summary>
    /// 最后安装文件夹
    /// </summary>
    string InstallPath { get; set; }
    public PluginInstallerVersion OldConfig { get; private set; }
    public IDialogManager DialogManager { get; }
    public ITipShow TipShow { get; }

    ZipArchive _zipfile=null;

    async Task ReadPackage(string fileName)
    {
        Debugger.Launch();
        _zipfile= ZipFile.OpenRead(fileName);
        PluginHostModel model = null;
        foreach (var item in _zipfile.Entries)
        {
            if (!item.FullName.Contains("Public.xml"))
            {
                continue;
            }
            XmlSerializer xmlser = new XmlSerializer(typeof(PluginHostModel));
            model = (PluginHostModel)xmlser.Deserialize(item.Open());
            this.PluginModel = model;
            break;
        }
        if (model == null)
            return;
        string HostfilePath = $"{model.PluginHostFilePath.File}";
        var hostxml = _zipfile.GetEntry(HostfilePath).Open();
        XmlSerializer pluginser = new XmlSerializer(typeof(Aria2PluginEntity));
        var pluginhost = (Aria2PluginEntity)pluginser.Deserialize(hostxml);
        this.PluginEntity = pluginhost;
        BitmapImage bmp = new();
        var logo = _zipfile.GetEntry(model.PluginIcon.Src);
        var temp = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".tmp";
        logo.ExtractToFile(temp);
        FileStream fs = new FileStream(temp, FileMode.Open, FileAccess.Read);
        await bmp.SetSourceAsync(fs.AsRandomAccessStream());
        this.MaxFiles = PluginModel.FilesData.FileList.Count;
        this.IconBmp = bmp;
    }

    [RelayCommand]
    async Task Loaded()
    {
        IApplicationSetup<App> ApplicationSetup = ProgramLife.GetService<IApplicationSetup<App>>();
        ApplicationSetup.Application.MainWindow.IsResizable = true;
        ApplicationSetup.Application.MainWindow.IsMaximizable = false;
        ApplicationSetup.Application.MainWindow.IsMinimizable = false;
        ApplicationSetup.Application.MainWindow.MaxHeight = 270;
        ApplicationSetup.Application.MainWindow.MinHeight = 270;
        ApplicationSetup.Application.MainWindow.MaxWidth = 500;
        ApplicationSetup.Application.MainWindow.MinWidth = 500;
        if (ApplicationSetup.LauncherArgs.Kind != Microsoft.Windows.AppLifecycle.ExtendedActivationKind.File)
            Process.GetCurrentProcess().Kill();
        var arg = ApplicationSetup.LauncherArgs.Data as FileActivatedEventArgs;
        var files = arg.Files[0];
        this.FilePath = files.Path;
        await ReadPackage(FilePath);
        this.InstallPath = Aria2Config.PluginPath + $"\\{PluginEntity.Name}";
        if(await CheckExiste())
        {
            if(OldConfig.Version == PluginModel.Version)
            {
                TipMessage = "已经安装相同版本，不可更换";
                SetupEnable = false;
                InstallType = InstallType.Exist;
            }
        }
    }

    private async Task<bool> CheckExiste()
    {
        return await Task.Run(async () =>
        {
            bool versionEx = false;
            foreach (var dir in new DirectoryInfo(Aria2Config.PluginPath).EnumerateDirectories())
            {
                bool flage = false;
                foreach (var file in dir.EnumerateFiles())
                {
                    if (file.FullName.EndsWith(".json") && file.FullName.Contains("InstallerVersion"))
                    {
                        flage = true;
                        this.OldConfig = JsonSerializer.Deserialize<PluginInstallerVersion>(await File.ReadAllTextAsync(file.FullName));
                        if(this.OldConfig.Guid == PluginEntity.GUID)
                        {
                            versionEx = true;
                        }
                        break;
                    }
                }
                if(flage == false)
                {
                    Directory.Delete(dir.FullName,true);
                }
            }
            return versionEx;
        });
    }

    [RelayCommand]
    async Task SetupAsync()
    {
        this.InstallType = InstallType.Installing;
        await Task.Run(async () =>
        {
            Directory.CreateDirectory(InstallPath);
            foreach (var item in PluginModel.FilesData.FileList)
            {
                try
                {
                    var stream = _zipfile.GetEntry(item.FilePath);
                    var filename = System.IO.Path.GetFileName(item.FilePath);
                    stream.ExtractToFile($"{InstallPath}\\{filename}");
                    ProgramLife.GetService<IApplicationSetup<App>>().TryEnqueue(() =>
                    {
                        NowFiles++;
                        ProcessRatio = Math.Round((double)(NowFiles / MaxFiles) * 100, 2);
                    });
                }
                catch (Exception ex)
                {
                    // Log 取出文件错误
                    continue;
                }
            }
            PluginInstallerVersion version = new()
            {
                Name = PluginEntity.Name,
                Guid = PluginEntity.GUID,
                Version = PluginModel.Version
            };
            await File.WriteAllTextAsync($"{InstallPath}\\InstallerVersion.json", JsonSerializer.Serialize(version));
            this.InstallType = InstallType.Installed;
        });
    }

    [ObservableProperty]
    bool _SetupEnable=true;

    [RelayCommand]
    void Uninstall()
    {
        cts.Cancel();
        Process.GetCurrentProcess().Kill();
    }

}

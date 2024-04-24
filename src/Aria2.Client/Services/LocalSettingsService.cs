using Aria2.Client.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.Services
{
    public sealed class LocalSettingsService : ILocalSettingsService
    {

        /// <summary>
        /// 保存文件名称
        /// </summary>
        public string LocalSettingFileName => "AppSetting.json";

        /// <summary>
        /// 保存文件夹
        /// </summary>
        public string LocalSettingFolder =>
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Aria2ClientPlugin\\";

        string file
        {
            get { return Path.Combine(LocalSettingFolder, LocalSettingFileName); }
        }

        public Dictionary<string, object> Config { get; set; }

        public async Task<bool> InitSetting(Dictionary<string, object> values,CancellationToken token = default)
        {
            var source = Path.Combine(LocalSettingFolder, LocalSettingFileName);
            if (!Directory.Exists(LocalSettingFolder))
                Directory.CreateDirectory(LocalSettingFolder);
            if (!File.Exists(source))
            {
                //不存在文件首先创建
                await initWrite();
                //获取到传入的默认值然后写入并保存
                if (values != null && values.Count > 0)
                {
                    this.Config = values;
                    await SaveAsync(token);
                }
                return true;
            }
            await refresh();
            bool saveFlage = false;
            foreach (var item in values)
            {
                if (this.Config.ContainsKey(item.Key))
                    continue;
                this.Config.Add(item.Key, item.Value);
                saveFlage = true;
            }
            if (saveFlage)
                await SaveAsync(token);
            return true;
        }

        public async Task<bool> InitSetting(CancellationToken token = default)
        {
            var source = Path.Combine(LocalSettingFolder, LocalSettingFileName);
            if (!Directory.Exists(LocalSettingFolder))
                Directory.CreateDirectory(LocalSettingFolder);
            if (!File.Exists(source))
            {
                await initWrite(token);
                return true;
            }
            await refresh(token);
            return true;
        }

        private async Task initWrite(CancellationToken token =default)
        {
            await File.WriteAllTextAsync(Path.Combine(LocalSettingFolder, LocalSettingFileName), "",token);
        }

        async Task refresh(CancellationToken token =default)
        {
            await Task.Run(async () =>
            {
                lock (this)
                {
                    JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
                    options.WriteIndented = true;
                    options.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                    if (!File.Exists(file))
                        return;
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                    StreamReader reader = new(file);
                    var str = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        Config = JsonSerializer.Deserialize<Dictionary<string, object>>(str, options)!;
                    }
                    else
                        Config = new();
                }
            });
        }

        public async Task<object> ReadConfig(string key, CancellationToken token = default)
        {
            try
            {
                await refresh(token);
                if (Config.ContainsKey(key))
                {
                    object ob = null;
                    Config.TryGetValue(key, out ob);
                    var je =  (JsonElement)ob;
                    switch (je.ValueKind)
                    {
                        case JsonValueKind.Undefined:
                            break;
                        case JsonValueKind.String:
                            return je.GetString();
                        case JsonValueKind.Number:
                            return je.GetInt32();
                        case JsonValueKind.True:
                            return true;
                        case JsonValueKind.False:
                            return false;
                        case JsonValueKind.Null:
                            return null;
                    }
                }
                else
                {
                    return default;
                }
            }
            catch (Exception)
            {
                return default;
            }
            return null;
        }

        public async Task<T> ReadObjectConfig<T>(string key,CancellationToken token = default)
        {
            try
            {
                await refresh(token);
                if (Config.ContainsKey(key))
                {
                    object ob = null;
                    Config.TryGetValue(key, out ob);
                    if (
                        ob is JsonElement json
                        && (
                            json.ValueKind == JsonValueKind.Object
                            || json.ValueKind == JsonValueKind.Array
                        )
                    )
                    {
                        return json.Deserialize<T>();
                    }
                    else
                    {
                        throw new Exception("值类型请使用ReadConfig方法");
                    }
                }
                else
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task SaveConfig<T>(string key, T value,CancellationToken token = default)
        {
            try
            {
                await refresh(token);
                if (Config.ContainsKey(key))
                {
                    Config[key] = value;
                }
                else
                {
                    Config.Add(key, value);
                }
                await SaveAsync(token);
            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task SaveAsync(CancellationToken token =default)
        {
            await File.WriteAllTextAsync(
                Path.Combine(LocalSettingFolder, LocalSettingFileName),
                JsonSerializer.Serialize(Config),token
            );
        }

        public async Task<bool> DelectConfig(string key, CancellationToken token = default)
        {
            if (Config.ContainsKey(key))
            {
                Config.Remove(key);
                await SaveAsync(token);
                return true;
            }
            return false;
        }
    }
}

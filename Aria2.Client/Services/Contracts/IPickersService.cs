using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Aria2.Client.Services.Contracts;

public interface IPickersService
{
    public Task<StorageFolder> GetFolderPicker();

    public FileOpenPicker GetFileOpenPicker();

    public FileSavePicker GetFileSavePicker();
}

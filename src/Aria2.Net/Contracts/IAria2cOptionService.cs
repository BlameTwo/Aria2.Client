using Aria2.Net.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Net.Contracts
{
    public interface IAria2cOptionService
    {
        /// <summary>
        /// 设置最大上传速度
        /// </summary>
        /// <param name="value">以单位字幕结尾，例如50K,12M等</param>
        /// <returns></returns>
        public Task<ResultCode<string>> ChangeMaxUploadAsync(string value,CancellationToken token);

        /// <summary>
        /// 设置最大下载速度
        /// </summary>
        /// <param name="value">以单位字幕结尾，例如50K,12M等</param>
        /// <returns></returns>
        public Task<ResultCode<string>> ChangeMaxDownloadAsync(string value, CancellationToken token);
    }
}

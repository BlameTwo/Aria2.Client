using System.Net;
using System.Net.Http;
using System.Text;

namespace BtSearch.Fitgril.Providers;

public class HttpClientProvider
{
    public HttpClientProvider(bool isClear)
    {
        if (isClear)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            Client = new HttpClient(handler);
            return;
        }
        Client = new HttpClient();
    }
    public HttpClient Client { get; private set; }



    public void SetCookie(string cookie)
    {
        SetHeader("cookie", cookie);
    }

    void SetHeader(string key, string value)
    {
        Client.DefaultRequestHeaders.Add(key, value);
    }

}

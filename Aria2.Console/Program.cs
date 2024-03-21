// 创建一个新的Web客户端实例
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;

using (var client = new WebClient())
{
    // 下载RSS源的XML内容
    string rssXml = client.DownloadString("https://dmhy.b168.net/topics/rss/rss.xml");
    // 创建一个XmlReader来解析下载的XML
    XmlReader reader = XmlReader.Create(new StringReader(rssXml));
    // 使用SyndicationFeed加载RSS源
    SyndicationFeed feed = SyndicationFeed.Load(reader);
    if (feed != null)
    {
        // 遍历每个条目并打印标题、链接等信息
        foreach (SyndicationItem item in feed.Items)
        {
            
            Console.WriteLine("Title: {0}", item.Title.Text);
            Console.WriteLine("Link: {0}", item.Links.FirstOrDefault()?.Uri.AbsoluteUri ?? "N/A");
            if (item.Summary != null)
            {
                Console.WriteLine("Summary: {0}", item.Summary.Text);
            }
            Console.WriteLine();
        }
    }
    else
    {
        Console.WriteLine("未能成功加载RSS源");
    }

    // 关闭XmlReader
    reader.Close();
}
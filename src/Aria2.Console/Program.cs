// 创建一个新的Web客户端实例
using HtmlAgilityPack;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;

var html= await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory+"1.html");
HtmlDocument doc = new();
doc.LoadHtml(html);
var nodes = doc.DocumentNode.SelectNodes("//article");
foreach (var node in nodes)
{
    var header = node.SelectSingleNode(".//header//h1/a").InnerText;
    var subtitle = node.SelectSingleNode(".//div/p").InnerText;
    var time = node.SelectSingleNode(".//div[2]/span/a/time");
    var time2 = DateTime.Parse(time.GetAttributeValue<string>("datetime","string"));
}
Console.ReadKey();
using IBtSearch.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IBtSearch.Models;

public class BTSearchResult
{
    /// <summary>
    /// 种子名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 种子哈希
    /// </summary>
    public string Hash { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    public string Size { get; set; }

    /// <summary>
    /// 种子简介
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 种子封面
    /// </summary>
    public string Cover { get; set; }

    /// <summary>
    /// 种子链接
    /// </summary>
    public string BTUrl { get; set; }

    /// <summary>
    /// 链接类型
    /// </summary>
    public BtResultType BTResultType { get; set; }

    /// <summary>
    /// 其他信息
    /// </summary>
    public Dictionary<string, object> Session { get; set; }

    public string WebUrl { get; set; }

    public string OrginSource => "Fitgril";

}
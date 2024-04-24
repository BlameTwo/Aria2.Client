### 插件开发

1. 引用程序集
   
   1. `IBtSearch`
      
      1. 搜索插件`IBTSearchPlugin`

2. 实现接口IBtSearchPlugin
   `Guid`:插件UUIID
   `Name`:插件名称
   `Orgin`:搜索插件的源地址
   `IAsyncEnumerable<BTSearchResult> SearchAsync`:搜索方法实现，异步迭代

3. 开发脚本

4. 调试脚本：`Aria2.Test`
   
   1. 测试新建测试进行调试

#### 发布插件

###### 设置生成项目为`Release`模式进行生成。

###### 按照以下方式进行配置插件安装包。

每一个插件都应该有一个Public.xml文件用来表示Host文件位置以及一些基础信

1.Public.xml（注意，这个文件名称必须是完整的大写开头的`Public`的文件）

```xml
<!--Public.xml-->
<?xml version="1.0" encoding="utf-8"?>
<Host>
    <!--插件类型-->
    <PluginType Type="Search"></PluginType>
    <!--插件详细信息Xml-->
    <PluginHostFilePath File="/1337XHost.xml"></PluginHostFilePath>
    <!--插件Icon，不支持Url，只支持压缩包内文件，需指定绝对路径-->
    <PluginIcon Src="1337X/Files/LoGo.png"></PluginIcon>
    <!--插件发布时间-->
    <PluginPublichTime>2024/4/19 17:07:40</PluginPublichTime>
    <!--插件 Version-->
    <Version>1.0 bate</Version>
    <!--插件全部文件-->
    <Files Folder="Files/">
        <!--插件文件列表，Path为绝对路径-->
        <File Type="DLL" IsUsings="true" Path="…………"/>
        <!--………………-->
        <File Type="Text" IsUsings="false" Path="…………"/>
    </Files>
</Host>
```

```xml
<!--PluginHost.xml-->
<?xml version="1.0" encoding="utf-8"?>
<Aria2Plugin>
    <!--插件名称-->
    <Name>1337X</Name>
    <!--GUID-->
    <GUID>BAA7032E-0DB0-2454-124F-E829283C1FA0</GUID>
    <!--WebUrl-->
    <WebHome>https://www.1337xx.to/</WebHome>
    <!--安装许可-->
    <LicenseFile FilePath="License.txt"/>
</Aria2Plugin>
```

完整的插件包结构

- 1337X(顶部文件夹)

- Public.xml

- 1337XHost.xml

- Files
  
  - 各种需要释放到插件文件夹的文件。

你好



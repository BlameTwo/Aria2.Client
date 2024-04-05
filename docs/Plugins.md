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

4. 调试脚本：Aria2.Test
   
   1. `测试`新建测试进行调试

5. 发布插件
   
   1. 讲插件主体合并到Aria2.Client 的注入容器中，在`SearchViewModel`中扩展插件内容。

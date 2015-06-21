Online Store项目简介
====================
Online Store采用了面向领域驱动的经典分层架构，并且为了展示微软.NET技术在企业级应用开发中的应用，
它所使用的第三方组件也几乎都是微软提供的：Entity Framework、ASP.NET MVC、Unity IoC、Unity AOP、Enterprise Library Caching等（用于记录日志的log4net除外，但log4net本身也是众所周知的框架），所以，开发人员只需要打开Online Store的源程序，就能够很清楚地看到系统的各个组件是如何组织在一起并协同工作的。
<br>
<br>
![](http://images0.cnblogs.com/blog2015/383187/201506/131522390199370.png)
<br>
Online Store所使用的技术
====================
Online Store项目使用或涵盖了以下Microsoft技术：
  * Microsoft Entity Framework 6 Code First（包括Repository模式的实现、枚举类型的支持以及分页功能的实现）<br>
  * ASP.NET MVC 4<br>
  * WCF<br>
  * Microsoft Patterns & Practices Unity Application Block<br>
  * Microsoft Patterns & Practices Unity Policy Injection Extension<br>
  * Microsoft Patterns & Practices Caching Application Block<br>
  * Microsoft Appfabric Caching<br>
  * 使用AutoMapper实现DTO与领域对象映射<br>
  * 基于Unity的AOP拦截<br>
  * 使用log4net记录拦截的Exception详细信息<br>

Online Store项目涵盖了以下模式和设计思想：
==
* 实体、值对象、领域服务<br>
* 规约、仓储、仓储上下文<br>
* 领域事件、事件聚合器、事件总线<br>
* 服务定位器模式、工作单元模式、分离接口模式、数据传输对象模式、层超类型模式、传输对象组装器模式<br>

运行Online Store案例
===
先决条件
----
本案例使用Visual Studio 2012开发，因此，要编译本案例的源代码程序，则需要首先安装Visual Studio 2012。由于数据库采用了SQL Server Express LocalDB，因此，这部分组件也需要正确安装（如果是选择完整安装Visual Studio 2012，则可以忽略LocalDB的安装）。 另外，Online Store提供了两种事件总线（Event Bus）的实现：一种是面向事件聚合器（Event Aggregator）的，它将把所获得的事件通过聚合器派发到一个或多个事件处理器上；另一种是面向微软MSMQ的，它将把所获得的事件直接派发到MSMQ队列中，如果采用这种事件总线，则需要在机器上安装和配置MSMQ组件，并确保新建的队列是事务型队列。 此外，无需安装其它组件。
编译运行
-----
克隆源代码资源库，或者直接下载zip压缩包，然后在Microsoft Visual Studio 2012中打开OnlineStore.sln文件，再将OnlineStore..Web项目设置为启动项目后，直接按F5（或者Debug –> Start Debugging菜单项）运行本案例即可。注意：

如果不打算以Debug的方式启动本案例，那就需要首先展开OnlineStore.Application，任选其中一个.svc的服务文件（比如UserService.svc）然后点击右键选择View In Browser菜单项，以便启动服务端的ASP.NET Development Server；最后再直接启动ByteartRetail.Web项目
由于OnlineStore的数据库采用的是SQL Server 2012 Express LocalDB（默认实例），在程序连接LocalDB数据库时，LocalDB需要创建/初始化数据库实例，因此在首次启动时有可能会出现数据库连接超时的异常，如果碰到这类问题，则请稍等片刻然后再重试
登录账户
-----
启动成功后，就可以单击页面右上角的“登录”链接进行账户登录。默认的登录账户有（用户名/密码）：

admin/admin：以管理员角色登录，可以对站点进行管理
sales/sales：以销售人员角色登录，可以查看系统中订单信息并进行发货等操作
buyer/buyer：以采购人员角色登录，可以管理商品分类和商品信息
test/：普通用户角色，不能对系统进行任何管理操作
解决方案结构
------
OnlineStore.sln包含以下项目：
<br>
* OnlineStore.Application：应用层<br>
* OnlineStore.Domain：领域层<br>
* OnlineStore.Repositories：仓储的具体实现（目前是基于Entity Framework 6.0的实现）<br>
* OnlineStore.Events：事件相关的事件处理器、事件总线和事件聚合器的定义<br>
* OnlineStore.Events.Handlers：具体的事件处理器定义<br>
* OnlineStore.Infrastructure：基础结构层<br>
* OnlineStore.ServiceContracts：基于WCF的服务契约<br>
* OnlineStore.Web：基于ASP.NET MVC的站点程序（表示层）<br>

总结
====
热烈欢迎爱好Microsoft.NET技术以及领域驱动设计的读者朋友对本案例进行深入讨论。同时也欢迎访问我的博客：http://www.cnblogs.com/zhili

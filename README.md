## ForceFramework v2.2
+ 这是asp.net core 2.2版本的
+ 结构介绍：
	+ App->具体项目
	+ Common->公用工具类
	+ DataLayer->数据库操作类
	+ Model->数据库实体类，其他实体类
	+ Resource-> 前端页面用到的各种插件，样式,使用的是metronic_v5.1.7的前端样板，静态资源 需要单独部署一个站点（需要打开跨域）
    + 其中DataLayer和Model类通过生成工具生成，生成工具https://github.com/curiousPeng/CodeGenerator
    + 现在使用的是https://github.com/curiousPeng/CodeGenerator-old 支持MySql,SqlServer,Oracle

#### 数据库
+ 支持MySQL和SQL server，理论上CodeGenerator支持生成那种就可以用那种。

#### 缓存
+ MemoryCache,已注入直接可用。
+ Redis,已注入直接可用，Redis进行了小封装，支持对象的直接hashset。

#### orm
+ CodeGenerator生成的是用Dapper,所以用Dapper。

#### 日志
+ NLog

#### 队列
+ RabbitMQ，封装了一下使用的是 * https://github.com/curiousPeng/Tools/tree/master/LightMessager * 项目。
+ demo用法也参考上面的项目

#### 界面
+ ![登录界面](https://github.com/curiousPeng/ForceFramework_MVC/blob/master/doc/Preview/login.jpg)
+ ![首页](https://github.com/curiousPeng/ForceFramework_MVC/blob/master/doc/Preview/index.jpg)
+ ![菜单](https://github.com/curiousPeng/ForceFramework_MVC/blob/master/doc/Preview/menu.jpg)
+ ![用户](https://github.com/curiousPeng/ForceFramework_MVC/blob/master/doc/Preview/user.jpg)
+ ![角色](https://github.com/curiousPeng/ForceFramework_MVC/blob/master/doc/Preview/role.jpg)
+ ![分配权限](https://github.com/curiousPeng/ForceFramework_MVC/blob/master/doc/Preview/roleAuth.jpg)

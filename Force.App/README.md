### Force.App
+ 结构介绍：
	+ Controller->控制器类
	+ Extension->扩展类
	+ Filter->控制器拦截类
	+ Util->其他帮助类
	+ ViewComponents->视图组件类
	+ Views->视图文件
	+ wwwroot->静态文件

#### 权限控制
+ admin类型用户可以直接访问所有controller的action，
+ 每个controller，只有get和post两种请求方式，例如home/add，[HttpGet]属性的代表返回视图，[HttpPost]的代表处理逻辑，返回json格式对象。

+ 权限模块见仁见智，根据不同需求，自己改改，可以加一个角色拥有者字段，其他角色只能更改自己拥有的角色等。


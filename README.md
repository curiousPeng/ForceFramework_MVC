## ForceFramework
+ 结构介绍：
	+ App->具体项目
	+ Common->公用工具类
	+ DataLayer->数据库操作类
	+ Model->数据库实体类，其他实体类
其中DataLayer和Model类通过生成工具生成，生成工具https://github.com/curiousPeng/CodeGenerator

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

#### 文档
+ Swagger


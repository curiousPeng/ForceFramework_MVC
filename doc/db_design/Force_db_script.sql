/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2019/11/18 10:36:27                          */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('RoleAuthMapping')
            and   type = 'U')
   drop table RoleAuthMapping
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SystemMenu')
            and   type = 'U')
   drop table SystemMenu
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SystemRole')
            and   type = 'U')
   drop table SystemRole
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SystemUser')
            and   type = 'U')
   drop table SystemUser
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SystemUserLog')
            and   type = 'U')
   drop table SystemUserLog
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SystemUserRoleMapping')
            and   type = 'U')
   drop table SystemUserRoleMapping
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UploadFileInfo')
            and   type = 'U')
   drop table UploadFileInfo
go

/*==============================================================*/
/* Table: RoleAuthMapping                                       */
/*==============================================================*/
create table RoleAuthMapping (
   Id                   int                  identity(1,1),
   RoleId               int                  not null,
   MenuId               int                  not null,
   CreatedTime          datetime             not null,
   constraint PK_ROLEAUTHMAPPING primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色跟对应权限项之间的关系',
   'user', @CurrentUser, 'table', 'RoleAuthMapping'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'RoleAuthMapping', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色Id',
   'user', @CurrentUser, 'table', 'RoleAuthMapping', 'column', 'RoleId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '菜单Id',
   'user', @CurrentUser, 'table', 'RoleAuthMapping', 'column', 'MenuId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'RoleAuthMapping', 'column', 'CreatedTime'
go

/*==============================================================*/
/* Table: SystemMenu                                            */
/*==============================================================*/
create table SystemMenu (
   Id                   int                  identity(1,1),
   Name                 varchar(50)          not null,
   ParentId             int                  not null,
   ActionRoute          varchar(260)         not null,
   Icon                 varchar(50)          null,
   Type                 smallint             not null,
   Sort                 int                  not null,
   IsUse                bit                  not null,
   Remark               varchar(200)         null,
   CreatedTime          datetime             not null default getdate(),
   constraint PK_SYSTEMMENU primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '菜单列表',
   'user', @CurrentUser, 'table', 'SystemMenu'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '名称',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Name'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '父级Id',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'ParentId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Action路由',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'ActionRoute'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '图标',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Icon'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '类型 [1 菜单 2 新增 3 编辑 4 删除 5 查询 6 页面]',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Type'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '排序',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Sort'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否使用',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'IsUse'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Remark'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'CreatedTime'
go

/*==============================================================*/
/* Table: SystemRole                                            */
/*==============================================================*/
create table SystemRole (
   Id                   int                  identity(1,1),
   Name                 varchar(50)          not null,
   Remark               varchar(10)          null,
   CreatedTime          datetime             not null,
   constraint PK_SYSTEMROLE primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '系统角色信息',
   'user', @CurrentUser, 'table', 'SystemRole'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'SystemRole', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色名称',
   'user', @CurrentUser, 'table', 'SystemRole', 'column', 'Name'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'SystemRole', 'column', 'Remark'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'SystemRole', 'column', 'CreatedTime'
go

/*==============================================================*/
/* Table: SystemUser                                            */
/*==============================================================*/
create table SystemUser (
   Id                   int                  identity(1,1),
   Account              varchar(50)          not null,
   NickName             varchar(50)          null,
   Email                varchar(50)          null,
   Password             varchar(50)          not null,
   Phone                varchar(15)          not null,
   HeadImage            varchar(260)         not null,
   Status               smallint             not null,
   CreatedTime          datetime             not null,
   constraint PK_SYSTEMUSER primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '包含后台用户数据',
   'user', @CurrentUser, 'table', 'SystemUser'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '账户',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Account'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '昵称',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'NickName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '邮箱',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Email'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '密码',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Password'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '手机',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Phone'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '头像',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'HeadImage'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '状态[1 正常 2 冻结]',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Status'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'CreatedTime'
go

/*==============================================================*/
/* Table: SystemUserLog                                         */
/*==============================================================*/
create table SystemUserLog (
   Id                   int                  identity(1,1),
   SystemUserId         int                  not null,
   SystemUserName       varchar(50)          not null,
   ActionRoute          varchar(260)         not null,
   Details              varchar(500)         not null,
   Type                 smallint             not null,
   IP                   varchar(50)          not null,
   CreatedTime          datetime             not null,
   constraint PK_SYSTEMUSERLOG primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户操作日志',
   'user', @CurrentUser, 'table', 'SystemUserLog'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户id',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'SystemUserId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户名称',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'SystemUserName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Action 路由',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'ActionRoute'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '操作详情',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'Details'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '类型 [1 后台登录退出 2 菜单操作 3 角色操作 4 操作用户 5 后台其他]',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'Type'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'IP',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'IP'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'CreatedTime'
go

/*==============================================================*/
/* Table: SystemUserRoleMapping                                 */
/*==============================================================*/
create table SystemUserRoleMapping (
   Id                   int                  identity(1,1),
   SystemUserId         int                  not null,
   RoleId               int                  not null,
   CreatedTime          datetime             not null,
   constraint PK_SYSTEMUSERROLEMAPPING primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户与所对应的角色之间的关系',
   'user', @CurrentUser, 'table', 'SystemUserRoleMapping'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'SystemUserRoleMapping', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户Id',
   'user', @CurrentUser, 'table', 'SystemUserRoleMapping', 'column', 'SystemUserId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色Id',
   'user', @CurrentUser, 'table', 'SystemUserRoleMapping', 'column', 'RoleId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'SystemUserRoleMapping', 'column', 'CreatedTime'
go

/*==============================================================*/
/* Table: UploadFileInfo                                        */
/*==============================================================*/
create table UploadFileInfo (
   Id                   int                  identity(1,1),
   Name                 varchar(50)          not null,
   URL                  varchar(500)         not null,
   Source               smallint             not null,
   Type                 varchar(10)          not null,
   IsUse                bit                  not null,
   HashVal              varchar(200)         not null,
   PhysicalPath         int                  not null,
   CreatedTime          datetime             not null,
   constraint PK_UPLOADFILEINFO primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用于记录上传的文件（可能是图片、apk包等等）',
   'user', @CurrentUser, 'table', 'UploadFileInfo'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '名称',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'Name'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'URL',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'URL'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '来源[1 admin后台 2 agent代理系统 3 商户系统]',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'Source'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '文件类型',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'Type'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否在使用',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'IsUse'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Hash值',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'HashVal'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '物理路径',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'PhysicalPath'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'CreatedTime'
go


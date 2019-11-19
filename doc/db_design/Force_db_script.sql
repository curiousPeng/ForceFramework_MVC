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
   '��ɫ����ӦȨ����֮��Ĺ�ϵ',
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
   '��ɫId',
   'user', @CurrentUser, 'table', 'RoleAuthMapping', 'column', 'RoleId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�˵�Id',
   'user', @CurrentUser, 'table', 'RoleAuthMapping', 'column', 'MenuId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
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
   '�˵��б�',
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
   '����',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Name'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����Id',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'ParentId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Action·��',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'ActionRoute'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ͼ��',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Icon'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '���� [1 �˵� 2 ���� 3 �༭ 4 ɾ�� 5 ��ѯ 6 ҳ��]',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Type'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Sort'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�Ƿ�ʹ��',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'IsUse'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ע',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Remark'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
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
   'ϵͳ��ɫ��Ϣ',
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
   '��ɫ����',
   'user', @CurrentUser, 'table', 'SystemRole', 'column', 'Name'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ע',
   'user', @CurrentUser, 'table', 'SystemRole', 'column', 'Remark'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
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
   '������̨�û�����',
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
   '�˻�',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Account'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�ǳ�',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'NickName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Email'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Password'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�ֻ�',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Phone'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ͷ��',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'HeadImage'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '״̬[1 ���� 2 ����]',
   'user', @CurrentUser, 'table', 'SystemUser', 'column', 'Status'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
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
   '�û�������־',
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
   '�û�id',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'SystemUserId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�û�����',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'SystemUserName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Action ·��',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'ActionRoute'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��������',
   'user', @CurrentUser, 'table', 'SystemUserLog', 'column', 'Details'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '���� [1 ��̨��¼�˳� 2 �˵����� 3 ��ɫ���� 4 �����û� 5 ��̨����]',
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
   '����ʱ��',
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
   '�û�������Ӧ�Ľ�ɫ֮��Ĺ�ϵ',
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
   '�û�Id',
   'user', @CurrentUser, 'table', 'SystemUserRoleMapping', 'column', 'SystemUserId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ɫId',
   'user', @CurrentUser, 'table', 'SystemUserRoleMapping', 'column', 'RoleId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
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
   '���ڼ�¼�ϴ����ļ���������ͼƬ��apk���ȵȣ�',
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
   '����',
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
   '��Դ[1 admin��̨ 2 agent����ϵͳ 3 �̻�ϵͳ]',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'Source'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�ļ�����',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'Type'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�Ƿ���ʹ��',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'IsUse'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Hashֵ',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'HashVal'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����·��',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'PhysicalPath'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
   'user', @CurrentUser, 'table', 'UploadFileInfo', 'column', 'CreatedTime'
go


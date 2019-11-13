USE [force]
GO
SET IDENTITY_INSERT [dbo].[RoleAuthMapping] ON 

GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (269, 1, 1, CAST(0x0000AA0700F334D9 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (270, 1, 2, CAST(0x0000AA0700F334DA AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (276, 1, 15, CAST(0x0000AA0700F334DC AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (297, 1, 67, CAST(0x0000AA0700F334E3 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (308, 1, 46, CAST(0x0000AA0700F334E7 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (309, 1, 47, CAST(0x0000AA0700F334E8 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (310, 1, 48, CAST(0x0000AA0700F334E8 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (311, 1, 49, CAST(0x0000AA0700F334E9 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (312, 1, 51, CAST(0x0000AA0700F334E9 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (313, 1, 52, CAST(0x0000AA0700F334E9 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (314, 1, 55, CAST(0x0000AA0700F334EA AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (315, 1, 56, CAST(0x0000AA0700F334EA AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (316, 1, 57, CAST(0x0000AA0700F334EA AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (317, 1, 61, CAST(0x0000AA0700F334EB AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (318, 1, 58, CAST(0x0000AA0700F334EB AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (319, 1, 62, CAST(0x0000AA0700F334EC AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (320, 1, 65, CAST(0x0000AA0700F334EC AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (321, 1, 66, CAST(0x0000AA0700F334EC AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (326, 1, 50, CAST(0x0000AA0700F334EE AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (327, 1, 53, CAST(0x0000AA0700F334EF AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (328, 1, 54, CAST(0x0000AA0700F334EF AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (329, 1, 59, CAST(0x0000AA0700F334F0 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (330, 1, 60, CAST(0x0000AA0700F334F0 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (331, 1, 63, CAST(0x0000AA0700F334F0 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (332, 1, 64, CAST(0x0000AA0700F334F1 AS DateTime))
GO
INSERT [dbo].[RoleAuthMapping] ([Id], [RoleId], [MenuId], [CreatedTime]) VALUES (333, 1, 68, CAST(0x0000AA0700F334F1 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[RoleAuthMapping] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemMenu] ON 

GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (1, N'系统菜单', 0, N'1,', N' ', N'flaticon-layers', 1, 1, 1, 1, N'系统菜单', CAST(0x0000A9FD00000000 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (2, N'首页', 0, N'2', N'javascript:;', N'flaticon-line-graph', 5, 2, 4, 1, N'', CAST(0x0000A9FD00AFEFFE AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (15, N'系统管理', 1, N'1,15,', N'javascript:;', N'icon-settings', 1, 8, 4, 1, N'', CAST(0x0000A9FD00BD84D1 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (16, N'用户管理', 15, N'1,15,16,', N'/systemuser/userlist', N'fa fa-user-md', 1, 1, 5, 1, N'', CAST(0x0000A9FD00BD988B AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (17, N'菜单管理', 15, N'1,15,17,', N'/menu/menuList', N'fa fa-user-md', 1, 2, 5, 1, N'', CAST(0x0000A9FD00BDB1A3 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (18, N'角色管理', 15, N'1,15,18,', N'/systemrole/roleList', N'fa fa-user-md', 1, 3, 5, 1, N'', CAST(0x0000A9FD00BDCEE4 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (19, N'系统日志', 15, N'1,15,19,', N'/operationallog/index', N'fa fa-user-md', 1, 4, 5, 1, N'', CAST(0x0000A9FD00BDE28C AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (20, N'用户信息', 15, N'1,15,20,', N'/systemuser/userInfo', N'fa fa-user-md', 1, 5, 5, 1, N'', CAST(0x0000A9FD00BDFA78 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (46, N'用户管理 查询 功能', 16, N'1,15,16,46,', N'/systemuser/getuserlist', N'', 5, 1, 6, 1, N'', CAST(0x0000A9FD00E85F12 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (47, N'用户管理 开启/关闭 功能', 16, N'1,15,16,47,', N'/systemuser/opstatus', N'', 3, 2, 6, 1, N'', CAST(0x0000A9FD00E90D98 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (48, N'用户管理 删除 功能', 16, N'1,15,16,48,', N'/systemuser/opdelete', N'', 4, 3, 6, 1, N'', CAST(0x0000A9FD00E94E4F AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (49, N'用户管理 添加/编辑 页面', 16, N'1,15,16,49,', N'/systemuser/useredit', N'', 6, 4, 6, 1, N'', CAST(0x0000A9FD00E9AE46 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (50, N'用户管理 添加/编辑 功能', 49, N'1,15,16,49,50,', N'/systemuser/opedit', N'', 3, 1, 7, 1, N'', CAST(0x0000A9FD00E9C9D5 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (51, N'菜单管理 查询 功能', 17, N'1,15,17,51,', N'/menu/getmenulist', N'', 5, 1, 6, 1, N'', CAST(0x0000A9FD00EA10EB AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (52, N'菜单管理 添加/编辑 页面', 17, N'1,15,17,52,', N'/menu/menuedit', N'', 6, 2, 6, 1, N'', CAST(0x0000A9FD00EA398C AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (53, N'菜单管理 添加 功能', 52, N'1,15,17,52,53,', N'/menu/opadd', N'', 2, 1, 7, 1, N'', CAST(0x0000A9FD00EA8AE7 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (54, N'菜单管理 编辑 功能', 52, N'1,15,17,52,54,', N'/menu/opedit', N'', 3, 2, 7, 1, N'', CAST(0x0000A9FD00EAA967 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (55, N'菜单管理 删除 功能', 17, N'1,15,17,55,', N'/menu/opdelete', N'', 4, 3, 6, 1, N'', CAST(0x0000A9FD00ECA93C AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (56, N'菜单管理 开启/关闭 功能', 17, N'1,15,17,56,', N'/menu/opisuse', N'', 3, 4, 6, 1, N'', CAST(0x0000A9FD00ECF7F1 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (57, N'角色管理 查询 功能', 18, N'1,15,18,57,', N'/systemrole/getrolelist', N'', 5, 1, 6, 1, N'', CAST(0x0000A9FD00ED773E AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (58, N'角色管理 添加/编辑 页面', 18, N'1,15,18,58,', N'/systemrole/roleedit', N'', 6, 3, 6, 1, N'', CAST(0x0000A9FD00EDF142 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (59, N'角色管理 添加 功能', 58, N'1,15,18,58,59,', N'/systemrole/opadd', N'', 2, 1, 7, 1, N'', CAST(0x0000A9FD00EE3007 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (60, N'角色管理 编辑 功能', 58, N'1,15,18,58,60,', N'/systemrole/opedit', N'', 3, 2, 7, 1, N'', CAST(0x0000A9FD00EE48DA AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (61, N'角色管理 删除 功能', 18, N'1,15,18,61,', N'/systemrole/opdelete', N'', 4, 2, 6, 1, N'', CAST(0x0000A9FD00EE6AFC AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (62, N'角色管理 绑定 权限 页面', 18, N'1,15,18,62,', N'/systemrole/rolemenu', N'', 6, 4, 6, 1, N'', CAST(0x0000A9FD00EEFDFE AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (63, N'角色管理 绑定 权限  查询功能', 62, N'1,15,18,62,63,', N'/systemrole/getrolemenu', N'', 5, 1, 7, 1, N'', CAST(0x0000A9FD00EF1EE6 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (64, N'角色管理 绑定 权限绑定 功能', 62, N'1,15,18,62,64,', N'/systemrole/rolemenuedit', N'', 3, 2, 7, 1, N'', CAST(0x0000A9FD00EF4D03 AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (65, N'系统日志 查询 功能', 19, N'1,15,19,65,', N'/operationallog/getoperationalloglist', N'', 5, 1, 6, 1, N'', CAST(0x0000A9FD00EF73BB AS DateTime))
GO
INSERT [dbo].[SystemMenu] ([Id], [Name], [ParentId], [ParentList], [ActionRoute], [Icon], [Type], [Sort], [Depth], [IsUse], [Remark], [CreatedTime]) VALUES (66, N'用户信息 编辑 功能', 20, N'1,15,20,66,', N'/systemuser/opuseredit', N'', 3, 1, 6, 1, N'', CAST(0x0000A9FD00F02B0B AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[SystemMenu] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemRole] ON 

GO
INSERT [dbo].[SystemRole] ([Id], [Name], [CreatedTime]) VALUES (1, N'系统角色', CAST(0x0000A9FD00C1647F AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[SystemRole] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemUser] ON 

GO
INSERT [dbo].[SystemUser] ([Id], [Account], [Email], [Password], [Phone], [HeadImage], [Status], [CreatedTime]) VALUES (1, N'admin', N'12456@qq.com', N'e10adc3949ba59abbe56e057f20f883e', N'18888888888', N'1', 1, CAST(0x0000A9F300000000 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[SystemUser] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemUserRoleMapping] ON 

GO
INSERT [dbo].[SystemUserRoleMapping] ([Id], [SystemUserId], [RoleId], [CreatedTime]) VALUES (1, 1, 1, CAST(0x0000A9FD00C171B9 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[SystemUserRoleMapping] OFF
GO

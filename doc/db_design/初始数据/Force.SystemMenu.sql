INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (2, '首页', 0, '/home/index', 'icon-home', 1, 0, 1, '首页', '2019-02-23 10:40:34.127')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (3, '权限菜单', 0, 'javascript:void(0)', 'icon-notebook', 1, 1, 1, '菜单设置', '2019-11-19 15:42:46.267')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (4, '菜单新增', 3, '/systemmenu/create', NULL, 2, 2, 1, '菜单新增', '2019-11-19 15:47:39.597')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (5, '菜单修改', 3, '/systemmenu/edit', NULL, 3, 3, 1, '菜单修改', '2019-11-19 17:12:45.593')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (6, '菜单状态快捷修改', 3, '/systemmenu/changestatus', NULL, 3, 4, 1, '菜单状态快捷修改', '2019-11-19 17:13:39.177')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (7, '系统用户管理', 0, 'javascript:void(0)', 'icon-users', 1, 1, 1, '系统用户管理', '2019-11-20 17:29:02.917')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (8, '系统用户新增', 7, '/systemuser/create', NULL, 2, 1, 1, '系统用户新增', '2019-11-26 11:34:35.793')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (9, '系统用户修改', 7, '/systemuser/edit', NULL, 3, 2, 1, NULL, '2019-11-26 11:35:11.143')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (10, '状态快捷设置', 7, '/systemuser/changestatus', NULL, 3, 3, 1, NULL, '2019-11-26 11:35:58.45')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (11, '角色管理', 0, 'javascript:void(0)', 'icon-shield ', 1, 3, 1, NULL, '2019-11-27 15:18:57.623')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (12, '角色新增', 11, '/systemrole/create', NULL, 2, 1, 1, NULL, '2019-11-27 16:04:10.99')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (13, '角色修改', 11, '/systemrole/edit', NULL, 3, 2, 1, NULL, '2019-11-27 16:04:35.03')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (14, '角色删除', 11, '/systemrole/delete', NULL, 4, 3, 1, NULL, '2019-11-27 16:05:10.997')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (15, '权限绑定', 11, '/systemrole/rolemenu', NULL, 3, 4, 1, NULL, '2019-11-28 15:33:40.847')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (16, '权限菜单拉取', 15, '/systemrole/menu', NULL, 3, 5, 1, NULL, '2019-11-28 15:39:19.627')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (17, '菜单列表', 3, '/systemmenu/index', 'icon-book-open', 1, 1, 1, NULL, '2019-11-28 16:07:36.183')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (18, '用户列表', 7, '/systemuser/index', 'icon-user', 1, 0, 1, NULL, '2019-11-29 10:12:31.79')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (19, '角色列表', 11, '/systemrole/index', 'icon-lock', 1, 0, 1, NULL, '2019-11-29 10:16:31.06')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (20, '角色分配', 7, '/systemuser/userrole', NULL, 3, 5, 1, NULL, '2019-11-29 14:24:41.767')
GO


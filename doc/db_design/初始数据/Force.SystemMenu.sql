INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (2, '��ҳ', 0, '/home/index', 'icon-home', 1, 0, 1, '��ҳ', '2019-02-23 10:40:34.127')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (3, 'Ȩ�޲˵�', 0, 'javascript:void(0)', 'icon-notebook', 1, 1, 1, '�˵�����', '2019-11-19 15:42:46.267')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (4, '�˵�����', 3, '/systemmenu/create', NULL, 2, 2, 1, '�˵�����', '2019-11-19 15:47:39.597')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (5, '�˵��޸�', 3, '/systemmenu/edit', NULL, 3, 3, 1, '�˵��޸�', '2019-11-19 17:12:45.593')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (6, '�˵�״̬����޸�', 3, '/systemmenu/changestatus', NULL, 3, 4, 1, '�˵�״̬����޸�', '2019-11-19 17:13:39.177')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (7, 'ϵͳ�û�����', 0, 'javascript:void(0)', 'icon-users', 1, 1, 1, 'ϵͳ�û�����', '2019-11-20 17:29:02.917')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (8, 'ϵͳ�û�����', 7, '/systemuser/create', NULL, 2, 1, 1, 'ϵͳ�û�����', '2019-11-26 11:34:35.793')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (9, 'ϵͳ�û��޸�', 7, '/systemuser/edit', NULL, 3, 2, 1, NULL, '2019-11-26 11:35:11.143')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (10, '״̬�������', 7, '/systemuser/changestatus', NULL, 3, 3, 1, NULL, '2019-11-26 11:35:58.45')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (11, '��ɫ����', 0, 'javascript:void(0)', 'icon-shield ', 1, 3, 1, NULL, '2019-11-27 15:18:57.623')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (12, '��ɫ����', 11, '/systemrole/create', NULL, 2, 1, 1, NULL, '2019-11-27 16:04:10.99')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (13, '��ɫ�޸�', 11, '/systemrole/edit', NULL, 3, 2, 1, NULL, '2019-11-27 16:04:35.03')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (14, '��ɫɾ��', 11, '/systemrole/delete', NULL, 4, 3, 1, NULL, '2019-11-27 16:05:10.997')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (15, 'Ȩ�ް�', 11, '/systemrole/rolemenu', NULL, 3, 4, 1, NULL, '2019-11-28 15:33:40.847')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (16, 'Ȩ�޲˵���ȡ', 15, '/systemrole/menu', NULL, 3, 5, 1, NULL, '2019-11-28 15:39:19.627')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (17, '�˵��б�', 3, '/systemmenu/index', 'icon-book-open', 1, 1, 1, NULL, '2019-11-28 16:07:36.183')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (18, '�û��б�', 7, '/systemuser/index', 'icon-user', 1, 0, 1, NULL, '2019-11-29 10:12:31.79')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (19, '��ɫ�б�', 11, '/systemrole/index', 'icon-lock', 1, 0, 1, NULL, '2019-11-29 10:16:31.06')
GO

INSERT INTO SystemMenu (Id, Name, ParentId, ActionRoute, Icon, Type, Sort, IsUse, Remark, CreatedTime) VALUES (20, '��ɫ����', 7, '/systemuser/userrole', NULL, 3, 5, 1, NULL, '2019-11-29 14:24:41.767')
GO


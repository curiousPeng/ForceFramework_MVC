SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MessageQueue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MsgHash] [bigint] NOT NULL,
	[MsgContent] [nvarchar](500) NOT NULL,
	[Status] [smallint] NOT NULL,
	[RetryCount] [int] NOT NULL,
	[LastRetryTime] [datetime] NULL,
	[CanBeRemoved] [bit] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Id] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_MsgHashs] ON [dbo].[MessageQueue]
(
	[MsgHash] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息的hash值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue', @level2type=N'COLUMN',@level2name=N'MsgHash'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue', @level2type=N'COLUMN',@level2name=N'MsgContent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态[1 Created 2 Retrying 3 ArrivedBroker 4 ArrivedConsumer 5 Exception 6 Processed]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重试次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue', @level2type=N'COLUMN',@level2name=N'RetryCount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近重试时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue', @level2type=N'COLUMN',@level2name=N'LastRetryTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'能否被删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue', @level2type=N'COLUMN',@level2name=N'CanBeRemoved'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue', @level2type=N'COLUMN',@level2name=N'CreatedTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LightMessager专用消息落地表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageQueue'
GO
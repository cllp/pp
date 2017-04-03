CREATE TABLE [dbo].[Log]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[UserId] int NULL,
	[Type] nvarchar (50),
	[Date] DateTime2 NOT NULL DEFAULT GETUTCDATE(),
	[Message] nvarchar (max)
)

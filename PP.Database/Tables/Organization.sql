CREATE TABLE [dbo].[Organization]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[Name] nvarchar (255),
	[County] nvarchar (255),
	[Domain] nvarchar (500)
)

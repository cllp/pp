CREATE TABLE [dbo].[Settings]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[Name] nvarchar (50),
	[Type] nvarchar (50), --ide: projekt område, ex e-tjänst
	[Description] nvarchar (255),
	[Value] nvarchar (500)
)

CREATE TABLE [dbo].[ProjectRole]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[Name] nvarchar (50),
	[Description] nvarchar (255),
	[PermissionId] int NOT NULL DEFAULT 0,
	[SortOrder] int not null default 0,
	[ProjectRoleGroupId] INT NOT NULL Foreign key references [ProjectRoleGroup]([Id])
)

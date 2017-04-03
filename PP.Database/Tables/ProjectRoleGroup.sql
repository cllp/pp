CREATE TABLE [dbo].[ProjectRoleGroup]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[Name] nvarchar (50),
	[Description] nvarchar (255),
	[SortOrder] int not null default 0,
	[ProjectCommentTypeId] INT NOT NULL Foreign key references [ProjectCommentType]([Id]), 
)


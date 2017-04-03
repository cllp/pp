CREATE TABLE [dbo].[ProjectCommentTypePermission]
(
	[ProjectCommentTypeId] int NOT NULL Foreign key references [ProjectCommentType]([Id]),
	[ProjectRoleGroupId] int NOT NULL Foreign key references [ProjectRoleGroup]([Id]),
	CONSTRAINT [PROJECTCOMMENTTYPEPERMISSION_CONSTRAINT] UNIQUE CLUSTERED
	(
		[ProjectCommentTypeId], [ProjectRoleGroupId]
	)
)

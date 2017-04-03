CREATE TABLE [dbo].[ProjectRoleAdministration]
(
    [RoleId] INT NOT NULL Foreign key references [ProjectRole]([Id]), 
    [RoleGroupId] INT NOT NULL Foreign key references [ProjectRoleGroup]([Id]),
	CONSTRAINT [PROJECTROLEADMIN_CONSTRAINT] UNIQUE CLUSTERED
	(
		[RoleId], [RoleGroupId]
	)
)

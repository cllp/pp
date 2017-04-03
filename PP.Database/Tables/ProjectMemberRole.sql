CREATE TABLE [dbo].[ProjectMemberRole]
(
	[ProjectMemberId] int NOT NULL Foreign key references [ProjectMember]([Id]),
	[ProjectRoleId] int NOT NULL Foreign key references [ProjectRole]([Id]),
	CONSTRAINT [PROJECTROLE_CONSTRAINT] UNIQUE CLUSTERED
	(
		[ProjectMemberId], [ProjectRoleId]
	)
)
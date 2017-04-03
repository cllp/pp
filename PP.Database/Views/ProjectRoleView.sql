CREATE VIEW [dbo].[ProjectRoleView]
	AS 
	SELECT 
	u.Email as UserEmail,
	u.Id as UserId,
	p.Id as ProjectId,
	pr.Id as ProjectRoleId,
	pr.Name as ProjectRoleName,
	pr.[Description] as ProjectRoleDescription,
	pr.PermissionId as PermissionId,
	pr.ProjectRoleGroupId as ProjectRoleGroupId,
	prg.Name as ProjectRoleGroupName
	FROM [Project] p
	INNER JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
	INNER JOIN [User] u ON u.Id = pm.[UserId]
	INNER JOIN [ProjectMemberRole] pmr ON pm.Id = pmr.[ProjectMemberId]
	INNER JOIN [ProjectRole] pr ON pr.Id = pmr.[ProjectRoleId]
	INNER JOIN [ProjectRoleGroup] prg ON prg.Id = pr.[ProjectRoleGroupId]
GO
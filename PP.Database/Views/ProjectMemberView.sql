CREATE VIEW [dbo].[ProjectMemberView]
	AS 
	SELECT 
	ISNULL(u.Email, '') as UserEmail,
	ISNULL(u.Name, '') as UserName,
	u.[Password],
	u.Id as UserId,
	u.RoleId as RoleId,
	p.Id as ProjectId,
	pm.Id as ProjectMemberId,
	CASE WHEN o.Domain IS NULL THEN 'External' ELSE 'Internal' END as [OrganizationState],
	CASE WHEN o.Domain IS NULL THEN '' ELSE (SELECT TOP 1 Name FROM Organization WHERE Domain = o.Domain) END as [Organization],
	CASE WHEN o.Domain IS NULL THEN 0 ELSE (SELECT TOP 1 Id FROM Organization WHERE Domain = o.Domain) END as [OrganizationId],
	CASE WHEN o.Domain IS NULL THEN '' ELSE (SELECT TOP 1 County FROM Organization WHERE Domain = o.Domain) END as [County],
	CASE WHEN o.Domain IS NULL THEN '' ELSE (SELECT TOP 1 Domain FROM Organization WHERE Domain = o.Domain) END as [Domain]
	FROM [Project] p
	INNER JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
	INNER JOIN [User] u ON u.Id = pm.[UserId]
	LEFT JOIN Organization o ON SUBSTRING(u.Email, CHARINDEX('@', u.Email) + 1, len(u.Email)) = o.Domain
	GO


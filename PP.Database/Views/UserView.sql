CREATE VIEW [dbo].[UserView]
	AS 
	SELECT 
	u.[Id],
  	u.[Name], 
	u.[Email],
	--CAST(u.[Password] as NVARCHAR(50)) as Password,
	--dbo.GetPasswordHash(u.[Password]) as Password,
	u.[RoleId],
	u.ChangePasswordRequest,
	CASE WHEN o.Domain IS NULL THEN 'External' ELSE 'Internal' END as [OrganizationState],
	CASE WHEN o.Domain IS NULL THEN '' ELSE (SELECT TOP 1 Name FROM Organization WHERE Domain = o.Domain) END as [Organization],
	CASE WHEN o.Domain IS NULL THEN 0 ELSE (SELECT TOP 1 Id FROM Organization WHERE Domain = o.Domain) END as [OrganizationId],
	CASE WHEN o.Domain IS NULL THEN '' ELSE (SELECT TOP 1 County FROM Organization WHERE Domain = o.Domain) END as [County],
	CASE WHEN o.Domain IS NULL THEN '' ELSE (SELECT TOP 1 Domain FROM Organization WHERE Domain = o.Domain) END as [Domain]
	FROM [User] u
	LEFT JOIN Organization o ON SUBSTRING(u.Email, CHARINDEX('@', u.Email) + 1, len(u.Email)) = o.Domain
GO



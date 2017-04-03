CREATE PROCEDURE [dbo].[GetInActiveProjects]
	
AS
BEGIN
	SELECT 
	p.Id,
	p.Name, 
	p.PhaseId,
	p.TypeId,
	0 as Favorite,
	0 as Member,
	Cast(CASE WHEN pv.Id IS NULL THEN 0 ELSE 1 END AS BIT) as HasPublishedVersion,
	pa.*,
	prog.*,
	o.*
	FROM Project p 
	INNER JOIN ProjectArea pa ON p.ProjectAreaId = pa.Id
	INNER JOIN Program prog ON pa.ProgramId = prog.Id
	INNER JOIN Organization o ON p.OrganizationId = o.Id
	OUTER APPLY (SELECT TOP 1 * FROM ProjectVersion WHERE ProjectId = p.Id) AS pv
	WHERE p.Active = 0
END
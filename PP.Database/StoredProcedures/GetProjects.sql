CREATE PROCEDURE [dbo].[GetProjects]
	@UserId int = 0,
	@ActiveState int
AS
BEGIN

IF (@ActiveState = 2)
BEGIN

	SELECT 
	p.Id,
	p.Name, 
	p.PhaseId,
	p.TypeId,
	Cast(CASE WHEN pf.Id IS NULL THEN 0 ELSE 1 END AS BIT) as Favorite,
	Cast(CASE WHEN pmv.UserId IS NULL THEN 0 ELSE 1 END AS BIT) as Member,
	p.Active,
	Cast(CASE WHEN pv.Id IS NULL THEN 0 ELSE 1 END AS BIT) as HasPublishedVersion,
	pa.*,
	prog.*,
	o.*
	FROM Project p 
	INNER JOIN ProjectArea pa ON p.ProjectAreaId = pa.Id
	INNER JOIN Program prog ON pa.ProgramId = prog.Id
	INNER JOIN Organization o ON p.OrganizationId = o.Id
	INNER JOIN [UserView] uv ON p.CreatedById = uv.Id
	OUTER APPLY (SELECT TOP 1 * FROM ProjectFavorite WHERE p.Id = ProjectId AND UserId = @UserId) AS pf
	OUTER APPLY (SELECT TOP 1 * FROM ProjectMemberView WHERE p.Id = ProjectId AND UserId = @UserId) AS pmv
	OUTER APPLY (SELECT TOP 1 * FROM ProjectVersion WHERE ProjectId = p.Id) AS pv
END
ELSE
	BEGIN
		SELECT 
		p.Id,
		p.Name, 
		p.PhaseId,
		p.TypeId,
		Cast(CASE WHEN pf.Id IS NULL THEN 0 ELSE 1 END AS BIT) as Favorite,
		Cast(CASE WHEN pmv.UserId IS NULL THEN 0 ELSE 1 END AS BIT) as Member,
		p.Active,
		Cast(CASE WHEN pv.Id IS NULL THEN 0 ELSE 1 END AS BIT) as HasPublishedVersion,
		pa.*,
		prog.*,
		o.*
		FROM Project p 
		INNER JOIN ProjectArea pa ON p.ProjectAreaId = pa.Id
		INNER JOIN Program prog ON pa.ProgramId = prog.Id
		INNER JOIN Organization o ON p.OrganizationId = o.Id
		INNER JOIN [UserView] uv ON p.CreatedById = uv.Id
		OUTER APPLY (SELECT TOP 1 * FROM ProjectFavorite WHERE p.Id = ProjectId AND UserId = @UserId) AS pf
		OUTER APPLY (SELECT TOP 1 * FROM ProjectMemberView WHERE p.Id = ProjectId AND UserId = @UserId) AS pmv
		OUTER APPLY (SELECT TOP 1 * FROM ProjectVersion WHERE ProjectId = p.Id) AS pv
		WHERE p.Active = @ActiveState
	END
END
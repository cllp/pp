CREATE PROCEDURE [dbo].[GetProject]
	@ProjectId int = 0
AS
BEGIN
	SELECT 
	p.*, 
	pv.PublishedDate,
	o.*, 
	uv.*,
	prog.*
	FROM Project p 
	INNER JOIN Organization o ON p.OrganizationId = o.Id
	INNER JOIN [UserView] uv ON p.CreatedById = uv.Id
	INNER JOIN ProjectArea pa ON p.ProjectAreaId = pa.Id
	INNER JOIN Program prog ON pa.ProgramId = prog.Id
	OUTER APPLY (SELECT TOP 1 * FROM ProjectVersion WHERE ProjectId = p.Id ORDER BY Id DESC) AS pv
	WHERE p.Id = @ProjectId
	
	SELECT itm.* FROM ProjectActivity itm WHERE itm.ProjectId = @ProjectId

	SELECT itm.* FROM ProjectFollowUp itm WHERE itm.ProjectId = @ProjectId

	SELECT itm.* FROM ProjectGoal itm WHERE itm.ProjectId = @ProjectId

	SELECT itm.* FROM ProjectMemberView itm WHERE itm.ProjectId = @ProjectId

	SELECT itm.* FROM ProjectRoleView itm WHERE itm.ProjectId = @ProjectId

	SELECT itm.* FROM ProjectRisk itm WHERE itm.ProjectId = @ProjectId

END

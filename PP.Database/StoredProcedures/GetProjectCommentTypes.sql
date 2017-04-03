CREATE PROCEDURE [dbo].[GetProjectCommentTypes]
	@ProjectId int,
	@UserId int
AS

	SELECT DISTINCT
	--p.Id as ProjectId,
	--pm.UserId,
	--u.Name as UserName,
	--pr.Id as RoleId,
	--pr.Name as RoleName,
	--pct.Id as ProjectCommentTypeId,
	--pct.Name as ProjectCommentTypeName,
	--prg.Id as ProjectRoleGroupId,
	--prg.Name as ProjectRoleGroupName
	pct.*
	FROM ProjectMember pm
	INNER JOIN Project p ON p.Id = pm.ProjectId
	INNER JOIN ProjectMemberRole pmr ON pm.Id = pmr.ProjectMemberId
	INNER JOIN ProjectRole pr ON pr.Id = pmr.ProjectRoleId
	INNER JOIN ProjectRoleGroup prg ON pr.ProjectRoleGroupId = prg.Id
	INNER JOIN ProjectCommentTypePermission pctp ON prg.Id = pctp.ProjectRoleGroupId
	INNER JOIN ProjectCommentType pct ON pctp.ProjectCommentTypeId = pct.Id
	INNER JOIN [User] u ON pm.UserId = u.Id
	WHERE p.Id = @ProjectId AND u.Id = @UserId
	--ORDER BY p.Id, u.Id

	
  
GO
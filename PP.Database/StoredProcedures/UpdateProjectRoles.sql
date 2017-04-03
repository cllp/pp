CREATE PROCEDURE [dbo].[UpdateProjectRoles]
	 @CurrentUserId int, 
	 @MemberUserId int, 
	 @MemberId int, 
	 @ProjectId int, 
	 @RoleIdList nvarchar(50)
AS

--SET @CurrentUserId = 8024
--SET @MemberUserId = 8024
--SET @ProjectId = 3305
--SET @RoleIdList = '3,7,6'

CREATE TABLE #AvailableRoles (Id uniqueidentifier default newid(), RoleId int)
CREATE TABLE #MemberPreviousRoles (Id uniqueidentifier default newid(), RoleId int)
CREATE TABLE #RequestedRoles (Id uniqueidentifier default newid(), RoleId int)
CREATE TABLE #ToBeAdded (Id uniqueidentifier default newid(), RoleId int)
CREATE TABLE #ToBeDeleted (Id uniqueidentifier default newid(), RoleId int)
CREATE TABLE #Denied (Id uniqueidentifier default newid(), RoleId int)


--Systemadmin skall kunna lägga till alla typer av medlemmar (RoleId = 3)
--Om projektet ligger i samma län och användaren är länsadmin skall alla roller listas
--länsadmin skall kunna lägga till alla typer av medlemmar i sitt län (RoleId = 2) 
--Om projektet ligger i samma kommun och användaren är kommunadmin skall alla roller listas
DECLARE @IsAdmin bit = (SELECT [dbo].[IsAdmin](@ProjectId, @CurrentUserId))

IF @IsAdmin = 1
BEGIN
	INSERT INTO #AvailableRoles (RoleId)
	Select Id FROM ProjectRole
END
ELSE
BEGIN	
	INSERT INTO #AvailableRoles (RoleId)
	SELECT pra.RoleId
	FROM ProjectMember pm
	INNER JOIN ProjectMemberRole pmr ON pmr.ProjectMemberId = pm.Id
	INNER JOIN ProjectRole pr ON pmr.ProjectRoleId = pr.Id
	INNER JOIN ProjectRoleGroup prg ON pr.ProjectRoleGroupId = prg.Id
	INNER JOIN ProjectRoleAdministration pra ON pra.RoleGroupId = prg.Id
	WHERE pm.UserId = @CurrentUserId AND pm.ProjectId = @ProjectId
END

INSERT INTO #MemberPreviousRoles (RoleId)
	select pr.Id from ProjectRole pr
	inner join ProjectMemberRole pmr on pmr.ProjectRoleId = pr.Id
	inner join ProjectMember pm on pm.Id = pmr.ProjectMemberId
	where pm.UserId = @MemberUserId and pm.ProjectId = @ProjectId

INSERT INTO #RequestedRoles (RoleId)
	SELECT Data FROM StringToIntTable(@RoleIdList)

--ta bort
INSERT INTO #ToBeDeleted (RoleId)
SELECT RoleId FROM #MemberPreviousRoles --alla föregående
WHERE RoleId IN (SELECT RoleId FROM #AvailableRoles) --som jag har rättigheter till
AND RoleId NOT IN (SELECT RoleId FROM #RequestedRoles) --och som inte finns i requested

--läggas till
INSERT INTO #ToBeAdded (RoleId)
SELECT RoleId FROM #RequestedRoles --lägg till alla efterfrågade roller
WHERE RoleId NOT IN (Select ROleId FROM #MemberPreviousRoles) --som inte redan finns
AND RoleId IN (SELECT RoleId FROM #AvailableRoles ) --och som jag har rättigheter till

--ej rättighet
INSERT INTO #Denied (RoleId)
SELECT RoleId FROM #RequestedRoles --lägg till alla efterfrågade roller
WHERE RoleId NOT IN (Select RoleId FROM #AvailableRoles) --som jag inte har rättigheter till

--inserting all roles
INSERT INTO ProjectMemberRole (ProjectMemberId, ProjectRoleId)
(SELECT @MemberId, RoleId FROM #ToBeAdded)

--removing all roles
DELETE FROM ProjectMemberRole 
WHERE ProjectMemberId = @MemberId 
AND ProjectRoleID IN (
	SELECT RoleId FROM #ToBeDeleted
)

--SELECT * FROM #AvailableRoles
--SELECT * FROM #MemberPreviousRoles
--SELECT * FROM #RequestedRoles
--SELECT * FROM #ToBeDeleted
--SELECT * FROM #Denied

select * from 
(
	SELECT pr.*, 'Deleted' AS [Status] FROM ProjectRole pr
	WHERE pr.Id IN (SELECT RoleId FROM #ToBeDeleted)
	UNION ALL
	SELECT pr.*, 'Created' [Status] FROM ProjectRole pr
	WHERE pr.Id IN (SELECT RoleId FROM #ToBeAdded)
	UNION ALL
	SELECT pr.*, 'Denied' [Status] FROM ProjectRole pr
	WHERE pr.Id IN (SELECT RoleId FROM #Denied)
) RawResults

DROP TABLE #ToBeDeleted
DROP TABLE #ToBeAdded
DROP TABLE #AvailableRoles
DROP TABLE #MemberPreviousRoles
DROP TABLE #RequestedRoles
DROP TABLE #Denied

RETURN 0

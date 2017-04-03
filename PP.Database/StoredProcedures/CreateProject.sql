CREATE PROCEDURE [dbo].[CreateProject]
	@OrganizationId int, 
	@Name nvarchar (255), 
	@Description nvarchar (max), 
	@StartDate DateTime2, 
	@CreatedById int,  
	@ProjectAreaId int,
	@ProgramOwnerId int,
	@ProjectCoordinatorId int
AS

DECLARE @ProjectId int, @ProjectMemberId int, @ProjectRoleId int

IF NOT EXISTS (SELECT Id FROM Project where Name like @Name)
BEGIN
	 INSERT INTO Project (
		[OrganizationId],
		[Name],
		[TypeId],
		[Description],
		[StartDate],
		[CreatedById], 
		[LastUpdate], 
		[CreatedDate], 
		[Active], 
		[ProjectAreaId], 
		[PhaseId], 
		[InternalCostPerHour], 
		[ExternalCostPerHour])
		VALUES (
			@OrganizationId, 
			@Name,
			1,
			@Description, 
			@StartDate, 
			@CreatedById, 
			GETUTCDATE(), 
			GETUTCDATE(), 
			1,
			@ProjectAreaId, 
			1, 
			0, 
			0);

	SET @ProjectId = SCOPE_IDENTITY()

	INSERT INTO ProjectMember (ProjectId, UserId)
	VALUES (@ProjectId, @CreatedById)
	SET @ProjectMemberId = SCOPE_IDENTITY()

	SET @ProjectRoleId = (SELECT [ProjectRole].Id  FROM [ProjectRole] WHERE Name = 'Projektägare' OR Name = 'Projectowner')

	INSERT INTO ProjectMemberRole (ProjectMemberId, ProjectRoleId)
	VALUES (@ProjectMemberId, @ProjectRoleId) --set to projectowner
	
	-- Programowner
	SET @ProjectMemberId = NULL
	SELECT @ProjectMemberId = Id FROM ProjectMember WHERE UserId = @ProgramOwnerId AND ProjectId = @ProjectId
	IF (@ProgramOwnerId > 0)
		BEGIN
			IF(@ProjectMemberId IS NULL)
			BEGIN
				INSERT INTO ProjectMember (ProjectId, UserId)
				VALUES (@ProjectId, @ProgramOwnerId)
				SET @ProjectMemberId = SCOPE_IDENTITY()
			END
			SET @ProjectRoleId = (SELECT [ProjectRole].Id  FROM [ProjectRole] WHERE Name = 'Programägare' OR Name = 'Programowner')

			INSERT INTO ProjectMemberRole (ProjectMemberId, ProjectRoleId)
			VALUES (@ProjectMemberId, @ProjectRoleId) --set to programtowner
		END
	
	-- Projectcoordinator
	SET @ProjectMemberId = NULL
	SELECT @ProjectMemberId = Id FROM ProjectMember WHERE UserId = @ProjectCoordinatorId AND ProjectId = @ProjectId
	IF (@ProjectCoordinatorId > 0)
		BEGIN
			IF(@ProjectMemberId IS NULL)
			BEGIN
				INSERT INTO ProjectMember (ProjectId, UserId)
				VALUES (@ProjectId, @ProjectCoordinatorId)
				SET @ProjectMemberId = SCOPE_IDENTITY()
			END
			SET @ProjectRoleId = (SELECT [ProjectRole].Id  FROM [ProjectRole] WHERE Name = 'Projektsamordnare' OR Name = 'Projectcoordinator')

			INSERT INTO ProjectMemberRole (ProjectMemberId, ProjectRoleId)
			VALUES (@ProjectMemberId, @ProjectRoleId) --set to projectcoordinator
		END
END
ELSE
BEGIN
SET @ProjectId = -1
END

SELECT @ProjectId

RETURN @ProjectId

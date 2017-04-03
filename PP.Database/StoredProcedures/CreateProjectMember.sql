CREATE PROCEDURE [dbo].[CreateProjectMember]
	@ProjectId int,
	@Name nvarchar (255),
	@Email nvarchar (255),
	@ProjectRoles nvarchar (255)--,
	--@MemberExists bit OUTPUT,
	--@UserExists bit OUTPUT	
AS

	DECLARE @ProjectMemberId int, @ProjectRoleId int, @ProjectMemberRoleId int,@UserId int, @MemberExists bit, @UserExists bit

	--BEGIN TRANSACTION
		 
	SET @UserId = (SELECT Id FROM [User] WHERE Email = @Email)

	IF @UserId IS NULL
	BEGIN
		--EXEC CreateUser @Name, @Email
		INSERT INTO [User] (Name, Email, RoleId)
		VALUES (@Name, @Email, 0) --default role
		SET @UserId = @@IDENTITY
		PRINT 'new userid: ' + CAST(@UserId as nvarchar(5))
		--SET @UserId = (SELECT Id from [User] WHERE Email = @Email)
		SET @UserExists = 0
	END
	ELSE
	BEGIN
		PRINT 'current userid: ' + CAST(@UserId as nvarchar(5))
		SET @UserExists = 1
	END

	IF NOT EXISTS (SELECT * FROM ProjectMember WHERE [ProjectId] = @ProjectId AND [UserId] = @UserId)
	BEGIN
		SET @MemberExists = 0
		INSERT INTO ProjectMember ([ProjectId], [UserId])
		VALUES (@ProjectId, @UserId);
		SET @ProjectMemberId = @@IDENTITY

		INSERT INTO ProjectMemberRole (ProjectMemberId,ProjectRoleId) --insert all new roles
		SELECT @ProjectMemberId as ProjectMemberId, data as ProjectRoleId 
		FROM dbo.StringToIntTable(@ProjectRoles)
	END
	ELSE
	BEGIN
		SET @MemberExists = 1
	END

	SELECT pmv.*, u.* FROM ProjectMemberView pmv
	INNER JOIN [User] u ON pmv.UserId = u.Id
	WHERE pmv.ProjectId = @ProjectId AND pmv.UserId = @UserId

	SELECT @MemberExists as MemberExists
	SELECT @UserExists as UserExists
	--COMMIT

GO
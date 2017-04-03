CREATE PROCEDURE [dbo].[DeleteProjectMember]
	@ProjectMemberId int = 0
AS

	DECLARE @UserId int

	SET @UserId = 
	(
		SELECT TOP 1 UserId 
		FROM ProjectMember
		WHERE Id = @ProjectMemberId 
	)

	BEGIN TRANSACTION
	
	DELETE FROM ProjectMemberRole WHERE ProjectMemberId = @ProjectMemberId
	DELETE FROM ProjectMember WHERE Id = @ProjectMemberId

	IF (SELECT Email FROM [User] WHERE Id = @UserId) IS NULL
	BEGIN
		DELETE FROM [User] WHERE Id = @UserId
	END

	COMMIT

RETURN 0
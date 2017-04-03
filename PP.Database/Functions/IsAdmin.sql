CREATE FUNCTION [dbo].[IsAdmin]
(
	@ProjectId int,
	@UserId int
)
RETURNS bit
AS
BEGIN

DECLARE @RoleId int = (SELECT RoleId FROM [User] WHERE Id = @UserId)
DECLARE @ProjectOrganizationId int = (SELECT OrganizationId FROM Project WHERE Id = @ProjectId)
DECLARE @Email nvarchar(100) = (SELECT Email FROM [User] WHERE Id = @UserId)
DECLARE @EmailDomain nvarchar(50) = SUBSTRING(@Email, CHARINDEX('@', @Email) + 1, len(@Email))
DECLARE @UserOrganizationId int = ISNULL((SELECT Id FROM Organization WHERE Domain = @EmailDomain), -1)
DECLARE @ReturnValue bit

IF(@RoleId = 3) --Superusers is superuser 
	BEGIN
		SET @ReturnValue = 1
	END
--if user is external or user of the current project is not of the same organization
ELSE IF (@UserOrganizationId < 1) 
BEGIN
	--PRINT 'User is external'
	SET @ReturnValue = 0
END
ELSE IF (@UserOrganizationId <> @ProjectOrganizationId)
BEGIN 
	--PRINT 'User organization does not match with project organization'
	SET @ReturnValue = 0
END
ELSE --user is internal and of the current project is the same organization
BEGIN
	IF(@RoleId = 1 OR @RoleId = 2) --Kommunadmin
	BEGIN
		SET @ReturnValue = 1
	END
	ELSE
	BEGIN
		SET @ReturnValue = 0
	END
END

	RETURN @ReturnValue
END
GO


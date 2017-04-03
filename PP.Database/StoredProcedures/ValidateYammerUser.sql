CREATE PROCEDURE [dbo].[ValidateYammerUser]
	@Email nvarchar(50),
	@Password nvarchar(25)
AS

	IF ((SELECT [dbo].[OrganizationState] (@Email)) <> 'External')
	BEGIN
		--SELECT 'EXTERNAL'
		IF NOT EXISTS (SELECT Id FROM [User] WHERE Email = @Email)
			BEGIN	
				--user does not exist and the user email domian is internal
				INSERT INTO [User] (Email, RoleId)
				VALUES (@Email, 1) --default role
			END
		--ELSE
		--	BEGIN
		--		--user exists, make sure to update the password
		--		--at the other hand do not update the password
		--		--UPDATE [User] SET [Password] = @Password WHERE Email = @Email 
		--	END
	END
	
	--get the user
	SELECT * FROM [UserView] WHERE Email = @Email --AND [Password] = @Password

GO
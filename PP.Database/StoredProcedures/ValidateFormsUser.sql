CREATE PROCEDURE [dbo].[ValidateFormsUser]
	@Email nvarchar(50),
	@Password nvarchar(25)

AS
	IF EXISTS (SELECT * FROM [User] WHERE Email = @Email AND [Password] = (SELECT dbo.GetPasswordHash(@Password)))
	BEGIN
		SELECT * FROM [UserView] WHERE Email = @Email
	END

GO



CREATE PROCEDURE [dbo].[ChangePasswordRequest]
	@Email nvarchar(60)
AS
	UPDATE [User] SET ChangePasswordRequest = NEWID(), ChangePasswordDate = GETUTCDATE() WHERE Email = @Email
GO

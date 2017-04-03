CREATE PROCEDURE [dbo].[ChangePassword]
	@Name nvarchar(50),
	@Password nvarchar(50),
	@ChangePasswordRequest nvarchar(50)
AS

	DECLARE @requestdate datetime2 = (SELECT ChangePasswordDate FROM [User] WHERE Id = 1)
	DECLARE @expired bit
	--SELECT @requestdate as requestdate

	SET @expired = (SELECT 
		case 
			when DATEDIFF(DAY, @requestdate, GETUTCDATE()) > 7 --Ändrat från 20 min till en vecka.
			then 1
			else 0
		end
		)
	--SELECT @expired as expired

	IF @expired = 1
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		DECLARE @hash varbinary(32)
		SET @hash = (SELECT dbo.GetPasswordHash(@Password))

		IF(@Name IS NULL)
		BEGIN
			UPDATE [User] SET [Password] = @hash, ChangePasswordRequest = null, ChangePasswordDate = null WHERE ChangePasswordRequest = @ChangePasswordRequest
		END
		ELSE
		BEGIN
			UPDATE [User] SET [Password] = @hash, ChangePasswordRequest = null, ChangePasswordDate = null, Name = @Name WHERE ChangePasswordRequest = @ChangePasswordRequest
		END
		RETURN 1
	END
GO
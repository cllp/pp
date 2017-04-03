CREATE PROCEDURE [dbo].[CreateUser]
	@Name nvarchar(50),
	@Email nvarchar(50)
AS
	
	IF NOT EXISTS (SELECT Id FROM [User] WHERE Email = @Email)
	BEGIN
		--creating a hashed password
		DECLARE @hash varbinary(32)
		DECLARE @rand nvarchar(16) = (select dbo.fn_random())
		SET @hash = (SELECT dbo.GetPasswordHash(@rand))

		INSERT INTO [User] (Name, Email, [Password], RoleId)
		VALUES (@Name, @Email, @hash, 0) --default role
	END

	SELECT @rand
	
	--SELECT * FROM [User] WHERE Email = @Email
	
GO
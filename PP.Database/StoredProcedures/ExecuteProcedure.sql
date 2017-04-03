CREATE PROCEDURE [dbo].[ExecuteProcedure]
	@Key nvarchar (50),
	@UserId int
AS
	DECLARE @Proc nvarchar(100)
	SET @Proc = (SELECT TOP 1 [Procedure] FROM Report WHERE [Key] = @Key) + ' ' + CAST(@UserId as varchar(5))
	exec (@proc) 

RETURN 0

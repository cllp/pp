CREATE PROCEDURE [dbo].[CreateProjectComment]
	@ProjectId int,
	@UserId int,
	@ProjectCommentAreaId int,
	@ProjectCommentTypeId int,
	@Text nvarchar (2000)
AS
	DECLARE @id int = 0
	INSERT INTO ProjectComment (ProjectId, ProjectCommentAreaId, ProjectCommentTypeId, UserId, [Text]) --insert all new roles
	VALUES (@ProjectId, @ProjectCommentAreaId, @ProjectCommentTypeId, @UserId, @Text)
	SET @id = @@IDENTITY

	--select the inserted projectcomment
	SELECT * FROM ProjectCommentView WHERE Id = @id;

	PRINT @id
	RETURN @id
GO
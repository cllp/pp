CREATE PROCEDURE [dbo].[UpdateProjectCommentActivity]
	@ProjectId int,
	@UserId int,
	@ProjectCommentAreaId int,
	@ProjectCommentTypeId int
AS

	DECLARE @ProjectCommentActivityId int = 0
	DECLARE @CurrentProjectCommentCount int = 0

	--get the current comment count
	SET @CurrentProjectCommentCount = (
		SELECT Count(Id) 
		FROM [ProjectComment] 
		WHERE ProjectId = @ProjectId 
		AND UserId <> @UserId--TODO: Maybe count only the comments that is not my own eg. AND UserId <> @UserId
		AND ProjectCommentAreaId = @ProjectCommentAreaId
		AND ProjectCommentTypeId = @ProjectCommentTypeId
	)

	SET @ProjectCommentActivityId = (
		SELECT Id 
		FROM [ProjectCommentActivity] 
		WHERE ProjectId = @ProjectId 
		AND UserId = @UserId 
		AND ProjectCommentAreaId = @ProjectCommentAreaId
		AND ProjectCommentTypeId = @ProjectCommentTypeId
	)

  IF @ProjectCommentActivityId > 0
	  BEGIN
		--UPDATE CURRENT ACTIVITY
		UPDATE [ProjectCommentActivity] SET LastReadCount = @CurrentProjectCommentCount, LastReadDate = GETUTCDATE()
		WHERE Id = @ProjectCommentActivityId
	  END
  ELSE
	  BEGIN
		----INSERT NEW ACTIVITY
		INSERT INTO [ProjectCommentActivity] (ProjectId, UserId, ProjectCommentAreaId, ProjectCommentTypeId, LastReadCount)
		VALUES (@ProjectId, @UserId, @ProjectCommentAreaId, @ProjectCommentTypeId, @CurrentProjectCommentCount)
	  END

RETURN 0

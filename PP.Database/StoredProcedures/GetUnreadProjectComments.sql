CREATE PROCEDURE [dbo].[GetUnreadProjectComments]
	@ProjectId int,
	@UserId int
AS

SELECT DISTINCT
pc.ProjectCommentAreaId as AreaId,
pca.Name as Area,
pc.ProjectCommentTypeId as TypeId,
pct.Name as [Type],
--COUNT COMMENTS (exclude own comments)
(
	SELECT Count(Id)
	FROM ProjectComment
	WHERE ProjectId = @ProjectId AND UserId <> @UserId
	AND ProjectCommentAreaId = pc.ProjectCommentAreaId
	AND ProjectCommentTypeId = pc.ProjectCommentTypeId
) -
--COUNT LAST READ
(
	ISNULL((
	SELECT LastReadCount
	FROM ProjectCommentActivity
	WHERE ProjectId = @ProjectId AND UserId = @UserId
	AND ProjectCommentAreaId = pc.ProjectCommentAreaId
	AND ProjectCommentTypeId = pc.ProjectCommentTypeId
), 0)
) as UnRead
FROM ProjectComment pc
INNER JOIN ProjectCommentArea pca ON pca.Id = pc.ProjectCommentAreaId
INNER JOIN ProjectCommentType pct ON pct.Id = pc.ProjectCommentTypeId
WHERE ProjectId = @ProjectId 
AND UserId <> @UserId
GROUP BY pc.ProjectCommentAreaId, pc.ProjectCommentTypeId, pca.Name, pct.Name

 -- SELECT
 -- pca.Id as AreaId,
 -- pca.Name as Area,
 -- pct.Id as TypeId,
 -- pct.Name as [Type],
 -- --Count(pc.Id) as CommentCount,
 -- --ISNULL(act.LastReadCount, 0) as LastReadCount,
 -- Count(pc.Id) - ISNULL(act.LastReadCount, 0) as UnRead
 -- FROM ProjectCommentArea pca
 -- INNER JOIN ProjectComment pc ON pc.ProjectCommentAreaId = pca.Id-- AND pc.ProjectId = @ProjectId
 -- INNER JOIN ProjectCommentType pct ON pc.ProjectCommentTypeId = pct.Id
 -- LEFT JOIN ProjectCommentActivity act ON
	--act.ProjectId = @ProjectId 
	--AND act.UserId = @UserId 
	--AND act.ProjectCommentTypeId = pct.Id
	--AND act.ProjectCommentAreaId = pca.Id
 -- WHERE pc.ProjectId = @ProjectId AND pc.UserId = @UserId
 -- GROUP BY pca.Id, pct.Id, pca.Name, pct.Name, pc.ProjectId, act.LastReadCount
 -- HAVING Count(pc.Id) - ISNULL(act.LastReadCount, 0) > 0 --Only show the unread messages
 -- ORDER BY pca.Id, pct.Id
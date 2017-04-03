CREATE VIEW [dbo].[ProjectCommentView]
	AS 
	SELECT 
	pc.Id,
	pc.[Date],
	pc.ProjectId,
	pc.[Text],
	u.Id as UserId,
	pca.Id as AreaId,
	pca.Name as Area,
	pct.Id as TypeId,
	pct.Name as [Type],
	COALESCE(u.Name, u.Email) AS Writer
	FROM [ProjectComment] pc 
	INNER JOIN [User] u ON pc.UserId = u.Id
	INNER JOIN [ProjectCommentArea] pca ON pc.ProjectCommentAreaId = pca.Id
	INNER JOIN [ProjectCommentType] pct ON pc.ProjectCommentTypeId = pct.Id
GO



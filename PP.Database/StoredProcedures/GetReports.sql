CREATE PROCEDURE [dbo].[GetReports]
	@UserId int
AS

	SELECT DISTINCT
	r.*
	FROM [Report] r
	INNER JOIN ReportPermission rp ON r.Id = rp.ReportId
	INNER JOIN [UserView] uv ON uv.RoleId = rp.RoleId AND uv.Id = @UserId
  
GO
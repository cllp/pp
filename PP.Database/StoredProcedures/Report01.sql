CREATE PROCEDURE [dbo].[Report01]
	@UserId int = 0
AS

SELECT p.[Id]
      ,RTRIM(o.[Name]) AS Organization
      ,RTRIM(p.[Name]) AS 'Name'
      ,RTRIM(pa.[Name]) AS ProjectArea
      ,CAST(.5 * pc.IHrs *1000 AS INT) AS 'InternalCost'
      ,1 * pc.EHrs *1000 AS 'ExternalCost'
  ,pc.Oth *1000 AS 'OtherCost'
  ,CAST(.5 * pc.IHrs + 1 * pc.EHrs + pc.oth AS Int)*1000 AS TotalCost
      ,p.[FundingStimulus] *1000 AS 'Stimulus'
  ,RTRIM(po.[Owner]) AS 'Owner'
  ,RTRIM(pl.[Leader]) AS 'Leader'
  FROM [dbo].[Project] AS p 
  JOIN [dbo].[ProjectArea] AS pa ON p.ProjectAreaId = pa.Id
  JOIN [dbo].[Organization] AS o ON p.[OrganizationId] = o.Id
  LEFT OUTER JOIN (SELECT Max(u.Name) AS 'Owner'
  ,pm.ProjectId AS ProjectID
  FROM dbo.ProjectMember AS pm 
  JOIN dbo.ProjectMemberRole AS pmr ON pm.Id = pmr.ProjectMemberId
  INNER JOIN dbo.ProjectRole AS pr ON pmr.ProjectRoleId = pr.Id
  JOIN dbo.[User] AS u ON pm.UserId=u.Id
 WHERE pr.Id = 3
 GROUP BY ProjectId) AS po ON po.ProjectId = p.Id
  LEFT OUTER JOIN (SELECT Max(u.Name) AS 'Leader'
  ,pm.ProjectId AS ProjectID
  FROM dbo.ProjectMember AS pm 
  JOIN dbo.ProjectMemberRole AS pmr ON pm.Id = pmr.ProjectMemberId
  INNER JOIN dbo.ProjectRole AS pr ON pmr.ProjectRoleId = pr.Id
  JOIN dbo.[User] AS u ON pm.UserId=u.Id
 WHERE pr.Id = 4 AND u.ID NOT IN (SELECT u.Id
  FROM dbo.ProjectMember AS pm 
  JOIN dbo.ProjectMemberRole AS pmr ON pm.Id = pmr.ProjectMemberId
  INNER JOIN dbo.ProjectRole AS pr ON pmr.ProjectRoleId = pr.Id
  JOIN dbo.[User] AS u ON pm.UserId=u.Id
 WHERE pr.Id = 3)
 Group By ProjectId) AS pl ON pl.ProjectId = p.Id
  JOIN (SELECT ProjectId
  ,Sum([InternalHours]) AS IHrs
      ,SUM([ExternalHours]) AS EHrs
      ,SUM([Cost]) AS Oth
  FROM [dbo].[ProjectActivity]
  GROUP BY ProjectId) AS pc ON pc.ProjectId = p.Id
  WHERE p.Active=1
GO




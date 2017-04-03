CREATE TABLE [dbo].[ReportPermission]
(
	[ReportId] int NOT NULL Foreign key references [Report]([Id]),
	[RoleId] int NOT NULL,
	CONSTRAINT [REPORTPERMISSION_CONSTRAINT] UNIQUE CLUSTERED
	(
		[ReportId], [RoleId]
	)
)

CREATE TABLE [dbo].[ProgramRoleAdministration]
(
	[UserId] INT NOT NULL Foreign key references [User]([Id]), 
    [ProgramTypeId] INT NOT NULL, 
    [ProjectRoleId] INT NOT NULL, 
	[OrganizationId] INT NULL, 
		CONSTRAINT [PROGRAMROLEADMIN_CONSTRAINT] UNIQUE CLUSTERED
	(
		[UserId], [ProjectRoleId], [ProgramTypeId]
	)
)

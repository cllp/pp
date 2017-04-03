CREATE TABLE [dbo].[ProjectVersion]
(
	[Id] INT NOT NULL IDENTITY (1,1) PRIMARY KEY, 
    [ProjectId] INT NOT NULL Foreign key references [Project]([Id]), 
    [Comment] NVARCHAR(MAX) NULL, 
	[PhaseId] INT NOT NULL, 
    [ProjectData] XML NULL, 
    [PublishedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [PublishedBy] INT NOT NULL REFERENCES [User]([Id])
)

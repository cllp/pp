CREATE TABLE [dbo].[ProjectRisk]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [ProjectId] INT NOT NULL Foreign key references [Project]([Id]), 
    [Name] NVARCHAR(255) NULL,
    [ConsequenceId] INT NOT NULL, 
    [ProbabilityId] INT NOT NULL
)

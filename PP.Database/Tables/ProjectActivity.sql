CREATE TABLE [dbo].[ProjectActivity]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Status] INT NOT NULL, 
    [InternalHours] INT NOT NULL, 
    [ExternalHours] INT NOT NULL, 
    [Cost] INT NOT NULL, 
    [ProjectId] INT NOT NULL Foreign key references [Project]([Id]), 
    [Name] NVARCHAR(255) NULL, 
    [Notes] NVARCHAR(MAX) NULL,
	[Created] DateTime2 NULL DEFAULT GETUTCDATE()
)

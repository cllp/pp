CREATE TABLE [dbo].[ProjectGoal]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [ProjectId] INT NOT NULL Foreign key references [Project]([Id]), 
    [Type] INT NOT NULL, 
    [GoalDefinition] NVARCHAR(MAX) NULL, 
    [MesaureMethod] NVARCHAR(MAX) NULL, 
    [Achieved] INT NOT NULL 
)

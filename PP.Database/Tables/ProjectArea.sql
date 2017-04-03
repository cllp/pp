CREATE TABLE [dbo].[ProjectArea]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
	[ProgramId] INT NOT NULL Foreign key references [Program]([Id]),
	[Active] bit NOT NULL DEFAULT 1,
	[OrganizationId] INT NULL Foreign key references [Organization]([Id])
)

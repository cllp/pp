CREATE TABLE [dbo].[Program]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
	[TypeId] int NOT NULL DEFAULT 1, --Typbegreppet avgör om det är ett Nationellt projekt (Typ 1), Länsprojekt (Typ 2), Kommunprojekt (Typ 3) eller Verksamhetsprojekt (Typ 4). Jag kan inte se att andra Programtyper är intressanta på sikt. Behöver alltså inte kunna redigeras.
	[RequireProgramOwner] bit NOT NULL DEFAULT 0,
	[RequireProjectCoordinator] bit NOT NULL DEFAULT 0,
	[Active] bit NOT NULL DEFAULT 1,
	[Description] NVARCHAR(255) NOT NULL,
	[OrganizationId] INT NULL Foreign key references [Organization]([Id]), 
)

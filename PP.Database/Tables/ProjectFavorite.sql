CREATE TABLE [dbo].[ProjectFavorite]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL Foreign key references [User]([Id]),	 
    [ProjectId] INT NOT NULL Foreign key references [Project]([Id])
)

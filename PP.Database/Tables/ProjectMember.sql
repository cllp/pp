CREATE TABLE [dbo].[ProjectMember]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[ProjectId] int NOT NULL Foreign key references [Project]([Id]),
	[UserId] int NOT NULL Foreign key references [User]([Id])
)

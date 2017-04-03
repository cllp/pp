CREATE TABLE [dbo].[ProjectComment]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[ProjectId] INT NOT NULL Foreign key references [Project]([Id]), 
	[UserId] INT NOT NULL Foreign key references [User]([Id]), 
	[ProjectCommentAreaId] INT NOT NULL Foreign key references [ProjectCommentArea]([Id]), --område: aktivitet, mål, uppföljning, etc.
	[ProjectCommentTypeId] INT NOT NULL Foreign key references [ProjectCommentType]([Id]), --administrativ / projektrelaterad.
	[Date] DateTime NOT NULL DEFAULT GETUTCDATE(),
	[Text] nvarchar (2000)
)

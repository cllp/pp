CREATE TABLE [dbo].[ProjectCommentActivity]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[ProjectId] INT NOT NULL Foreign key references [Project]([Id]), 
	[UserId] INT NOT NULL Foreign key references [User]([Id]), 
	[ProjectCommentAreaId] INT NOT NULL Foreign key references [ProjectCommentArea]([Id]), --område: aktivitet, mål, uppföljning, etc.
	[ProjectCommentTypeId] INT NOT NULL Foreign key references [ProjectCommentType]([Id]), --administrativ / projektrelaterad.
	[LastReadCount] int NOT NULL DEFAULT 0,
	[LastReadDate] DateTime NOT NULL DEFAULT GETUTCDATE(),
	CONSTRAINT [ProjectCommentActivity_CONSTRAINT] UNIQUE
	(
		[ProjectId], [UserId], [ProjectCommentAreaId], [ProjectCommentTypeId]
	)

	/*
	När en användare loggar in så behöver vi läsa upp en count för varje projektkommentarsområde och matcha den med en current count i [ProjectCommentActivity].
	På det sättet få vi en indikation på hur många som tillkommit på varje område sedan kommentarstråden senast öppnades för respektive område och typ.

	När kommentarstråd visas måste [LastReadCount] och [LastReadDate] uppdateras med antal kommentarer som finns för resp ([ProjectId], [ProjectCommentAreaId], [ProjectCommentTypeId])
	*/
)

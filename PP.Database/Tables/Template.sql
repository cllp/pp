CREATE TABLE [dbo].[Template]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[Key] nvarchar (50), --projectmember, confirmationemail etc
	[Section] nvarchar (25), --email, pdf, helptext
	[Format] nvarchar (25), --html, txt
	[Data] nvarchar (max),
	CONSTRAINT [TEMPLATEKEY_CONSTRAINT] UNIQUE
	(
		[Key]
	)
)

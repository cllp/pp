CREATE TABLE [dbo].[ProjectFollowUp] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]     INT            NOT NULL,
    [Date]          DATETIME       NULL,
    [InternalHours] INT            NOT NULL,
    [ExternalHours] INT            NOT NULL,
    [OtherCosts]    INT            NOT NULL,
    [Notes]         NVARCHAR (MAX) NULL,
    [ActivityTotal] INT            NULL,
	[Canceled]		BIT			NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

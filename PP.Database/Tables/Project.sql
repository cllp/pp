CREATE TABLE [dbo].[Project]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [OrganizationId] INT NOT NULL Foreign key references [Organization]([Id]), 
    [Name] NVARCHAR(255) NULL, 
    [TypeId] INT NOT NULL DEFAULT 0, --enum External | Internal 
	[StartDate] DATETIME NULL DEFAULT GETUTCDATE(), 
    [CreatedById] int NOT NULL Foreign key references [User]([Id]),
    [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [LastUpdate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Active] BIT NOT NULL DEFAULT 0, 
    [ProjectAreaId] INT NOT NULL, 
    [InternalCostPerHour] DECIMAL(5, 2) NOT NULL DEFAULT 0, 
    [ExternalCostPerHour] DECIMAL(5, 2) NOT NULL DEFAULT 0, 
    [PhaseId] INT NOT NULL DEFAULT 0, 
    [Description] NVARCHAR(MAX) NULL, 
    [FundingEstimate] INT NOT NULL DEFAULT 0, 
    [FundingActual] INT NOT NULL DEFAULT 0, 
    [FundingStimulus] INT NOT NULL DEFAULT 0, 
    [FundingExternal] INT NOT NULL DEFAULT 0, 
    [PlanIntroductionBackground] NVARCHAR(MAX) NULL,
    [PlanIntroductionDefinition] NVARCHAR(MAX) NULL,
    [PlanIntroductionComments] NVARCHAR(MAX) NULL,
    [PlanDescriptionExtent] NVARCHAR(MAX) NULL,
    [PlanDescriptionDelimitation] NVARCHAR(MAX) NULL,
    [PlanDescriptionManagement] NVARCHAR(MAX) NULL,
    [PlanDescriptionEvaluation] NVARCHAR(MAX) NULL,
    [PlanImplementationDescription] NVARCHAR(MAX) NULL,
    [PlanCommunicationDefinition] NVARCHAR(MAX) NULL,
    [PlanCommunicationInterestAnalysis] NVARCHAR(MAX) NULL,
    [PlanCommunicationPlan] NVARCHAR(MAX) NULL,
    [Debriefing] NVARCHAR(MAX) NULL


)

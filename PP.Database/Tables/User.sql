CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [Name] NCHAR(100) NULL, 
	[Email] NCHAR(100) NULL,
	--[YammerId] bigint NULL,
	[Password] VARBINARY(32) null,
	[RoleId] INT NULL,
	[ChangePasswordRequest] uniqueidentifier null,
	[ChangePasswordDate] DateTime2 null,
    CONSTRAINT uc_Email UNIQUE (Email)
)
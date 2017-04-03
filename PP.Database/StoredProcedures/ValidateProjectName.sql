CREATE  PROCEDURE [dbo].[ValidateProjectName]
	@ProjectId int = 0,
	@ProjectName nvarchar (255)
AS
	
	DECLARE @Valid bit = 1

	DECLARE @ProjectIdFromName int = 0
	SET @ProjectIdFromName = ISNULL((SELECT TOP 1 Id FROM Project WHERE Name = @ProjectName), 0)

	DECLARE @OrganizationIdFromName int = 0
	SET @OrganizationIdFromName = ISNULL((SELECT TOP 1 OrganizationId FROM Project WHERE Name = @ProjectName), 0)

	DECLARE @ProjectIdFromId int = 0
	SET @ProjectIdFromId = ISNULL((SELECT TOP 1 Id FROM Project WHERE Id = @ProjectId), 0)

	DECLARE @OrganizationIdFromId int = 0
	SET @OrganizationIdFromId = ISNULL((SELECT TOP 1 OrganizationId FROM Project WHERE Id = @ProjectId), 0)

	IF @ProjectIdFromName > 0
	BEGIN
		IF @OrganizationIdFromName = @OrganizationIdFromId
		BEGIN
			IF @ProjectIdFromId <> @ProjectIdFromName
			BEGIN
				PRINT 'exists for same organization'
				SET @Valid = 0
			END
		END
	END

	SELECT @Valid as [valid]

RETURN 0
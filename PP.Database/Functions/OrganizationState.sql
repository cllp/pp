CREATE FUNCTION [dbo].[OrganizationState]
(
	@Email nvarchar(100)
)
RETURNS nvarchar (25)
AS
BEGIN

DECLARE @state nvarchar(25)

DECLARE @EmailDomain nvarchar(50)
SET @EmailDomain = SUBSTRING(@Email, CHARINDEX('@', @Email) + 1, len(@Email))

SET @state = 
	(
		SELECT CASE WHEN o.Domain IS NULL THEN 'External' ELSE 'Internal' END as [OrganizationState]
		FROM Organization o
		WHERE o.Domain = @EmailDomain
	)

	RETURN ISNULL(@state, 'External')
END
GO


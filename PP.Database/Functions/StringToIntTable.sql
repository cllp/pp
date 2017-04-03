CREATE FUNCTION [dbo].[StringToIntTable]
(
    @string VARCHAR(MAX)
)
RETURNS @output TABLE(
    data int
)
BEGIN

    DECLARE @start INT, @end INT
    SELECT @start = 1, @end = CHARINDEX(',', @string)

    WHILE @start < LEN(@string) + 1 BEGIN
        IF @end = 0 
            SET @end = LEN(@string) + 1

        INSERT INTO @output (data) 
        VALUES(CAST(SUBSTRING(@string, @start, @end - @start) AS int))
        SET @start = @end + 1
        SET @end = CHARINDEX(',', @string, @start)
    END

    RETURN

END
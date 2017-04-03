CREATE PROCEDURE [dbo].[SearchUsers]
	@search nvarchar(100),
	@ProjectId int
AS
	--take all space and convert to comma
	SET @search = REPLACE(@search,' ',',');
	--Create a temp table with the known columns
	create table #SearchTable (
		Id int IDENTITY(1,1) UNIQUE CLUSTERED not null,
		Search nvarchar(25)
	)

	INSERT INTO #SearchTable
		SELECT data FROM [dbo].[StringToStringTable](@search)

	--SELECT * FROM #SearchTable 

	--SELECT TOP 10 uv.* FROM [UserView] uv, #SearchTable st
	--WHERE ( 
	--	uv.Name LIKE '%' + st.Search + '%'
	--	OR uv.Email LIKE '%' + st.Search + '%'
	--	OR uv.Organization LIKE '%' + st.Search + '%'
	--	--OR uv.County LIKE '%' + st.Search + '%'
	--	--OR uv.Domain LIKE '%' + st.Search + '%'
	--)
	--AND uv.Id NOT IN (
	--	--select current projectmembers
	--	SELECT UserId FROM [ProjectMember] WHERE ProjectId = @ProjectId
	--)

	--DROP TABLE #SearchTable

	--NEW
	DECLARE @query AS NVARCHAR(500)
	SET @query = 'SELECT TOP 10 uv.* FROM [UserView] uv WHERE uv.Id NOT IN (SELECT UserId FROM [ProjectMember] WHERE ProjectId = ' + CAST(@ProjectId AS char(5)) + ')'

	DECLARE @Phrase nvarchar(25)

	DECLARE MY_CURSOR CURSOR 
	  LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
	SELECT DISTINCT Search 
	FROM #SearchTable

	OPEN MY_CURSOR
	FETCH NEXT FROM MY_CURSOR INTO @Phrase
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		SET @query = @query + ' AND (Name LIKE ''%' + @Phrase + '%'' OR Email LIKE ''%' + @Phrase + '%'' OR Organization LIKE ''%' + @Phrase + '%'')'
		FETCH NEXT FROM MY_CURSOR INTO @Phrase
	END
	CLOSE MY_CURSOR
	DEALLOCATE MY_CURSOR

	DROP TABLE #SearchTable

	EXECUTE(@query)

GO
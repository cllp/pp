CREATE function GetPasswordHash(@Password nvarchar(255))
RETURNS VARBINARY(32)
BEGIN
	RETURN CONVERT(VARBINARY(32), HASHBYTES('MD5', @Password + '$w0rdf1$h'));
END
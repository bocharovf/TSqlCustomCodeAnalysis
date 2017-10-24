/*
	Demonstrate custom CA for stored procedure
*/
CREATE PROCEDURE [dbo].[TestStoredProcedure]
AS
BEGIN

	-- restricted scalar functions:
	SELECT 
            CONCAT_WS('',''),
            TRANSLATE('', '', ''),
            TRIM(''),
            COMPRESS(''),
            CURRENT_TRANSACTION_ID(),
            DATEDIFF_BIG(S, GETDATE(), GETDATE()),
            DECOMPRESS(0x123),
            HOST_NAME(),
            ISJSON(''),
            JSON_MODIFY('', '', ''),
            JSON_QUERY('', ''),
            JSON_VALUE('', ''),
            SESSION_CONTEXT(''),
            STRING_ESCAPE('', '')
	
	-- restricted aggregate functions:
	SELECT STRING_AGG(DATA.DATA, '') 
	FROM
	(
		SELECT 'DATA' DATA
	) DATA

	-- restricted table valued functions:
	SELECT * FROM STRING_SPLIT('', '')
	SELECT * FROM OPENJSON('')

	RETURN 0

END

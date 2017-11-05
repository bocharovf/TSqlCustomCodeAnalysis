



select * from dbo.[GetCodeAnalysisRuleErrors]('', 
N'
CREATE PROC A
AS
BEGIN
SELECT TRIM(''TEST'')

SELECT TRIM(''TEST'');

SELECT TRIM(''TEST'');
END
')


select * from sys.objects 



select obj.[name], obj.type_desc, error.* 
from sys.sql_modules modules
join sys.objects obj on obj.object_id = modules.object_id
CROSS APPLY dbo.[GetCodeAnalysisRuleErrors]('', modules.definition) error
order by obj.object_id

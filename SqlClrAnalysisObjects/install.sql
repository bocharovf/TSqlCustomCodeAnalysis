sp_configure 'show advanced options', 1;  
GO  
RECONFIGURE;  
GO  
sp_configure 'clr enabled', 1;  
GO  
RECONFIGURE;  
GO  

ALTER DATABASE CURRENT SET TRUSTWORTHY ON
GO

CREATE ASSEMBLY [Microsoft.SqlServer.TransactSql.ScriptDom]
FROM 'C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\140\Microsoft.SqlServer.TransactSql.ScriptDom.dll'
WITH PERMISSION_SET = UNSAFE; 
GO
 
CREATE ASSEMBLY [SqlAnalysisCommon]
    AUTHORIZATION [dbo]
    FROM 'D:\dev\TsqlScriptChecker\SqlClrAnalysisObjects\bin\Release\SqlAnalysisCommon.dll'
	WITH PERMISSION_SET = UNSAFE; 

GO

-- DROP FUNCTION [dbo].[GetCodeAnalysisRuleErrors]
-- DROP ASSEMBLY [SqlClrAnalysisObjects]
CREATE ASSEMBLY [SqlClrAnalysisObjects]
    AUTHORIZATION [dbo]
    FROM 'D:\dev\TsqlScriptChecker\SqlClrAnalysisObjects\bin\Release\SqlClrAnalysisObjects.dll'
	WITH PERMISSION_SET = UNSAFE; 

GO

CREATE FUNCTION [dbo].[GetCodeAnalysisRuleErrors]
(@ruleName NVARCHAR (MAX) NULL, @sqlText NVARCHAR (MAX) NULL)
RETURNS 
     TABLE (
        [StartLine]   INT             NULL,
        [StartColumn] INT             NULL,
        [Code]        NVARCHAR (4000) NULL,
        [Description] NVARCHAR (4000) NULL)
AS
 EXTERNAL NAME [SqlClrAnalysisObjects].[UserDefinedFunctions].[GetCodeAnalysisRuleErrors]

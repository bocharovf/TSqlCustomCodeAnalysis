# About

The project demonstrate ability to extend default code analysis (CA) capabilities in VS database projects with custom CA rules. It also contains CLR user-defined function (UDF) to run code analysis directly from T-SQL.

# Content

The solution consists of projects:
* **SqlCodeAnalysisRules** - custom CA rule for VS database project. See "Run custom CA rule in Visual Studio".
* **SqlClrAnalysisObjects** - CLR UDF to run code analysis from T-SQL. See "Run custom CA rule from T-SQL".
* **SqlAnalysisCommon** - shared library with generic rule logic and resources.
* **SampleDBWithCA** - sample database project with CA enabled. Just to demonstrate custom CA rule.
* WebScraper - secondary project used to parse microsoft documentation and determine supported SQL Server versions for each function.

## Run custom CA rule in Visual Studio
1. Install SQL Server Data Tools
2. Update references to SDK libraries:

%Microsoft SQL Server Dir%\%version%\SDK\Assemblies\Microsoft.SqlServer.TransactSql.ScriptDom.dll

%VS Install Dir%\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\%version%\Microsoft.Data.Tools.Schema.Sql.dll

%VS Install Dir%\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\%version%\Microsoft.Data.Tools.Utilities.dll

%VS Install Dir%\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\%version%\Microsoft.SqlServer.Dac.dll

%VS Install Dir%\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\%version%\Microsoft.SqlServer.Dac.Extensions.dll
  
See https://msdn.microsoft.com/en-us/library/dn632175(v=vs.103).aspx for details.

3. Build solution
4. Copy both SqlAnalysisCommon.dll and SqlAnalysisCommon.dll to VS extension folder:
%Visual Studio Install Dir%\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\140\Extensions\
5. Restart Visual Studio
6. Run code analysis for SampleDBWithCA project.
7. Check custom CA rule warnings for TestStoredProcedure

## Run custom CA rule from T-SQL

1. Enable CLR in SQL Server:
```
sp_configure 'show advanced options', 1;  
GO  
RECONFIGURE;  
GO  
sp_configure 'clr enabled', 1;  
GO  
RECONFIGURE;  
GO 
```
2. Set database trustworthy:
```
ALTER DATABASE CURRENT SET TRUSTWORTHY ON;
```
3. Create required assemblies:
```
CREATE ASSEMBLY [Microsoft.SqlServer.TransactSql.ScriptDom]
FROM '... path to Microsoft.SqlServer.TransactSql.ScriptDom'
WITH PERMISSION_SET = UNSAFE;

CREATE ASSEMBLY [SqlAnalysisCommon]
FROM '... path to SqlAnalysisCommon.dll'
WITH PERMISSION_SET = UNSAFE;

CREATE ASSEMBLY [SqlClrAnalysisObjects]
FROM '... path to SqlClrAnalysisObjects.dll'
WITH PERMISSION_SET = UNSAFE;
```
4. Create UDF:
```
CREATE OR ALTER FUNCTION [dbo].[GetCodeAnalysisRuleErrors](@ruleName [nvarchar](max), @sqlText [nvarchar](max))
RETURNS  TABLE (
	[StartLine] [int] NULL,
	[StartColumn] [int] NULL,
	[Code] [nvarchar](4000) NULL,
	[Description] [nvarchar](4000) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlClrAnalysisObjects].[UserDefinedFunctions].[GetCodeAnalysisRuleErrors]
```
5. Run sample query to analyse all database objects:
```
SELECT 
	QUOTENAME(OBJECT_SCHEMA_NAME(modules.object_id)) + '.' +
	QUOTENAME(OBJECT_NAME(modules.object_id)) ObjectName, 
	errors.StartLine, 
	errors.StartColumn, 
	errors.Code, 
	errors.[Description]
FROM sys.sql_modules modules
CROSS APPLY dbo.GetCodeAnalysisRuleErrors('IncompatibleFunctionsSql2012Rule', modules.[definition]) errors 
```

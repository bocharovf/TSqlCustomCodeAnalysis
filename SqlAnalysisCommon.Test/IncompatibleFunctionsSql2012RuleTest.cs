using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlAnalysisCommon.Rules;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlAnalysisCommon.Test
{
    [TestClass]
    public class IncompatibleFunctionsSql2012RuleTest
    {
        [TestMethod]
        public void GetFragmentProblems_FragmentWithoutProblems_EmptyList()
        {
            var rule = new IncompatibleFunctionsSql2012Rule();
            var fragment = CreateTSqlFragment(
@"
DECLARE @VAR INT = 1
SELECT 
    @VAR ROW1_COL1

SELECT
        LTRIM(RTRIM('DATA')) ROW2_COL1"
            );

            var problems = rule.GetFragmentProblems(fragment).ToArray();

            Assert.AreEqual(0, problems.Count());
        }

        [TestMethod]
        public void GetFragmentProblems_AllFunctions_FindAllProblems()
        {
            var rule = new IncompatibleFunctionsSql2012Rule();
            var fragment = CreateTSqlFragment(
@"
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

SELECT STRING_AGG(DATA.DATA, '') 
FROM
(
	SELECT 'DATA' DATA
) DATA

SELECT * FROM STRING_SPLIT('', '')

SELECT * FROM OPENJSON('')
"
            );

            var problems = rule.GetFragmentProblems(fragment).ToArray();

            Assert.AreEqual(17, problems.Count());
        }

        #region Different sql objects

        [TestMethod]
        public void GetFragmentProblems_MultipleStatements_FindAllProblems()
        {
            var rule = new IncompatibleFunctionsSql2012Rule();
            var fragment = CreateTSqlFragment(
@"
SELECT 
    JSON_VALUE('{}', '$') ROW1_COL1

SELECT
        ISJSON('{}') ROW2_COL1"
            );

            var problems = rule.GetFragmentProblems(fragment).ToArray();

            Assert.AreEqual(2, problems.Count());

            Assert.AreEqual("IncompatibleFunctionUsage", problems[0].Code);
            Assert.AreEqual(3, problems[0].Line);
            Assert.AreEqual(5, problems[0].Column);
            Assert.AreEqual(
                "Avoid using function JSON_VALUE which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.", 
                problems[0].Description);

            Assert.AreEqual("IncompatibleFunctionUsage", problems[1].Code);
            Assert.AreEqual(6, problems[1].Line);
            Assert.AreEqual(9, problems[1].Column);
            Assert.AreEqual(
                "Avoid using function ISJSON which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.",
                problems[1].Description);
        }

        [TestMethod]
        public void GetFragmentProblems_MultipleBatches_FindAllProblems()
        {
            var rule = new IncompatibleFunctionsSql2012Rule();
            var fragment = CreateTSqlFragment(
@"
GO

SELECT 
    JSON_VALUE('{}', '$') ROW1_COL1

GO

SELECT
        ISJSON('{}') ROW2_COL1"
            );

            var problems = rule.GetFragmentProblems(fragment).ToArray();

            Assert.AreEqual(2, problems.Count());

            Assert.AreEqual("IncompatibleFunctionUsage", problems[0].Code);
            Assert.AreEqual(5, problems[0].Line);
            Assert.AreEqual(5, problems[0].Column);
            Assert.AreEqual(
                "Avoid using function JSON_VALUE which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.",
                problems[0].Description);

            Assert.AreEqual("IncompatibleFunctionUsage", problems[1].Code);
            Assert.AreEqual(10, problems[1].Line);
            Assert.AreEqual(9, problems[1].Column);
            Assert.AreEqual(
                "Avoid using function ISJSON which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.",
                problems[1].Description);
        }

        [TestMethod]
        public void GetFragmentProblems_StoredProcedure_FindAllProblems()
        {
            var rule = new IncompatibleFunctionsSql2012Rule();
            var fragment = CreateTSqlFragment(
@"
/*
    Comment
*/
CREATE OR ALTER PROCEDURE TestProc
(
    @Param INT
)
AS
BEGIN

    SELECT TRIM('DATA');

    UPDATE MyTable
    SET MyColumn = COMPRESS('DATA')

END
GO
"
            );

            var problems = rule.GetFragmentProblems(fragment).ToArray();

            Assert.AreEqual(2, problems.Count());

            Assert.AreEqual("IncompatibleFunctionUsage", problems[0].Code);
            Assert.AreEqual(12, problems[0].Line);
            Assert.AreEqual(12, problems[0].Column);
            Assert.AreEqual(
                "Avoid using function TRIM which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.",
                problems[0].Description);

            Assert.AreEqual("IncompatibleFunctionUsage", problems[1].Code);
            Assert.AreEqual(15, problems[1].Line);
            Assert.AreEqual(20, problems[1].Column);
            Assert.AreEqual(
                "Avoid using function COMPRESS which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.",
                problems[1].Description);
        }

        [TestMethod]
        public void GetFragmentProblems_View_FindAllProblems()
        {
            var rule = new IncompatibleFunctionsSql2012Rule();
            var fragment = CreateTSqlFragment(
@"
CREATE OR ALTER VIEW TestView
AS
    SELECT DECOMPRESS(0x1);
GO
"
            );

            var problems = rule.GetFragmentProblems(fragment).ToArray();

            Assert.AreEqual(1, problems.Count());

            Assert.AreEqual("IncompatibleFunctionUsage", problems[0].Code);
            Assert.AreEqual(4, problems[0].Line);
            Assert.AreEqual(12, problems[0].Column);
            Assert.AreEqual(
                "Avoid using function DECOMPRESS which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.",
                problems[0].Description);
        }

        [TestMethod]
        public void GetFragmentProblems_Function_FindAllProblems()
        {
            var rule = new IncompatibleFunctionsSql2012Rule();
            var fragment = CreateTSqlFragment(
@"
CREATE FUNCTION dbo.ufnGetInventoryStock(@ProductID int)  
RETURNS int   
AS   
-- Returns the stock level for the product.  
BEGIN  
    DECLARE @DATA VARCHAR(100)
    SET @DATA = TRIM('') 
    RETURN 1;  
END;  
GO 
"
            );

            var problems = rule.GetFragmentProblems(fragment).ToArray();

            Assert.AreEqual(1, problems.Count());

            Assert.AreEqual("IncompatibleFunctionUsage", problems[0].Code);
            Assert.AreEqual(8, problems[0].Line);
            Assert.AreEqual(17, problems[0].Column);
            Assert.AreEqual(
                "Avoid using function TRIM which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.",
                problems[0].Description);
        }

        [TestMethod]
        public void GetFragmentProblems_Trigger_FindAllProblems()
        {
            var rule = new IncompatibleFunctionsSql2012Rule();
            var fragment = CreateTSqlFragment(
@"
--Create an INSTEAD OF INSERT trigger on the view.
CREATE TRIGGER InsteadTrigger on InsteadView
INSTEAD OF INSERT
AS
BEGIN
  --Build an INSERT statement ignoring inserted.ID and 
  --inserted.ComputedCol.
  INSERT INTO BaseTable
       SELECT JSON_QUERY(CustomFields,'$.OtherLanguages')
       FROM inserted
END;
GO
"
            );

            var problems = rule.GetFragmentProblems(fragment).ToArray();

            Assert.AreEqual(1, problems.Count());

            Assert.AreEqual("IncompatibleFunctionUsage", problems[0].Code);
            Assert.AreEqual(10, problems[0].Line);
            Assert.AreEqual(15, problems[0].Column);
            Assert.AreEqual(
                "Avoid using function JSON_QUERY which is incompatible with SQL Server 2012 in stored procedures, functions and triggers.",
                problems[0].Description);
        }
        
        #endregion

        private TSqlFragment CreateTSqlFragment(string sqlText) {
            var parser = new TSql140Parser(false, SqlEngineType.Standalone);
            var reader = new StringReader(sqlText);
            IList<ParseError> errors = new List<ParseError>();
            return parser.Parse(reader, out errors);
        }
    }
}

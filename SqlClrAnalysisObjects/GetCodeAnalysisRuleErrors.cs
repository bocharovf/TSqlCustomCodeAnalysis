using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlAnalysisCommon.Common;
using SqlAnalysisCommon.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Gets all violations of specified rule in SQL fragment.
    /// </summary>
    /// <param name="ruleName">Name of the rule to check.</param>
    /// <param name="sqlText">SQL fragmet to process.</param>
    /// <returns>Table-valued result describing rule violation.</returns>
    [SqlFunction(
       DataAccess = DataAccessKind.Read,
       FillRowMethodName = "GetCodeAnalysisRuleErrors_FillRow",
       TableDefinition = "StartLine int, StartColumn int, Code nvarchar(4000), [Description] nvarchar(4000)")]
    public static IEnumerable GetCodeAnalysisRuleErrors(SqlString ruleName, SqlString sqlText)
    {
        var rule = GetRuleByName(ruleName.Value);
        var resultCollection = new List<ISqlCodeAnalysisProblem>();
        var parser = new TSql140Parser(false, SqlEngineType.All);
        var reader = new StringReader(sqlText.Value);
        IList<ParseError> errors = new List<ParseError>();
        TSqlFragment sqlFragment = parser.Parse(reader, out errors);
        
        IEnumerable<ISqlCodeAnalysisProblem> problems = rule.GetFragmentProblems(sqlFragment);
        if (problems != null)
        {
            resultCollection.AddRange(problems);
        }

        return resultCollection;
    }

    /// <summary>
    /// Parse code analysis problem object into table row.
    /// </summary>
    /// <param name="codeAnalysisProblem">Code analysis problem object.</param>
    /// <param name="startLine">Start line of the problem.</param>
    /// <param name="startColumn">Start column of the problem.</param>
    /// <param name="code">Problem unique code.</param>
    /// <param name="description">Probkem description.</param>
    public static void GetCodeAnalysisRuleErrors_FillRow(
         object codeAnalysisProblem,
         out SqlInt32 startLine,
         out SqlInt32 startColumn,
         out SqlString code,
         out SqlString description)
    {
        ISqlCodeAnalysisProblem problem = (ISqlCodeAnalysisProblem)codeAnalysisProblem;

        startLine = problem.Line;
        startColumn = problem.Column;
        code = problem.Code;
        description = problem.Description;
    }

    private static IGenericSqlAnalysisRule GetRuleByName(string ruleName) {
        Assembly rulesAssembly = Assembly.GetAssembly(typeof(IGenericSqlAnalysisRule));
        string fullTypeName = String.Format("SqlAnalysisCommon.Rules.{0}", ruleName);
        Type ruleType = rulesAssembly.GetType(fullTypeName);
        return Activator.CreateInstance(ruleType) as IGenericSqlAnalysisRule;
    }
}

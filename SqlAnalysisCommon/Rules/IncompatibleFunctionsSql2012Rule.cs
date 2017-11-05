using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlAnalysisCommon.Common;
using SqlAnalysisCommon.Problems;
using SqlAnalysisCommon.Visitors;
using System.Collections.Generic;
using System.Linq;

namespace SqlAnalysisCommon.Rules
{
    /// <summary>
    /// General rule that checks for usage of functions 
    /// incompatible with SQL Server 2012.
    /// </summary>>
    public class IncompatibleFunctionsSql2012Rule : IGenericSqlAnalysisRule
    {
        /// <summary>
        /// List of restricted functions which appears after SQL Server 2012.
        /// </summary>
        private readonly string[] IncompatibleFunctions = new string[] {
            "CONCAT_WS",
            "STRING_AGG",
            "TRANSLATE",
            "TRIM",
            "COMPRESS",
            "CURRENT_TRANSACTION_ID",
            "DATEDIFF_BIG",
            "DECOMPRESS",
            "HOST_NAME",
            "ISJSON",
            "JSON_MODIFY",
            "JSON_QUERY",
            "JSON_VALUE",
            "OPENJSON",
            "SESSION_CONTEXT",
            "STRING_ESCAPE",
            "STRING_SPLIT"
        };

        /// <summary>
        /// Gets list of problems caused by usage of functions incompatible with SQL Server 2012.
        /// </summary>
        /// <param name="fragment">SQL fragment to analyse.</param>
        /// <returns>List of problems.</returns>
        public IEnumerable<ISqlCodeAnalysisProblem> GetFragmentProblems(TSqlFragment fragment)
        {
            var restrictedFunctionCalls = new List<FunctionCallFragmentInfo>();
            var codeAnalysisProblems = new List<FunctionCallIncompatibleWithSql2012Problem>();

            var visitorTvp = new RestrictedGlobalFunctionTableReferenceVisitor(IncompatibleFunctions);
            var visitorFunction = new RestrctedFunctionCallVisitor(IncompatibleFunctions);
            var openJson = new OpenJsonTableReferenceVisitor();

            fragment.Accept(visitorTvp);
            fragment.Accept(visitorFunction);
            fragment.Accept(openJson);

            restrictedFunctionCalls.AddRange(visitorTvp.FragmentsFound);
            restrictedFunctionCalls.AddRange(visitorFunction.FragmentsFound);
            restrictedFunctionCalls.AddRange(openJson.FragmentsFound);

            return restrictedFunctionCalls
                .Select(fc => new FunctionCallIncompatibleWithSql2012Problem(fc.Fragment, fc.FunctionName));
        }
    }
}

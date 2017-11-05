using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace SqlAnalysisCommon.Common
{
    /// <summary>
    /// General SQL analysis rule interface.
    /// </summary>
    public interface IGenericSqlAnalysisRule
    {
        /// <summary>
        /// Gets collection of problems found in SQL fragment.
        /// </summary>
        /// <param name="sqlFragment">SQL fragment to analyse.</param>
        /// <returns>Collection of problems.</returns>
        IEnumerable<ISqlCodeAnalysisProblem> GetFragmentProblems(TSqlFragment sqlFragment);
    }
}

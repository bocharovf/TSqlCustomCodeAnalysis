using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlAnalysisCommon.Common
{
    /// <summary>
    /// General code analysis problem interface.
    /// </summary>
    public interface ISqlCodeAnalysisProblem
    {
        /// <summary>
        /// Fragment that caused problem. 
        /// </summary>
        TSqlFragment Fragment { get; }

        /// <summary>
        /// Problem unique code.
        /// </summary>
        string Code { get; }

        int Line { get; }
        int Column { get; }
        string Description { get; }
    }
}
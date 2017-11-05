using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlAnalysisCommon.Common
{
    /// <summary>
    /// Represents information of function call.
    /// </summary>
    public class FunctionCallFragmentInfo
    {
        /// <summary>
        /// Function call fragment.
        /// </summary>
        public TSqlFragment Fragment { get; set; }

        /// <summary>
        /// Name of called function.
        /// </summary>
        public string FunctionName { get; set; }
    }
}

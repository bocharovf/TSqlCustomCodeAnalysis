using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlAnalysisCommon.Common;
using System.Collections.Generic;

namespace SqlAnalysisCommon.Visitors
{
    /// <summary>
    /// Visitor to detect OPEN_JSON function call.
    /// </summary>
    public class OpenJsonTableReferenceVisitor : TSqlConcreteFragmentVisitor
    {
        public IList<FunctionCallFragmentInfo> FragmentsFound { get; private set; }

        public OpenJsonTableReferenceVisitor()
        {
            FragmentsFound = new List<FunctionCallFragmentInfo>();
        }

        public override void ExplicitVisit(OpenJsonTableReference node)
        {
            FragmentsFound.Add(new FunctionCallFragmentInfo()
            {
                Fragment = node,
                FunctionName = "OPEN_JSON"
            });
        }
    }
}

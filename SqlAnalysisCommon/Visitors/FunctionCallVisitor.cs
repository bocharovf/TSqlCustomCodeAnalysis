using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlAnalysisCommon.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlAnalysisCommon.Visitors
{
    /// <summary>
    /// Visitor to detect usage of restricted scalar functions.
    /// </summary>
    public class RestrctedFunctionCallVisitor : TSqlConcreteFragmentVisitor
    {
        public IList<FunctionCallFragmentInfo> FragmentsFound { get; private set; }
        public IEnumerable<string> RestrictedFunctionsList { get; private set; }

        public RestrctedFunctionCallVisitor(IEnumerable<string> restrictedFunctionsList)
        {
            FragmentsFound = new List<FunctionCallFragmentInfo>();
            RestrictedFunctionsList = restrictedFunctionsList;
        }

        public override void ExplicitVisit(FunctionCall node)
        {
            string functionName = node.FunctionName.Value;
            if (RestrictedFunctionsList.Contains(functionName, StringComparer.InvariantCultureIgnoreCase))
            {
                FragmentsFound.Add(new FunctionCallFragmentInfo() {
                    Fragment = node,
                    FunctionName = functionName
                });
            }
        }
    }
}

using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlAnalysisCommon.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlAnalysisCommon.Visitors
{
    /// <summary>
    /// Visitor to detect usage of restricted table valued functions.
    /// </summary>
    public class RestrictedGlobalFunctionTableReferenceVisitor : TSqlConcreteFragmentVisitor
    {
        public IList<FunctionCallFragmentInfo> FragmentsFound { get; private set; }
        public IEnumerable<string> RestrictedFunctionsList { get; private set; }

        public RestrictedGlobalFunctionTableReferenceVisitor(IEnumerable<string> restrictedFunctionsList)
        {
            FragmentsFound = new List<FunctionCallFragmentInfo>();
            RestrictedFunctionsList = restrictedFunctionsList;
        }

        public override void ExplicitVisit(GlobalFunctionTableReference node)
        {
            string functionName = node.Name.Value;
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

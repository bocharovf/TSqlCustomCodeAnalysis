using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCodeAnalysisRules
{
    internal class GlobalFunctionTableReferenceVisitor : TSqlConcreteFragmentVisitor
    {
        public IList<TSqlFragment> FragmentsFound { get; private set; }
        public IEnumerable<string> RestrictedFunctionsList { get; private set; }

        public GlobalFunctionTableReferenceVisitor(IEnumerable<string> restrictedFunctionsList)
        {
            FragmentsFound = new List<TSqlFragment>();
            RestrictedFunctionsList = restrictedFunctionsList;
        }

        public override void ExplicitVisit(GlobalFunctionTableReference node)
        {
            if (RestrictedFunctionsList.Contains(node.Name.Value, StringComparer.InvariantCultureIgnoreCase))
            {
                FragmentsFound.Add(node);
            }
        }
    }
}

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCodeAnalysisRules
{
    internal class OpenJsonTableReferenceVisitor : TSqlConcreteFragmentVisitor
    {
        public IList<TSqlFragment> FragmentsFound { get; private set; }

        public OpenJsonTableReferenceVisitor()
        {
            FragmentsFound = new List<TSqlFragment>();
        }

        public override void ExplicitVisit(OpenJsonTableReference node)
        {
            FragmentsFound.Add(node);
        }
    }
}

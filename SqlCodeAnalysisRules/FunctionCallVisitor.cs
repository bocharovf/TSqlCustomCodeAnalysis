﻿using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCodeAnalysisRules
{
    internal class FunctionCallVisitor : TSqlConcreteFragmentVisitor
    {
        public IList<TSqlFragment> FragmentsFound { get; private set; }
        public IEnumerable<string> RestrictedFunctionsList { get; private set; }

        public FunctionCallVisitor(IEnumerable<string> restrictedFunctionsList)
        {
            FragmentsFound = new List<TSqlFragment>();
            RestrictedFunctionsList = restrictedFunctionsList;
        }

        public override void ExplicitVisit(FunctionCall node)
        {
            if (RestrictedFunctionsList.Contains(node.FunctionName.Value, StringComparer.InvariantCultureIgnoreCase))
            {
                FragmentsFound.Add(node);
            }
        }
    }
}

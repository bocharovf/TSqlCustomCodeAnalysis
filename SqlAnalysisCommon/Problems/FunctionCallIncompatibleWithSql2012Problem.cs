using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlAnalysisCommon.Common;
using System;
using System.Globalization;
using System.Resources;

namespace SqlAnalysisCommon.Problems
{
    /// <summary>
    /// Represents problem caused by usage of function incompatible with SQL Server 2012.
    /// </summary>
    internal class FunctionCallIncompatibleWithSql2012Problem : ISqlCodeAnalysisProblem
    {
        private const string ProblemCode = "IncompatibleFunctionUsage";
        private const string ProblemDescriptionResourceId = "AvoidFunctionsIncompatibleWithSql2012_ProblemDescription";
        private ResourceManager ResourceManager;

        public TSqlFragment Fragment { get; set; }

        public string Code { get { return ProblemCode; } }

        /// <summary>
        /// Name of the incompatible function that caused problem.
        /// </summary>
        public string FunctionUsed { get; set; }

        public int Line
        {
            get
            {
                return Fragment.StartLine;
            }
        }

        public int Column
        {
            get
            {
                return Fragment.StartColumn;
            }
        }

        public string Description
        {
            get
            {
                var desciption = this.ResourceManager.GetString(ProblemDescriptionResourceId, CultureInfo.CurrentUICulture);
                return String.Format(desciption, FunctionUsed);
            }
        }

        public FunctionCallIncompatibleWithSql2012Problem(TSqlFragment fragment, string functionName)
        {
            this.Fragment = fragment;
            this.FunctionUsed = functionName;

            this.ResourceManager = new ResourceManager(CommonRuleConstants.ResourceBaseName, GetType().Assembly);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCodeAnalysisRules
{
    internal static class RuleConstants
    {
        /// <summary>  
        /// The name of the resources file to use when looking up rule resources  
        /// </summary>  
        public const string ResourceBaseName = "SqlCodeAnalysisRules.RuleResources";

        /// <summary>  
        /// The design category (should not be localized)  
        /// </summary>  
        public const string CategoryDesign = "Design";

        /// <summary>  
        /// The performance category (should not be localized)  
        /// </summary>  
        public const string CategoryPerformance = "Design";

        public const string AvoidFunctionsIncompatibleWithSql2012_RuleName = "AvoidFunctionsIncompatibleWithSql2012_RuleName";
        public const string AvoidFunctionsIncompatibleWithSql2012_ProblemDescription = "AvoidFunctionsIncompatibleWithSql2012_ProblemDescription";
    }
}

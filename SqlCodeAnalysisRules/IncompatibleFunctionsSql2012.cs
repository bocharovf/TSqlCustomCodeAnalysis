using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlAnalysisCommon;
using SqlAnalysisCommon.Common;
using SqlAnalysisCommon.Rules;
using System.Collections.Generic;
using System.Linq;

namespace SqlCodeAnalysisRules
{
    /// <summary>
    /// Code analysis rule that checks for usage of functions 
    /// incompatible with SQL Server 2012.
    /// </summary>
    [LocalizedExportCodeAnalysisRule(
        IncompatibleFunctionsSql2012.RuleId,
        CommonRuleConstants.ResourceBaseName,
        IncompatibleFunctionsSql2012.RuleName,
        IncompatibleFunctionsSql2012.ProblemDescription,    
        Category = CodeAnalysisRuleConstants.CategoryPerformance,
        RuleScope = SqlRuleScope.Element)]
    public sealed class IncompatibleFunctionsSql2012 : SqlCodeAnalysisRule
    {
        public const string RuleId = "SqlCodeAnalysisRules.SR1005";
        
        /// <summary>
        /// Resource id of the rule name.
        /// </summary>
        public const string RuleName = "AvoidFunctionsIncompatibleWithSql2012_RuleName";

        /// <summary>
        /// Resource id of the rule description.
        /// </summary>
        public const string ProblemDescription = "AvoidFunctionsIncompatibleWithSql2012_ProblemDescription";

        public IncompatibleFunctionsSql2012()
        {
            SupportedElementTypes = new[]
            {  
              ModelSchema.ExtendedProcedure,
              ModelSchema.Procedure,

              ModelSchema.TableValuedFunction,
              ModelSchema.ScalarFunction,
              ModelSchema.PartitionFunction,

              ModelSchema.View,
              ModelSchema.Rule,
              ModelSchema.Table,

              ModelSchema.DatabaseDdlTrigger,
              ModelSchema.DmlTrigger,
              ModelSchema.ServerDdlTrigger
           };
        }
 
        public override IList<SqlRuleProblem> Analyze(
            SqlRuleExecutionContext ruleExecutionContext)
        {
            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            TSqlFragment fragment = ruleExecutionContext.ScriptFragment;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;
            string elementName = GetElementName(ruleExecutionContext, modelElement);

            var rule = new IncompatibleFunctionsSql2012Rule();
            IEnumerable<ISqlCodeAnalysisProblem> problems = rule.GetFragmentProblems(fragment);
            return problems
                    .Select(problem => new SqlRuleProblem(
                        problem.Description,
                        modelElement, 
                        problem.Fragment
                    )).ToList();
        }

        private static string GetElementName(
            SqlRuleExecutionContext ruleExecutionContext,
            TSqlObject modelElement)
        {
            var displayServices = ruleExecutionContext.SchemaModel.DisplayServices;
            string elementName = displayServices.GetElementName(
                modelElement, ElementNameStyle.EscapedFullyQualifiedName);
            return elementName;
        }
    }
}

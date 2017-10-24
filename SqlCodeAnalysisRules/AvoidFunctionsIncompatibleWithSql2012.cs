using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCodeAnalysisRules
{
    [LocalizedExportCodeAnalysisRule(AvoidFunctionsIncompatibleWithSql2012.RuleId,
        RuleConstants.ResourceBaseName,
        RuleConstants.AvoidFunctionsIncompatibleWithSql2012_RuleName,
        RuleConstants.AvoidFunctionsIncompatibleWithSql2012_ProblemDescription,    
        Category = RuleConstants.CategoryPerformance,
        RuleScope = SqlRuleScope.Element)]
    public sealed class AvoidFunctionsIncompatibleWithSql2012 : SqlCodeAnalysisRule
    {
        public const string RuleId = "SqlCodeAnalysisRules.SR1005";
        private readonly string[] IncompatibleFunctions = new string[] {
            "CONCAT_WS",
            "STRING_AGG",
            "TRANSLATE",
            "TRIM",
            "COMPRESS",
            "CURRENT_TRANSACTION_ID",
            "DATEDIFF_BIG",
            "DECOMPRESS",
            "HOST_NAME",
            "ISJSON",
            "JSON_MODIFY",
            "JSON_QUERY",
            "JSON_VALUE",
            "OPENJSON",
            "SESSION_CONTEXT",
            "STRING_ESCAPE",
            "STRING_SPLIT"
        };

        public AvoidFunctionsIncompatibleWithSql2012()
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
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();
            List<TSqlFragment> restrictedFunctionCalls = new List<TSqlFragment>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;

            string elementName = GetElementName(ruleExecutionContext, modelElement);

            TSqlFragment fragment = ruleExecutionContext.ScriptFragment;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            var visitorTvp = new GlobalFunctionTableReferenceVisitor(IncompatibleFunctions);
            var visitorFunction = new FunctionCallVisitor(IncompatibleFunctions);
            var openJson = new OpenJsonTableReferenceVisitor();

            fragment.Accept(visitorTvp);
            fragment.Accept(visitorFunction);
            fragment.Accept(openJson);

            restrictedFunctionCalls.AddRange(visitorTvp.FragmentsFound);
            restrictedFunctionCalls.AddRange(visitorFunction.FragmentsFound);
            restrictedFunctionCalls.AddRange(openJson.FragmentsFound);

            foreach (TSqlFragment functionCall in restrictedFunctionCalls)
            {
                SqlRuleProblem problem = new SqlRuleProblem(
                    String.Format(CultureInfo.CurrentCulture,
                        ruleDescriptor.DisplayDescription, elementName),
                    modelElement,
                    functionCall);
                problems.Add(problem);
            }
            return problems;
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

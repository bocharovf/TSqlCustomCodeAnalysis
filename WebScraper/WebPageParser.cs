using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebScraper
{
    class WebPageParser
    {
        private static Regex RawSupportedTextExpression = new Regex("alt=\"yes\" data-linktype=\"relative-path\">([^<]*)<");
        private static Regex FunctionLinkExpression = new Regex(@"## \[(.*)\]\((.*)\.md\)");
        public IEnumerable<SqlObjectLink> GetFunctionLinks(string webPageContent, Uri baseUri)
        {
            return FunctionLinkExpression.Matches(webPageContent)
                                        .OfType<Match>()
                                        .AsParallel()
                                        .Select(match => new { name = match.Groups[1].Value, href = match.Groups[2].Value })
                                        .Distinct()
                                        .Where(matchData => Uri.IsWellFormedUriString(matchData.href, UriKind.RelativeOrAbsolute))
                                        .Select(matchData => new SqlObjectLink(matchData.name, new Uri(baseUri, matchData.href)))
                                        .ToList();
        }

        public IEnumerable<string> GetRawSupportedPlatforms(string webPageContent)
        {
            return RawSupportedTextExpression.Matches(webPageContent)
                                            .OfType<Match>()
                                            .Select(match => match.Groups[1].Value);
        }
    }
}

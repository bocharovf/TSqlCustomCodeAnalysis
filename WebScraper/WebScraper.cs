using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    class WebScraper
    {
        public async Task<IEnumerable<SqlObjectInfo>> ParseDocumentation(Uri tableOfContentsUri, Uri baseUri)
        {
            var loader = new WebPageLoader(tableOfContentsUri);
            var parser = new WebPageParser();

            string tableOfcontentsPageContent = await loader.GetPageContent(tableOfContentsUri);
            IEnumerable<SqlObjectLink> functionLinks = parser.GetFunctionLinks(tableOfcontentsPageContent, baseUri);

            var tasks = functionLinks.AsParallel()
                                .Select(link => ParseObjectLink(link, loader, parser))
                                .ToArray();
            Task.WaitAll(tasks);
            return tasks.Select(task => task.Result);
        }

        private async Task<SqlObjectInfo> ParseObjectLink(SqlObjectLink functionLink, WebPageLoader loader, WebPageParser parser) {
            string functionPageContent = String.Empty;
            try
            {
                functionPageContent = await loader.GetPageContent(functionLink.Href);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to load info for {functionLink.Name} from {functionLink.Href} because of {ex.Message}");
                return new SqlObjectInfo(functionLink.Href, functionLink.Name, null) {
                    Error = ex
                };
            }
            
            IEnumerable<string> rawPlatforms = parser.GetRawSupportedPlatforms(functionPageContent);
            return new SqlObjectInfo(functionLink.Href, functionLink.Name, rawPlatforms);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebScraper
{
    class Program
    {
        private readonly static Uri TableOfContentsUri = new Uri("https://raw.githubusercontent.com/MicrosoftDocs/sql-docs/live/docs/t-sql/functions/TOC.md");
        private readonly static Uri FunctionDetailsBaseUri = new Uri("https://docs.microsoft.com/en-us/sql/t-sql/functions/");
        static void Main(string[] args)
        {
            var webScraper = new WebScraper();
            var dataExporter = new CsvDataExporter(@"D:\export.csv");
            var objectInfo = webScraper.ParseDocumentation(TableOfContentsUri, FunctionDetailsBaseUri)
                                        .GetAwaiter()
                                        .GetResult();
            dataExporter.ExportData(objectInfo);
        }
    }
}

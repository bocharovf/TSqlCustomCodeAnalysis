using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    class ConsoleDataExporter : IDataExporter
    {
        public void ExportData(IEnumerable<SqlObjectInfo> sqlObjects)
        {
            foreach (var sqlObject in sqlObjects.OrderBy(so => so.Name))
            {
                string message = String.Empty;
                if (sqlObject.Error != null)
                {
                    message = $"Function {sqlObject.Name} parse error {sqlObject.Error.Message}";
                }
                else {
                    string supportString = sqlObject.SupportedPlatforms == null ? "-" : String.Join(", ", sqlObject.SupportedPlatforms);
                    message = $"Function {sqlObject.Name} supports {supportString}";
                }
                
                Console.WriteLine(message);
            }
        }
    }
}

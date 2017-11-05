using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    class CsvDataExporter : IDataExporter
    {
        private const string sep = ";";
        public string OutputFileName { get; private set; }
        public CsvDataExporter(string outputFileName)
        {
            this.OutputFileName = outputFileName;
        }
        public void ExportData(IEnumerable<SqlObjectInfo> sqlObjects)
        {
            using (StreamWriter writer = File.CreateText(OutputFileName))
            {
                var columns = new string [] {
                    "Function",
                    "SQL Server",
                    "Azure SQL Database",
                    "Azure SQL Data Warehouse",
                    "Parallel Data Warehouse"
                };
                var columnString = String.Join(sep, columns);
                writer.WriteLine(columnString);

                foreach (var sqlObject in sqlObjects.OrderBy(so => so.Name))
                {
                    var segments = new string[5];
                    segments[0] = sqlObject.Name;
                    segments[1] = GetColumnValueVersion(sqlObject, SupportedPlatform.SqlServer2008, SupportedPlatform.SqlServer2017);
                    segments[2] = GetColumnValueYesNo(sqlObject, SupportedPlatform.AzureSqlDatabase);
                    segments[3] = GetColumnValueYesNo(sqlObject, SupportedPlatform.AzureSqlDataWarehouse);
                    segments[4] = GetColumnValueYesNo(sqlObject, SupportedPlatform.ParallelDataWarehouse);                    
                    var line = String.Join(sep, segments);
                    writer.WriteLine(line);
                }
                writer.Flush();
            }
        }

        private string GetColumnValueYesNo(SqlObjectInfo sqlObjectInfo, SupportedPlatform platform)
        {
            var isPlatformSupported = sqlObjectInfo.SupportedPlatforms.Contains(platform);
            return isPlatformSupported ? "Yes" : "No";
        }

        private string GetColumnValueVersion(SqlObjectInfo sqlObjectInfo, SupportedPlatform min, SupportedPlatform max)
        {
            var minPlatform = sqlObjectInfo.SupportedPlatforms.FirstOrDefault(platform => platform >= min && platform <= max );
            return minPlatform == SupportedPlatform.Unknown ? "No" : minPlatform.ToString();
        }
    }
}

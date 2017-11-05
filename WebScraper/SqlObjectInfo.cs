using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    class SqlObjectInfo
    {
        public string Name { get; private set; }
        public Uri Href { get; private set; }
        public IEnumerable<SupportedPlatform> SupportedPlatforms { get; private set; }
        public Exception Error { get; set; }

        public SqlObjectInfo(Uri href, string name, IEnumerable<string> rawSupportedPlatforms)
        {
            this.Href = href;
            this.Name = name;
            this.SupportedPlatforms = rawSupportedPlatforms
                                        .Select(platformText => PlatformTextToEnum(platformText))
                                        .Distinct()
                                        .ToList();
        }

        private SupportedPlatform PlatformTextToEnum(string platformText)
        {
            if (platformText.IndexOf("SQL Server", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                if (platformText.Contains("2008"))
                    return SupportedPlatform.SqlServer2008;
                else if (platformText.Contains("2012"))
                    return SupportedPlatform.SqlServer2012;
                else if (platformText.Contains("2014"))
                    return SupportedPlatform.SqlServer2014;
                else if (platformText.Contains("2016"))
                    return SupportedPlatform.SqlServer2016;
                else if (platformText.Contains("2017"))
                    return SupportedPlatform.SqlServer2017;
                else return SupportedPlatform.Unknown;
            }
            else if (platformText.IndexOf("Azure SQL Database", StringComparison.CurrentCultureIgnoreCase) >= 0)
                return SupportedPlatform.AzureSqlDatabase;
            else if (platformText.IndexOf("Azure SQL Data Warehouse", StringComparison.CurrentCultureIgnoreCase) >= 0)
                return SupportedPlatform.AzureSqlDataWarehouse;
            else if (platformText.IndexOf("Parallel Data Warehouse", StringComparison.CurrentCultureIgnoreCase) >= 0)
                return SupportedPlatform.ParallelDataWarehouse;
            else return SupportedPlatform.Unknown;
        }

    }
}

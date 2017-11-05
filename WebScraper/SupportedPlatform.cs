using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    enum SupportedPlatform
    {
        Unknown = 0,

        SqlServer2008,
        SqlServer2012,
        SqlServer2014,
        SqlServer2016,
        SqlServer2017,

        AzureSqlDatabase,
        AzureSqlDataWarehouse,
        ParallelDataWarehouse,
    }
}

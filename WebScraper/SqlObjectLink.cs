using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    class SqlObjectLink
    {
        public SqlObjectLink(string name, Uri href)
        {
            this.Name = name;
            this.Href = href;
        }

        public string Name { get; set; }
        public Uri Href { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    class WebPageLoader
    {
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Creates new instance of <c>WebPageLoader</c>
        /// </summary>
        /// <param name="baseUrl">Base URI to use when downloading relative URI</param>
        public WebPageLoader(Uri baseUri)
        {
            this.BaseUri = baseUri;
        }

        public async Task<string> GetPageContent(Uri uri)
        {
            var client = new WebClient();
            var absoluteUri = uri.IsAbsoluteUri ? uri : new Uri(this.BaseUri, uri);
            return await client.DownloadStringTaskAsync(absoluteUri);
        }
    }
}

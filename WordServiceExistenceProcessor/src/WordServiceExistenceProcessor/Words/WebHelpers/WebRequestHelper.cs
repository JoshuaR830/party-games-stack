using System.Net;

namespace WordServiceExistenceProcessor.Words.WebHelpers
{
    public class WebRequestHelper : IWebRequestHelper
    {
        public WebRequest Create(string url)
        {
            return WebRequest.Create(url);
        }

        public HttpWebResponse GetResponse(WebRequest request)
        {
            return (HttpWebResponse) request.GetResponse();
        }
    }
}
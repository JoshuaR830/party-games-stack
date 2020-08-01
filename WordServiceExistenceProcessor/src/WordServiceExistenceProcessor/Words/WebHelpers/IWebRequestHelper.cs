using System.Net;

namespace WordServiceExistenceProcessor.Words.WebHelpers
{
    public interface IWebRequestHelper
    {
        WebRequest Create(string url);
        HttpWebResponse GetResponse(WebRequest request);
    }
}
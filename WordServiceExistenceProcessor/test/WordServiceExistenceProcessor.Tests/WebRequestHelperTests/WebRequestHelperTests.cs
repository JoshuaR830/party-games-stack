using System.Net;
using FluentAssertions;
using WordServiceExistenceProcessor.Words.WebHelpers;
using Xunit;

namespace WordServiceExistenceProcessor.Tests.WebRequestHelperTests
{
    public class WebRequestHelperTests
    {
        [Fact]
        public void TestRequest()
        {
            var url = "https://www.google.com";
            var webRequestHelper = new WebRequestHelper();
        
            var request = webRequestHelper.Create(url);

            request.Should().NotBeNull();
        }

        [Fact]
        public void TestResponse()
        {
            var url = "https://www.google.com";
            var webRequestHelper = new WebRequestHelper();

            var request = webRequestHelper.Create(url);
            var response = webRequestHelper.GetResponse(request);

            response.Should().BeOfType<HttpWebResponse>();
        }
    }
}
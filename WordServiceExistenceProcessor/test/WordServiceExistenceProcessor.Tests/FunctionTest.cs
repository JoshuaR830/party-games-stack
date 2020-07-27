using Amazon.Lambda.TestUtilities;
using Xunit;
using FluentAssertions;
namespace WordServiceExistenceProcessor.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestToUpperFunction()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new WordServiceExistenceProcessor.Function();
            var context = new TestLambdaContext();
            var upperCase = function.FunctionHandler("hello world", context);

            upperCase.Should().Be("HELLO WORLD");
        }
    }
}

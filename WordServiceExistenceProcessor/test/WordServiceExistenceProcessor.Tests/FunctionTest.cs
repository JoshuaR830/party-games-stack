// using System.Threading.Tasks;
// using Amazon.Lambda.TestUtilities;
// using Xunit;
// using FluentAssertions;
// namespace WordServiceExistenceProcessor.Tests
// {
//     public class FunctionTest
//     {
//         [Fact]
//         public async Task TestToUpperFunction()
//         {
//
//             // Invoke the lambda function and confirm the string was upper cased.
//             var function = new Function();
//             var context = new TestLambdaContext();
//             var upperCase = await function.FunctionHandler("hello world", context);
//
//             upperCase.Should().Be("HELLO WORLD");
//         }
//     }
// }

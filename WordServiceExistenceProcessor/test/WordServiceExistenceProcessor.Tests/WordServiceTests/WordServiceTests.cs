using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace WordServiceExistenceProcessor.Tests.WordServiceTests
{
    public class WordServiceTests
    {
        [Fact]
        public async Task TestToUpperFunction()
        {
            var dynamoDb = Substitute.For<IAmazonDynamoDB>();

            // dynamoDb.GetItemAsync(new GetItemRequest
            // {
            //     TableName = "WordTable",
            //     Key = new Dictionary<string, AttributeValue>
            //     {
            //         {"Word", new AttributeValue {S = input}}
            //     }
            // }).Returns(new GetItemResponse
            // {
            //     Item = new Dictionary<string, AttributeValue>()
            // });
            //
            var handler = new Handler(dynamoDb);
            var upperCase = await handler.Handle("hello world");
            upperCase.Should().Be("HELLO WORLD");
        }
    }
}
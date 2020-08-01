using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using WordServiceExistenceProcessor.DynamoDB;

namespace WordServiceExistenceProcessor.Tests.WordServiceTests
{
    public class WordServiceTests
    {
        [Fact]
        public async Task WhenWordExistsResponseShouldBeTrue()
        {
            var input = "test";
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();

            dynamoDbWrapper.GetDictionaryItem(input).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "The definition of the word"}
                }
            });
            
            var handler = new Handler(dynamoDbWrapper);
            var isWord = await handler.Handle(input);

            isWord.Should().BeTrue();
        }
        
        [Fact]
        public async Task WhenWordDoesNotExistResponseShouldBeFalse()
        {
            var input = "NotAWord";
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();

            dynamoDbWrapper.GetDictionaryItem(input).Returns(new GetItemResponse
            {
                IsItemSet = false
            });
            
            var handler = new Handler(dynamoDbWrapper);
            var isWord = await handler.Handle(input);

            isWord.Should().BeFalse();
        }
    }
}
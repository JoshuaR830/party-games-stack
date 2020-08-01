using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Xunit;
using FluentAssertions;
using NSubstitute;
using WordServiceExistenceProcessor.DynamoDB;
using WordServiceExistenceProcessor.Words.WebHelpers;
using WordServiceExistenceProcessor.Words.WordService;

namespace WordServiceExistenceProcessor.Tests.WordServiceTests
{
    public class WordServiceTests
    {
        [Fact]
        public async Task WhenWordExistsResponseShouldBeTrue()
        {
            var input = "test";
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();
            var dynamoDbBatchWrapper = Substitute.For<IBatchGetItemRequestWrapper>();
            var wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            var webDictionaryRequestHelper = Substitute.For<IWebDictionaryRequestHelper>();

            dynamoDbWrapper.GetDictionaryItem(input).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "The definition of the word"}
                }
            });
            
            dynamoDbWrapper.GetDictionaryItem(input).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "The definition of the word"}
                }
            });

            wordExistenceHelper.GetWordWithSuffix(input).Returns(new WordResponseWrapper(true,
                new WordData(input, "The definition of the word", WordStatus.Temporary)));
            
            var handler = new Handler(dynamoDbWrapper, dynamoDbBatchWrapper, wordExistenceHelper, webDictionaryRequestHelper);
            var isWord = await handler.Handle(input);

            isWord.IsSuccessful.Should().BeTrue();
            isWord.WordResponse.Word.Should().Be("test");
            isWord.WordResponse.Definition.Should().Be("The definition of the word");
            isWord.WordResponse.Status.Should().Be(WordStatus.Temporary);
        }
        
        [Fact]
        public async Task WhenWordDoesNotExistResponseShouldBeFalse()
        {
            var input = "NotAWord";
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();
            var dynamoDbBatchWrapper = Substitute.For<IBatchGetItemRequestWrapper>();
            var wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            var webDictionaryRequestHelper = Substitute.For<IWebDictionaryRequestHelper>();

            dynamoDbWrapper.GetDictionaryItem(input).Returns(new GetItemResponse
            {
                IsItemSet = false
            });
            
            var handler = new Handler(dynamoDbWrapper, dynamoDbBatchWrapper, wordExistenceHelper, webDictionaryRequestHelper);
            var isWord = await handler.Handle(input);

            isWord.IsSuccessful.Should().BeFalse();
        }
    }
}
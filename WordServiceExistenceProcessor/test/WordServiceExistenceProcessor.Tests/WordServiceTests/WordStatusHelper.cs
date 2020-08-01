using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using FluentAssertions;
using NSubstitute;
using WordServiceExistenceProcessor.DynamoDB;
using WordServiceExistenceProcessor.Words.WordService;
using Xunit;

namespace WordServiceExistenceProcessor.Tests.WordServiceTests
{
    public class WordStatusHelper
    {
        private const string SuccessWord = "test";
        private const string FailWord = "notAWord";
        private readonly WordExistenceHelper _wordExistenceHelper;

        public WordStatusHelper()
        {
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();
            
            dynamoDbWrapper.GetDictionaryItem(SuccessWord).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = SuccessWord},
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary definition"}
                }
            });
            
            dynamoDbWrapper.GetDictionaryItem(FailWord).Returns(new GetItemResponse
            {
                IsItemSet = false
            });
            
            _wordExistenceHelper = new WordExistenceHelper(dynamoDbWrapper);
        }
        
        [Fact]
        public async Task WhenWordExistsASuccessfulWrappedResponseShouldBeReturned()
        {
            var response = await _wordExistenceHelper.GetWordStatus(SuccessWord);
            var expected = new WordResponseWrapper(true, new WordData(SuccessWord, "Temporary definition", WordStatus.Temporary));

            response.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public async Task WhenWordDoesNotExistAFailedWrappedResponseShouldBeReturned()
        {
            var response = await _wordExistenceHelper.GetWordStatus(FailWord);
            var expected = new WordResponseWrapper(false);

            response.ShouldBeEquivalentTo(expected);
        }
    }
}
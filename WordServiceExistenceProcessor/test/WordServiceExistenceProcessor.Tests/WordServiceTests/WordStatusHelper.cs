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
        
        private const string ScottishWord = "scottish";
        private const string IrishWord = "irish";
        private const string ObsoleteWord = "obsolete";
        private const string ArchaicWord = "archaic";
        
        private const string FailWord = "notAWord";
        private readonly WordExistenceHelper _wordExistenceHelper;

        public WordStatusHelper()
        {
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();
            var dynamoDbBatchWrapper = Substitute.For<IBatchGetItemRequestWrapper>();

            dynamoDbWrapper.GetDictionaryItem(SuccessWord).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = SuccessWord},
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary definition"}
                }
            });
            
            dynamoDbWrapper.GetDictionaryItem(ScottishWord).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = ScottishWord},
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary scot. definition"}
                }
            });
            
            dynamoDbWrapper.GetDictionaryItem(IrishWord).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = IrishWord},
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary [irish] definition"}
                }
            });
            
            dynamoDbWrapper.GetDictionaryItem(ObsoleteWord).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = ObsoleteWord},
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary obs. definition"}
                }
            });
            
            dynamoDbWrapper.GetDictionaryItem(ArchaicWord).Returns(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = ArchaicWord},
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary archaic definition"}
                }
            });
            
            dynamoDbWrapper.GetDictionaryItem(FailWord).Returns(new GetItemResponse
            {
                IsItemSet = false
            });
            
            _wordExistenceHelper = new WordExistenceHelper(dynamoDbWrapper, dynamoDbBatchWrapper);
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
        
        [Fact]
        public async Task WhenWordExistsButIsScottishResponseShouldBeFalse()
        {
            var response = await _wordExistenceHelper.GetWordStatus(ScottishWord);
            var expected = new WordResponseWrapper(false);

            response.ShouldBeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task WhenWordExistsButIsIrishResponseShouldBeFalse()
        {
            var response = await _wordExistenceHelper.GetWordStatus(IrishWord);
            var expected = new WordResponseWrapper(false);

            response.ShouldBeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task WhenWordExistsButIsObsoleteResponseShouldBeFalse()
        {
            var response = await _wordExistenceHelper.GetWordStatus(ObsoleteWord);
            var expected = new WordResponseWrapper(false);

            response.ShouldBeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task WhenWordExistsButIsArchaicResponseShouldBeFalse()
        {
            var response = await _wordExistenceHelper.GetWordStatus(ArchaicWord);
            var expected = new WordResponseWrapper(false);

            response.ShouldBeEquivalentTo(expected);
        }
    }
}
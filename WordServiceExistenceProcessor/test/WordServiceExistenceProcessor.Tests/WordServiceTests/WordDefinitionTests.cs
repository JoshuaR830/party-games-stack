using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using FluentAssertions;
using NSubstitute;
using WordServiceExistenceProcessor.DynamoDB;
using WordServiceExistenceProcessor.Words.WordService;
using Xunit;

namespace WordServiceExistenceProcessor.Tests.WordServiceTests
{
    public class WordDefinitionTests
    {
        private const string TemporaryWord = "temporary";
        private const string PermanentWord = "permanent";
        private const string BothWord = "both";
        private const string NeitherWord = "neither";
        private const string FailWord = "notAWord";
        private readonly WordExistenceHelper _wordExistenceHelper;

        public WordDefinitionTests()
        {
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();
            var dynamoDbBatchWrapper = Substitute.For<IBatchGetItemRequestWrapper>();

            _wordExistenceHelper = new WordExistenceHelper(dynamoDbWrapper, dynamoDbBatchWrapper);
        }

        [Fact]
        public void WhenOnlyATemporaryDefinitionIsSet()
        {
            var response = _wordExistenceHelper.GetDefinition(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = TemporaryWord},
                    ["Status"] = new AttributeValue {S = "Temporary"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary definition"},
                }
            });
            
            response.Should().Be("Temporary definition");
        }
        
        [Fact]
        public void WhenOnlyAPermanentDefinitionIsSet()
        {
            var response = _wordExistenceHelper.GetDefinition(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = PermanentWord},
                    ["Status"] = new AttributeValue {S = "Permanent"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary definition"},
                    ["PermanentDefinition"] = new AttributeValue {S = "Permanent definition"}
                }
            });
            
            response.Should().Be("Permanent definition");
        }
        
        [Fact]
        public void WhenBothAPermanentAndTemporaryDefinitionAreSet()
        {

            var response = _wordExistenceHelper.GetDefinition(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["Word"] = new AttributeValue {S = BothWord},
                    ["Status"] = new AttributeValue {S = "Permanent"},
                    ["TemporaryDefinition"] = new AttributeValue {S = "Temporary definition"},
                    ["PermanentDefinition"] = new AttributeValue {S = "Permanent definition"}
                }
            });

            response.Should().Be("Permanent definition");
        }

        [Fact]
        public void WhenNeitherAPermanentOrATemporaryDefinitionAreSet()
        {
            var response = _wordExistenceHelper.GetDefinition(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                   ["Word"] = new AttributeValue {S = NeitherWord},
                   ["Status"] = new AttributeValue {S = "Temporary"},
                }
            });
            
            response.Should().Be("");
        }
    }
}
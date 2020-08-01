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
    public class BatchGetItemsTests
    {
        private readonly WordExistenceHelper _wordExistenceHelper;

        public BatchGetItemsTests()
        {
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();
            var dynamoDbBatchWrapper = Substitute.For<IBatchGetItemRequestWrapper>();

            dynamoDbBatchWrapper.GetDictionaryItems(new List<string> {"run", "running"})
                .Returns(new BatchGetItemResponse
                {
                    Responses = new Dictionary<string, List<Dictionary<string, AttributeValue>>>
                    {
                        {
                            "WordTable", new List<Dictionary<string, AttributeValue>>
                            {
                                new Dictionary<string, AttributeValue>()
                                {
                                    {"Word", new AttributeValue {S = "run"}},
                                    {"TemporaryDefinition", new AttributeValue {S = "Moving your legs at pace"}}
                                }
                            }
                        }
                    }
                });

            dynamoDbBatchWrapper.GetDictionaryItems(new List<string> {"quick", "quickly"})
                .Returns(new BatchGetItemResponse
                {
                    Responses = new Dictionary<string, List<Dictionary<string, AttributeValue>>>
                    {
                        {
                            "WordTable", new List<Dictionary<string, AttributeValue>>
                            {
                                new Dictionary<string, AttributeValue>()
                                {
                                    {"Word", new AttributeValue {S = "quick"}}
                                },
                                new Dictionary<string, AttributeValue>()
                                {
                                    {"Word", new AttributeValue {S = "quickly"}}
                                }
                            }
                        }
                    }
                });
            
            _wordExistenceHelper = new WordExistenceHelper(dynamoDbWrapper, dynamoDbBatchWrapper);
        }

        [Fact]
        public async Task WhenTheWordDoesNotExistWithTheSuffixButExistsWithoutItThenOnlyTheShortWordShouldBeReturned()
        {
            var response = await _wordExistenceHelper.GetWordWithSuffix("running");
            response.ShouldBeEquivalentTo(new WordResponseWrapper(false));
        }

        [Fact]
        public async Task WhenTheWordDoesExistWithTheSuffixThenOnlyTheWordWithTheSuffixShouldBeReturned()
        {
            var response = await _wordExistenceHelper.GetWordWithSuffix("quickly");
            response.ShouldBeEquivalentTo(new WordResponseWrapper(false));
        }
    }
}
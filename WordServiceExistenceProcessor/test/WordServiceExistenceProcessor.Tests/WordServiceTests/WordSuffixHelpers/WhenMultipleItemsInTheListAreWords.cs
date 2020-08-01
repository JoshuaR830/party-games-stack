using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using FluentAssertions;
using NSubstitute;
using WordServiceExistenceProcessor.DynamoDB;
using WordServiceExistenceProcessor.Words.WordService;
using Xunit;

namespace WordServiceExistenceProcessor.Tests.WordServiceTests.WordSuffixHelpers
{
    public class WhenMultipleItemsInTheListAreWords
    {
        private readonly WordExistenceHelper _wordExistenceHelper;

        public WhenMultipleItemsInTheListAreWords()
        {
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();
            var dynamoDbBatchWrapper = Substitute.For<IBatchGetItemRequestWrapper>();
            
            dynamoDbBatchWrapper.GetDictionaryItems(Arg.Any<List<string>>())
                .Returns(Task.FromResult(new BatchGetItemResponse
                {
                    Responses = new Dictionary<string, List<Dictionary<string, AttributeValue>>>
                    {
                        {
                            "WordTable", new List<Dictionary<string, AttributeValue>>
                            {
                                new Dictionary<string, AttributeValue>()
                                {
                                    {"Word", new AttributeValue {S = "quick"}},
                                    {"TemporaryDefinition", new AttributeValue {S = "Very fast"}},
                                    {"Status", new AttributeValue {S = "Temporary"}}
                                },
                                new Dictionary<string, AttributeValue>()
                                {
                                    {"Word", new AttributeValue {S = "quickly"}},
                                    {"TemporaryDefinition", new AttributeValue {S = "Doing something very fast"}},
                                    {"Status", new AttributeValue {S = "Temporary"}}
                                }
                            }
                        }
                    }
                }));

            _wordExistenceHelper = new WordExistenceHelper(dynamoDbWrapper, dynamoDbBatchWrapper);
        }
        
        [Fact]
        public async Task ThenOnlyTheWordWithTheSuffixShouldBeReturned()
        {
            var response = await _wordExistenceHelper.GetWordWithSuffix("quickly");
            response.ShouldBeEquivalentTo(new WordResponseWrapper(true, new WordData("quickly", "Doing something very fast", WordStatus.Temporary)));
        }
    }
}
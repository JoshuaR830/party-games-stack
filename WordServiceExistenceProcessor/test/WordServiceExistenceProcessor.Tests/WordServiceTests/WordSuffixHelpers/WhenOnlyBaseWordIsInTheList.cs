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
    public class WhenOnlyBaseWordIsInTheList
    {
        private readonly WordExistenceHelper _wordExistenceHelper;

        public WhenOnlyBaseWordIsInTheList()
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
                                    {"Word", new AttributeValue {S = "run"}},
                                    {"TemporaryDefinition", new AttributeValue {S = "Moving your legs at pace"}},
                                    {"Status", new AttributeValue {S = "Temporary"}}
                                }
                            }
                        }
                    }
                }));

            _wordExistenceHelper = new WordExistenceHelper(dynamoDbWrapper, dynamoDbBatchWrapper);
        }
        
        [Fact]
        public async Task WhenTheWordDoesNotExistWithTheSuffixButExistsWithoutItThenOnlyTheShortWordShouldBeReturned()
        {
            var response = await _wordExistenceHelper.GetWordWithSuffix("running");
            response.ShouldBeEquivalentTo(new WordResponseWrapper(true, new WordData("run", "Moving your legs at pace", WordStatus.Temporary)));
        }
    }
}
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
    public class WhenThereAreNoItemsInTheList
    {
        private readonly WordExistenceHelper _wordExistenceHelper;

        public WhenThereAreNoItemsInTheList()
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
            var response = await _wordExistenceHelper.GetWordWithSuffix("swimming");
            response.ShouldBeEquivalentTo(new WordResponseWrapper(false));
        }
    }
}
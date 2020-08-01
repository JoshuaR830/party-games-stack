using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace WordServiceExistenceProcessor.DynamoDB
{
    public interface IBatchGetItemRequestWrapper
    {
        Task<BatchGetItemResponse> GetDictionaryItems(List<string> words);
    }
}
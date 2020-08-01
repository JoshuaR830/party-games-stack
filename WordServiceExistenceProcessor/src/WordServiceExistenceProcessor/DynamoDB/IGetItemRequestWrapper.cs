using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace WordServiceExistenceProcessor.DynamoDB
{
    public interface IGetItemRequestWrapper
    {
        Task<GetItemResponse> GetDictionaryItem(string word);
    }
}
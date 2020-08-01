using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace WordServiceExistenceProcessor.Words.WordService
{
    public interface IWordExistenceHelper
    {
        Task<WordResponseWrapper> GetWordStatus(string word);
        string GetDefinition(GetItemResponse wordResponse);
    }
}
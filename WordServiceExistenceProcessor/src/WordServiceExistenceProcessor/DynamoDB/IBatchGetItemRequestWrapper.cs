using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordServiceExistenceProcessor.DynamoDB
{
    public interface IBatchGetItemRequestWrapper
    {
        Task GetDictionaryItems(List<string> words);
    }
}
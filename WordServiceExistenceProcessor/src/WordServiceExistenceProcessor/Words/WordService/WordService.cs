using System;
using System.Threading.Tasks;
using WordServiceExistenceProcessor.DynamoDB;

namespace WordServiceExistenceProcessor.Words.WordService
{
    public class WordService : IWordService
    {
        private readonly IGetItemRequestWrapper _dynamoDbWrapper;

        public WordService(IGetItemRequestWrapper dynamoDbWrapper)
        {
            _dynamoDbWrapper = dynamoDbWrapper;
        }

        public async Task<WordResponseWrapper> GetWordStatus(string word)
        {
            var wordResponse = await _dynamoDbWrapper.GetDictionaryItem(word);

            if (!wordResponse.IsItemSet)
                return new WordResponseWrapper(false);

            var wordItem = wordResponse.Item;
            var wordData = new WordData(wordItem["Word"].S, wordItem["TemporaryDefinition"].S, wordItem["PermanentDefinition"].S, Enum.Parse<WordStatus>(wordItem["Status"].S) );
            
            return new WordResponseWrapper(true, wordData);
        }
    }
}
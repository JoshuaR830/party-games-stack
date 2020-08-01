// using Chat.WordGame.LocalDictionaryHelpers;

using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using WordServiceExistenceProcessor.DynamoDB;

namespace WordServiceExistenceProcessor.Words.WordService
{
    public class WordExistenceHelper : IWordExistenceHelper
    {
        private readonly IGetItemRequestWrapper _dynamoDbWrapper;

        public WordExistenceHelper(IGetItemRequestWrapper dynamoDbWrapper)
        {
            _dynamoDbWrapper = dynamoDbWrapper;
        }
        
        // ToDo: see if the ending is suffixed and cut off the suffix
        // ToDo: create a batch get for bot the suffixed word and the normal word
        // ToDo: I think this should use 1 rcu still
        // ToDo: batching them together makes for a more efficient system
        // ToDo: the suffix check would have to come before the word status so that it is known
        // ToDo: the word status check will now have to work out which response to give -> if the response doesn't exist for the big one

        public async Task<WordResponseWrapper> GetWordStatus(string word)
        {
            var wordResponse = await _dynamoDbWrapper.GetDictionaryItem(word);

            if (!wordResponse.IsItemSet)
                return new WordResponseWrapper(false);

            var wordItem = wordResponse.Item;

            var definition = this.GetDefinition(wordResponse);
            var wordData = new WordData(wordItem["Word"].S, definition, Enum.Parse<WordStatus>(wordItem["Status"].S) );
            
            return new WordResponseWrapper(true, wordData);
        }

        public string GetDefinition(GetItemResponse wordResponse)
        {
            var wordItem = wordResponse.Item;
            var definition = "";

            if (wordItem.ContainsKey("TemporaryDefinition"))
                definition = wordItem["TemporaryDefinition"].S;

            if (wordItem.ContainsKey("PermanentDefinition"))
                definition = wordItem["PermanentDefinition"].S;
            
            return definition;
        }
    }
}
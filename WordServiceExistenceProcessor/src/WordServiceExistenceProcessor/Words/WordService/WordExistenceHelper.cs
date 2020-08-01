using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using WordServiceExistenceProcessor.DynamoDB;

namespace WordServiceExistenceProcessor.Words.WordService
{
    public class WordExistenceHelper : IWordExistenceHelper
    {
        private readonly IGetItemRequestWrapper _dynamoDbWrapper;
        private readonly IBatchGetItemRequestWrapper _dynamoDbBatchWrapper;

        public WordExistenceHelper(IGetItemRequestWrapper dynamoDbWrapper, IBatchGetItemRequestWrapper dynamoDbBatchWrapper)
        {
            _dynamoDbWrapper = dynamoDbWrapper;
            _dynamoDbBatchWrapper = dynamoDbBatchWrapper;
        }

        public async Task<WordResponseWrapper> GetWordStatus(string word)
        {
            var wordResponse = await _dynamoDbWrapper.GetDictionaryItem(word);

            if (!wordResponse.IsItemSet)
                return new WordResponseWrapper(false);

            var wordItem = wordResponse.Item;

            var definition = GetDefinition(wordResponse.Item);
            var wordData = new WordData(wordItem["Word"].S, definition, Enum.Parse<WordStatus>(wordItem["Status"].S) );
            
            return new WordResponseWrapper(true, wordData);
        }

        public string GetDefinition(Dictionary<string, AttributeValue> wordItem)
        {
            var definition = "";

            if (wordItem.ContainsKey("TemporaryDefinition"))
                definition = wordItem["TemporaryDefinition"].S;

            if (wordItem.ContainsKey("PermanentDefinition"))
                definition = wordItem["PermanentDefinition"].S;
            
            return definition;
        }

        public async Task<WordResponseWrapper> GetWordWithSuffix(string word)
        {
            var potentialWords = GetWordFragments(word);
            var batch = await _dynamoDbBatchWrapper.GetDictionaryItems(potentialWords);

            if (batch == null)
                return new WordResponseWrapper(false);

            var response = batch.Responses["WordTable"];
            
            if (!response.Any(x => x.ContainsKey("Word")))
                return new WordResponseWrapper(false);

            var status = WordStatus.Temporary;
            
            if (response.Any(x => x["Word"].S == word))
            {
                var myWord = response.First(x => x["Word"].S == word);
                
                if (myWord.ContainsKey("Status"))
                    status = Enum.Parse<WordStatus>( myWord["Status"].S);
                
                return new WordResponseWrapper(true, new WordData(word, GetDefinition(myWord), status));
            }

            var alternativeWord = response.FirstOrDefault();
            
            if (alternativeWord == null)
                return new WordResponseWrapper(false);

            if (alternativeWord.ContainsKey("Status"))
                status = Enum.Parse<WordStatus>(alternativeWord["Status"].S);
            
            return new WordResponseWrapper(true, new WordData(alternativeWord["Word"].S, GetDefinition(alternativeWord), status));
        }

        public List<string> GetWordFragments(string word)
        {
            var endings = new List<string> {"ning", "ing", "ed", "er", "es", "ly", "s", "d"};

            endings = endings
                .Where(x => x.Length < word.Length)
                .OrderByDescending(s => s.Length)
                .ToList();
            
            var myWordEndings = new List<string> { word };

            foreach (var ending in endings)
            {
                var shortenedWord = word.Remove(word.Length - ending.Length);

                if (word.Substring(word.Length - ending.Length) != ending)
                    continue;
                
                myWordEndings.Add(shortenedWord); 
            }

            return myWordEndings;
        }
    }
}
﻿// using Chat.WordGame.LocalDictionaryHelpers;

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

            foreach (var response in batch.Responses)
            {
                Console.WriteLine(response.Key);
                Console.WriteLine(response.Value);
                
                foreach (var thing in response.Value)
                {
                    var definition = GetDefinition(thing);
                }
            }
            

            
            //
            // var wordResponses = new List<WordResponseWrapper>();
            // foreach (var itemsResponse in batchResponse.Responses)
            // {
            //     var something = itemsResponse.Value;
            //     foreach (var thing in something)
            //     {
            //         var value = "";
            //         if (thing.ContainsKey("Word"))
            //             value += thing["Word"].S + ", ";
            //         if (thing.ContainsKey("TemporaryDefinition"))
            //             value += thing["TemporaryDefinition"].S + ", ";
            //         if (thing.ContainsKey("PermanentDefinition"))
            //             value += thing["PermanentDefinition"].S + ", ";
            //         if (thing.ContainsKey("Status"))
            //             value += thing["Status"].S;
            //         
            //         wordResponses.Add(new WordResponseWrapper(true, new WordData()));
            //         Console.WriteLine(value);
            //     }
            // }
            return new WordResponseWrapper(false);
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
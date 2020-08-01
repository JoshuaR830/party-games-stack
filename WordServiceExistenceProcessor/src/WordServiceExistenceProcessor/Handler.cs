using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WordServiceExistenceProcessor.DynamoDB;

namespace WordServiceExistenceProcessor
{
    public class Handler
    {
        private readonly IGetItemRequestWrapper _dynamoDbWrapper;
        private readonly IBatchGetItemRequestWrapper _dynamoDbBatchWrapper;
        
        public Handler(IGetItemRequestWrapper dynamoDbWrapper, IBatchGetItemRequestWrapper dynamoDbBatchWrapper)
        {
            _dynamoDbWrapper = dynamoDbWrapper;
            _dynamoDbBatchWrapper = dynamoDbBatchWrapper;
        }

        public async Task<bool> Handle(string input)
        {
            Console.WriteLine(input);
            // var request = new PutItemRequest
            // {
            //     TableName = "WordTable",
            //     Item = new Dictionary<string, AttributeValue>
            //     {
            //         { "Word", new AttributeValue { S = input}}
            //     }
            // };
            //

            

            // ToDo: first of all establish which ending the word ends with
            
            
            
            // ToDo: build the list up from scratch based on endings - so you get a list of items - contains original word and the original word with every possible ending removed 
            // ToDo: e.g. runners would have a list ["runners", "runner", "runn", "run"]
            // ToDo: are any of those in the dictionary -> run would be -> because it is -> ask dictionary -> does the original word exist?
            // ToDo: create a list of word response wrappers
            // ToDo: process that list - first ask - does the word exist in the list
            // ToDo: if the word does exist in the list - great


            var response = await _dynamoDbWrapper.GetDictionaryItem(input);
            Console.WriteLine(JsonConvert.SerializeObject(response));
            return response.IsItemSet;
        }
}
    }

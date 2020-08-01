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
            
            var response = await _dynamoDbWrapper.GetDictionaryItem(input);
            Console.WriteLine(JsonConvert.SerializeObject(response));
            return response.IsItemSet;
        }
    }
}
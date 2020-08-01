using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using WordServiceExistenceProcessor.DynamoDB;

namespace WordServiceExistenceProcessor
{
    public class Handler
    {
        private readonly IGetItemRequestWrapper _dynamoDbWrapper;
        
        public Handler(IGetItemRequestWrapper dynamoDbWrapper)
        {
            _dynamoDbWrapper = dynamoDbWrapper;
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
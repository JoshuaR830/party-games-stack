using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace WordServiceExistenceProcessor
{
    public class Handler
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public Handler(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<string> Handle(string input)
        {
            Console.WriteLine(input);
            var request = new PutItemRequest
            {
                TableName = "WordTable",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Word", new AttributeValue { S = input}}
                }
            };
            
            await _dynamoDb.PutItemAsync(request);

            return input?.ToLower();
        }
    }
}
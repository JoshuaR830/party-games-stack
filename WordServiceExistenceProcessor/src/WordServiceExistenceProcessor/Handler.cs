using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;

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
            // var request = new PutItemRequest
            // {
            //     TableName = "WordTable",
            //     Item = new Dictionary<string, AttributeValue>
            //     {
            //         { "Word", new AttributeValue { S = input}}
            //     }
            // };
            //
            
            // var request = new GetItemRequest
            // {
            //     TableName = "WordTable",
            //     Key = new Dictionary<string, AttributeValue>
            //     {
            //         { "Word", new AttributeValue { S = input}}
            //     },
            //     ProjectionExpression = "#s, #temp, #perm",
            //     ExpressionAttributeNames = new Dictionary<string, string>{
            //         {"#s", "Status"},
            //         {"#temp", "TemporaryDefinition"},
            //         {"#perm", "PermanentDefinition"}
            //     }
            // };
            
            
            // await _dynamoDb.PutItemAsync(request);
            // var response = await _dynamoDb.GetItemAsync(request);
            // return JsonConvert.SerializeObject(response);

            return input.ToUpper();
        }
    }
}
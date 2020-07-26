using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WordServiceExistenceProcessor
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(string input, ILambdaContext context)
        {
            var client = new AmazonDynamoDBClient();
            var request = new PutItemRequest
            {
                TableName = "WordTable",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Word", new AttributeValue { S = input}}
                }
            };
            await client.PutItemAsync(request);
            return input?.ToUpper();
        }
    }
}

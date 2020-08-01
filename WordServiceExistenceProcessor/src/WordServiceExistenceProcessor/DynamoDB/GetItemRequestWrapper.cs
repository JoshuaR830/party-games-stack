using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace WordServiceExistenceProcessor.DynamoDB
{
    public class GetItemRequestWrapper : IGetItemRequestWrapper
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public GetItemRequestWrapper(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public async Task<GetItemResponse> GetDictionaryItem(string word)
        {
            var request = new GetItemRequest
            {
                TableName = "WordTable",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Word", new AttributeValue { S = word}}
                },
                ProjectionExpression = "#s, #temp, #perm",
                ExpressionAttributeNames = new Dictionary<string, string>{
                    {"#s", "Status"},
                    {"#temp", "TemporaryDefinition"},
                    {"#perm", "PermanentDefinition"}
                }
            };
            
            return await _dynamoDb.GetItemAsync(request);
        }
    }
}
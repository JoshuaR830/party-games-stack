using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace WordServiceExistenceProcessor.DynamoDB
{
    public class BatchGetItemRequestWrapper : IBatchGetItemRequestWrapper
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public BatchGetItemRequestWrapper(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public async Task<BatchGetItemResponse> GetDictionaryItems(List<string> words)
        {
            var keys = new List<Dictionary<string, AttributeValue>>();
            
            foreach (var word in words)
            {
                keys.Add(new Dictionary<string, AttributeValue>
                {
                    {"Word", new AttributeValue { S = word}}
                });
            }

            return await _dynamoDb.BatchGetItemAsync(new BatchGetItemRequest
            {
                RequestItems = new Dictionary<string, KeysAndAttributes>
                {
                    {
                        "WordTable", new KeysAndAttributes
                        {
                            Keys = keys,
                            ProjectionExpression = "#w, #s, #t, #p",
                            ExpressionAttributeNames = new Dictionary<string, string>
                            {
                                {"#w", "Word"},
                                {"#s", "Status"},
                                {"#t", "TemporaryDefinition"},
                                {"#p", "PermanentDefinition"}
                            }
                        }
                    }
                }
            });

            
        }
    }
}
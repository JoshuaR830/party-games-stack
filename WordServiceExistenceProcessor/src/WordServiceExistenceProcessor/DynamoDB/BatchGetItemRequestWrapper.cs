using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;

namespace WordServiceExistenceProcessor.DynamoDB
{
    public class BatchGetItemRequestWrapper : IBatchGetItemRequestWrapper
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public BatchGetItemRequestWrapper(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public async Task GetDictionaryItems(List<string> words)
        {
            var keys = new List<Dictionary<string, AttributeValue>>();
            
            foreach (var word in words)
            {
                keys.Add(new Dictionary<string, AttributeValue>
                {
                    {"Word", new AttributeValue { S = word}}
                });
            }

            var items = await _dynamoDb.BatchGetItemAsync(new BatchGetItemRequest
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
            
            foreach (var itemsResponse in items.Responses)
            {
                var something = itemsResponse.Value;
                foreach (var thing in something)
                {
                    var value = "";
                    if (thing.ContainsKey("Word"))
                        value += thing["Word"].S + ", ";
                    if (thing.ContainsKey("TemporaryDefinition"))
                        value += thing["TemporaryDefinition"].S + ", ";
                    if (thing.ContainsKey("PermanentDefinition"))
                        value += thing["PermanentDefinition"].S + ", ";
                    if (thing.ContainsKey("Status"))
                        value += thing["Status"].S;
                    
                    // ToDo: first of all establish which ending the word ends with
                    // ToDo: build the list up from scratch based on endings - so you get a list of items - contains original word and the original word with every possible ending removed 
                    // ToDo: e.g. runners would have a list ["runners", "runner", "runn", "run"]
                    // ToDo: are any of those in the dictionary -> run would be -> because it is -> ask dictionary -> does the original word exist?
                    // ToDo: create a list of word response wrappers
                    // ToDo: process that list - first ask - does the word exist in the list
                    // ToDo: if the word does exist in the list - great

                    Console.WriteLine(value);
                }
            }
        }
    }
}
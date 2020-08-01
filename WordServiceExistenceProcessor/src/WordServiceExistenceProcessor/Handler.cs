using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WordServiceExistenceProcessor.DynamoDB;
using WordServiceExistenceProcessor.Words.WebHelpers;
using WordServiceExistenceProcessor.Words.WordService;

namespace WordServiceExistenceProcessor
{
    public class Handler
    {
        private readonly IGetItemRequestWrapper _dynamoDbWrapper;
        private readonly IBatchGetItemRequestWrapper _dynamoDbBatchWrapper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWebDictionaryRequestHelper _webDictionaryRequestHelper;
        
        public Handler(IGetItemRequestWrapper dynamoDbWrapper, IBatchGetItemRequestWrapper dynamoDbBatchWrapper, IWordExistenceHelper wordExistenceHelper, IWebDictionaryRequestHelper webDictionaryRequestHelper)
        {
            _dynamoDbWrapper = dynamoDbWrapper;
            _dynamoDbBatchWrapper = dynamoDbBatchWrapper;
            _wordExistenceHelper = wordExistenceHelper;
            _webDictionaryRequestHelper = webDictionaryRequestHelper;
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

            var response2 = await _wordExistenceHelper.GetWordWithSuffix(input);

            if (response2 != null)
            {
                if (input != response2?.WordResponse?.Word)
                {
                    Console.WriteLine("Hmm - looks like there is a problem");
                    var responseText = _webDictionaryRequestHelper.MakeContentRequest(response2.WordResponse?.Word)
                        .ToLower();
                    if (!responseText.Contains("word forms"))
                        return false;

                    var finished = new WordResponseWrapper(false);

                    if (responseText.Contains(input))
                        finished = new WordResponseWrapper(true, new WordData(input, response2?.WordResponse?.Definition, WordStatus.Temporary));

                    Console.WriteLine($"Processed response: {JsonConvert.SerializeObject(finished)}");
                }
            }

            Console.WriteLine($"Batch response: {response2?.WordResponse?.Word}");
            
            var response = await _dynamoDbWrapper.GetDictionaryItem(input);
            Console.WriteLine(JsonConvert.SerializeObject(response));
            return response.IsItemSet;
        }
    }
}
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

        public async Task<WordResponseWrapper> Handle(string input)
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

            var response = await _wordExistenceHelper.GetWordWithSuffix(input);
            var finished = new WordResponseWrapper(false);

            Console.WriteLine($"Batch response: {response?.WordResponse?.Word}");

            if (response == null || response.WordResponse == null)
                return finished;
            
            if (input != response.WordResponse.Word)
            {
                Console.WriteLine("Hmm - looks like there is a problem");
                
                var responseText = _webDictionaryRequestHelper.MakeContentRequest(response.WordResponse?.Word)
                    .ToLower();
                if (!responseText.Contains("word forms"))
                    return finished;
                
                if (responseText.Contains(input))
                    finished = new WordResponseWrapper(true, new WordData(input, response?.WordResponse?.Definition, WordStatus.Suffix));
            }

            if (input == response.WordResponse.Word)
                finished = new WordResponseWrapper(true, new WordData(input, response.WordResponse?.Definition, response.WordResponse.Status));

            Console.WriteLine($"Processed response: {JsonConvert.SerializeObject(finished)}");
            
            var response3 = await _dynamoDbWrapper.GetDictionaryItem(input);
            Console.WriteLine(JsonConvert.SerializeObject(response3));
            
            return finished;
        }
    }
}
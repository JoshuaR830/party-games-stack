using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WordServiceExistenceProcessor.DynamoDB;
using WordServiceExistenceProcessor.Words.WordService;

namespace WordServiceExistenceProcessor
{
    public class Handler
    {
        private readonly IGetItemRequestWrapper _dynamoDbWrapper;
        private readonly IBatchGetItemRequestWrapper _dynamoDbBatchWrapper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        
        public Handler(IGetItemRequestWrapper dynamoDbWrapper, IBatchGetItemRequestWrapper dynamoDbBatchWrapper, IWordExistenceHelper wordExistenceHelper)
        {
            _dynamoDbWrapper = dynamoDbWrapper;
            _dynamoDbBatchWrapper = dynamoDbBatchWrapper;
            _wordExistenceHelper = wordExistenceHelper;
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

            if (input != response2?.WordResponse?.Word)
            {
                Console.WriteLine("Hmm - looks like there is a problem");
            }
            
            Console.WriteLine($"Batch response: {response2?.WordResponse?.Word}");
            
            var response = await _dynamoDbWrapper.GetDictionaryItem(input);
            Console.WriteLine(JsonConvert.SerializeObject(response));
            return response.IsItemSet;
        }
    }
}
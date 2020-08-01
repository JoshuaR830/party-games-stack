using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using WordServiceExistenceProcessor.DynamoDB;
using WordServiceExistenceProcessor.Words.WebHelpers;
using WordServiceExistenceProcessor.Words.WordService;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace WordServiceExistenceProcessor
{
    public class Function
    {
        private ServiceCollection _serviceCollection;
        
        public Function()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDefaultAWSOptions(new AWSOptions());
            _serviceCollection.AddAWSService<IAmazonDynamoDB>();
            _serviceCollection.AddTransient<IGetItemRequestWrapper, GetItemRequestWrapper>();
            _serviceCollection.AddTransient<IBatchGetItemRequestWrapper, BatchGetItemRequestWrapper>();
            _serviceCollection.AddTransient<IWordExistenceHelper, WordExistenceHelper>();
            _serviceCollection.AddTransient<IWebDictionaryRequestHelper, WebDictionaryRequestHelper>();
            _serviceCollection.AddTransient<IWebRequestHelper, WebRequestHelper>();
            _serviceCollection.AddTransient<Handler>();
        }
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> FunctionHandler(string input, ILambdaContext context)
        {
            using (ServiceProvider serviceProvider = _serviceCollection.BuildServiceProvider())
            {
                return await serviceProvider.GetService<Handler>().Handle(input);
            }
        }
    }
}

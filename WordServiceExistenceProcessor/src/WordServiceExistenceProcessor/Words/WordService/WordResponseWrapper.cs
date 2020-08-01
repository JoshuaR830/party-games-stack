namespace WordServiceExistenceProcessor.Words.WordService
{
    public class WordResponseWrapper
    {
        public bool IsSuccessful { get; }
        public WordData WordResponse { get; }

        public WordResponseWrapper(bool isSuccessful, WordData wordResponse = null)
        {
            IsSuccessful = isSuccessful;
            WordResponse = wordResponse;
        }
    }
}
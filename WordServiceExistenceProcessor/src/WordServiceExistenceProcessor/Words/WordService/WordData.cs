using System.Collections.Generic;

namespace WordServiceExistenceProcessor.Words.WordService
{
    public class WordData
    {
        public string Word { get; }
        public string Definition { get; }
        public WordStatus Status { get; }

        public WordData(string word, string definition, WordStatus status)
        {
            Word = word;
            Definition = definition;
            Status = status;
        }
    }
}
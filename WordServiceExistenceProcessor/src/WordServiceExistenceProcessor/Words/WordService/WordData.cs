using System.Collections.Generic;

namespace WordServiceExistenceProcessor.Words.WordService
{
    public class WordData
    {
        public string Word { get; }
        public string TemporaryDefinition { get; }
        public string PermanentDefinition { get; }
        public WordStatus Status { get; }

        public WordData(string word, string temporaryDefinition, string permanentDefinition, WordStatus status)
        {
            Word = word;
            TemporaryDefinition = temporaryDefinition;
            PermanentDefinition = permanentDefinition;
            Status = status;
        }
    }
}
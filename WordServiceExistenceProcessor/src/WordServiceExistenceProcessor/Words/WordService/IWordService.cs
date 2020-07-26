namespace Chat.WordGame.WordHelpers
{
    public interface IWordService
    {
        bool GetWordStatus(string filename, string word);
        string GetDefinition(string filename, string word);
        WordCategory GetCategory(string filename, string word);
        void AddNewWordToDictionary(string filename, string word, string definition, WordCategory category = WordCategory.None);
        void UpdateExistingWord(string filename, string word, string definition, WordCategory category = WordCategory.None);
        void UpdateCategory(string filename, string word, WordCategory category);
        void ToggleIsWordInDictionary(string filename, string word, bool expectedNewStatus);
        void AddWordToGuessedWords(string dictionaryFilename, string guessedWordsFilename, string word);
        void AmendDictionary(string filename, string word, string definition, WordCategory category = WordCategory.None);
        void UpdateDictionaryFile();
        void UpdateGuessedWordsFile();
    }
}
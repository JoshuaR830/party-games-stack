using System.Threading.Tasks;

namespace WordServiceExistenceProcessor.Words.WordService
{
    public interface IWordService
    {
        Task<WordResponseWrapper> GetWordStatus(string word);
    }
}
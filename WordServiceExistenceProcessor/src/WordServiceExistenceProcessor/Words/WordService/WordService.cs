// ﻿using System;
// using System.Linq;
// using Chat.WordGame.LocalDictionaryHelpers;
// using Newtonsoft.Json;
//
// namespace Chat.WordGame.WordHelpers
// {
//     public class WordService : IWordService
//     {
//         private readonly IWordHelper _wordHelper;
//         private readonly IWordExistenceHelper _wordExistenceHelper;
//         private readonly IWordDefinitionHelper _wordDefinitionHelper;
//         private readonly IFileHelper _fileHelper;
//         private readonly IFilenameHelper _filenameHelper;
//         
//         private readonly GuessedWords _guessedWords;
//
//         public WordService(IWordExistenceHelper wordExistenceHelper, IWordHelper wordHelper, IWordDefinitionHelper wordDefinitionHelper, IFileHelper fileHelper, IFilenameHelper filenameHelper)
//         {
//             _wordExistenceHelper = wordExistenceHelper;
//             _wordHelper = wordHelper;
//             _wordDefinitionHelper = wordDefinitionHelper;
//             _fileHelper = fileHelper;
//             _filenameHelper = filenameHelper;
//
//             if (_guessedWords == null)
//             {
//                 Console.WriteLine("Guessed words");
//                 var json = _fileHelper.ReadFile(_filenameHelper.GetGuessedWordsFilename());
//                 _guessedWords = JsonConvert.DeserializeObject<GuessedWords>(json) ?? new GuessedWords();
//
//             }
//         }
//
//         public bool GetWordStatus(string filename, string word)
//         {
//             var wordExists = _wordExistenceHelper.DoesWordExist(word);
//             
//             if (wordExists)
//                 return true;
//
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//             wordExists = _wordHelper.StrippedSuffixDictionaryCheck(wordDictionary, word);
//
//             return wordExists;
//         }
//
//         public string GetDefinition(string filename, string word)
//         {
//             if (GetWordStatus(filename, word))
//                 return _wordDefinitionHelper.GetDefinitionForWord(word);
//
//             return null;
//         }
//         
//         public WordCategory GetCategory(string filename, string word)
//         {
//             if (GetWordStatus(filename, word))
//             {
//                 var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//                 return wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList()[0].Category;
//             }
//
//             return WordCategory.None;
//         }
//
//         public void AmendDictionary(string filename, string word, string definition, WordCategory category = WordCategory.None)
//         {
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//
//             if (wordDictionary.Words.Any(x => x.Word.ToLower() == word.ToLower()))
//             {
//                 UpdateExistingWord(filename, word, definition, category);
//             }
//             else
//             {
//                 AddNewWordToDictionary(filename, word, definition, category);
//             }
//             
//         }
//
//         public void AddNewWordToDictionary(string filename, string word, string definition, WordCategory category = WordCategory.None)
//         {
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//
//             wordDictionary.Words.Add(new WordData
//             {
//                 Word = word,
//                 PermanentDefinition = definition,
//                 TemporaryDefinition = null,
//                 Status = WordStatus.Permanent,
//                 Category = category
//             });
//         }
//
//         public void UpdateExistingWord(string filename, string word, string definition, WordCategory category = WordCategory.None)
//         {
//             if (word == "" || definition == "")
//                 return;
//          
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//
//             var wordList = wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
//
//             if (!wordList.Any())
//                 return;
//
//             var item = wordDictionary.Words.IndexOf(wordList.First());
//             wordDictionary.Words[item].PermanentDefinition = definition;
//             wordDictionary.Words[item].Status = WordStatus.Permanent;
//             wordDictionary.Words[item].Category = category;
//         }
//
//         public void UpdateCategory(string filename, string word, WordCategory category)
//         {
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//
//             var wordList = wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
//
//             if (!wordList.Any())
//                 return;
//
//             var item = wordDictionary.Words.IndexOf(wordList.First());
//             wordDictionary.Words[item].Category = category;
//         }
//
//         public void ToggleIsWordInDictionary(string filename, string word, bool expectedNewStatus)
//         {
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//
//             var items = wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
//
//             if (!items.Any())
//                 return;
//
//             var item = items.First();
//
//             var index = wordDictionary.Words.IndexOf(item);
//
//             if (expectedNewStatus == false)
//             {
//                 wordDictionary.Words[index].Status = WordStatus.DoesNotExist;
//                 return;
//             }
//             
//             Console.WriteLine(JsonConvert.SerializeObject(wordDictionary.Words[index]));
//             wordDictionary.Words[index].Status = item.PermanentDefinition != null ? WordStatus.Permanent : WordStatus.Temporary;
//             Console.WriteLine(JsonConvert.SerializeObject(wordDictionary.Words[index]));
//         }
//
//         public void AddWordToGuessedWords(string dictionaryFilename, string guessedWordsFilename, string word)
//         {
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//
//             var items = wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
//             
//             var guessed = _guessedWords.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
//             
//             if (guessed.Any())
//             {
//                 var index = _guessedWords.Words.IndexOf(guessed.First());
//                 _guessedWords.Words[index] = new GuessedWord(word, items.Any() ? items.First().Status : WordStatus.DoesNotExist);
//             }
//             else
//             {
//                 _guessedWords.AddWord(word, items.Any() ? items.First().Status : WordStatus.DoesNotExist);
//             }
//
//         }
//         
//         public void UpdateDictionaryFile()
//         {
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//             _fileHelper.WriteFile(_filenameHelper.GetDictionaryFilename(), wordDictionary);
//         }
//
//         public void UpdateGuessedWordsFile()
//         {
//             _fileHelper.WriteFile(_filenameHelper.GetGuessedWordsFilename(), _guessedWords);
//         }
//
//         public WordDictionary GetDictionary()
//         {
//             var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
//             return wordDictionary;
//         }
//     }
// }
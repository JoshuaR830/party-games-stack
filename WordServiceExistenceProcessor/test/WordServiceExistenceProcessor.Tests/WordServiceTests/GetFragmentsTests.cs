using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using WordServiceExistenceProcessor.DynamoDB;
using WordServiceExistenceProcessor.Words.WordService;
using Xunit;

namespace WordServiceExistenceProcessor.Tests.WordServiceTests
{
    public class GetFragmentsTests
    {
        private readonly WordExistenceHelper _wordExistenceHelper;
        
        public GetFragmentsTests()
        {
            var dynamoDbWrapper = Substitute.For<IGetItemRequestWrapper>();
            var dynamoDbBatchWrapper = Substitute.For<IBatchGetItemRequestWrapper>();
            
            _wordExistenceHelper = new WordExistenceHelper(dynamoDbWrapper, dynamoDbBatchWrapper);
        }

        [Fact]
        public void WhenWordHasOneLetterEnding()
        {
            var word = "cheeses";
            var response = _wordExistenceHelper.GetWordFragments(word);

            var expected = new List<string> {word, "cheese", "chees"};
            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void WhenNotAWordButEndingInOneLetter()
        {
            var word = "reallynotawords";
            var response = _wordExistenceHelper.GetWordFragments(word);
            
            var expected = new List<string> {word, "reallynotaword"};
            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void WhenWordHasTwoLetterEnding()
        {
            var word = "boxes";
            var response = _wordExistenceHelper.GetWordFragments(word);

            var expected = new List<string> {word, "boxe", "box"};
            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void WhenNotAWordButEndingInTwoLetters()
        {
            var word = "reallynotawordes";
            var response = _wordExistenceHelper.GetWordFragments(word);
            
            var expected = new List<string> {word, "reallynotaworde", "reallynotaword"};
            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void WhenWordHasThreeLetterEnding()
        {
            var word = "kicking";
            var response = _wordExistenceHelper.GetWordFragments(word);
            
            var expected = new List<string> {word, "kick"};
            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void WhenNotAWordButEndingInThreeLetters()
        {
            var word = "reallynotawording";
            var response = _wordExistenceHelper.GetWordFragments(word);
            
            var expected = new List<string> {word, "reallynotaword"};
            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void WhenWordHasFourLetterEnding()
        {
            var word = "running";
            var response = _wordExistenceHelper.GetWordFragments(word);
            
            var expected = new List<string> {word, "runn", "run"};
            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void WhenNotAWordButEndingInfourLetters()
        {
            var word = "reallynotawordning";
            var response = _wordExistenceHelper.GetWordFragments(word);
            
            var expected = new List<string> {word, "reallynotawordn", "reallynotaword"};
            response.Should().BeEquivalentTo(expected);
        }
    }
}
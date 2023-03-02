using NUnit.Framework;
using System;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;


public class TestGetRandomLetterUseCase 
    {
        [Test]
        public void TestGetNewLetterUseCaseSimplePasses()
        {
            IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

            string actualResult = useCase.GetRandomLetter();

            int actualResultLength = actualResult.Length;

            int expectedLength = 1;

            Assert.AreEqual(expectedLength, actualResultLength);
        }

        [Test]
        public void test_get_new_letter_use_case_returns_letter()
        {
            IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

            string actualResult = useCase.GetRandomLetter();

            Assert.IsTrue(Char.IsLetter(actualResult[0]));
        }
    }


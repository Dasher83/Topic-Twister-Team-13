using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared;


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
        
    }


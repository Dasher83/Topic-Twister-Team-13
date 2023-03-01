using System.Collections;
using System.Collections.Generic;
using TopicTwister.NewRound.Shared.Interfaces;
using UnityEngine;


namespace TopicTwister.NewRound.Services
{
    public class LetterService : IShuffleLetterService
    {
        private IShuffleLetterUseCase _useCase;

        private LetterService(){}
        
        public LetterService(IShuffleLetterUseCase useCase)
        {
            _useCase = useCase;
        }
        
        public string GetRandomLetter()
        {
            string letter = _useCase.GetRandomLetter();
            return letter;
        }
    }
}

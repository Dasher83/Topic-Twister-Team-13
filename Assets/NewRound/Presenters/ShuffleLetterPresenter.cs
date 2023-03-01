using System.Collections;
using System.Collections.Generic;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using UnityEngine;

namespace TopicTwister.NewRound.Presenters
{
    public class ShuffleLetterPresenter : IShuffleLetterPresenter
    {
        private IShuffleLetterView _shuffleLetterView;
        private IAction _getRandomLetterAction;
        
        private ShuffleLetterPresenter(){}

        private ShuffleLetterPresenter(IShuffleLetterView shuffleLetterView)
        {
            _shuffleLetterView = shuffleLetterView;
            
        }
        
        
        public void GetRandomLetter()
        {
            throw new System.NotImplementedException();
        }

        public void ShowLetter(string letter)
        {
            _shuffleLetterView.ShowLetter(letter);
        }
    }
}

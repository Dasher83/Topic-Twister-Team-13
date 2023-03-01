using System.Collections;
using System.Collections.Generic;
using TopicTwister.Shared.Interfaces;
using UnityEngine;


namespace TopicTwister.NewRound.Actions
{
    public class GetShuffledLetterAction : IAction
    {
        private IShuffleLetterPresenter _shuffleLetterPresenter;
        
        public IShuffleLetterPresenter ShuffleLetterPresenter { set { _shuffleLetterPresenter = value; } }
        
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}

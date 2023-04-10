using System;
using TMPro;
using TopicTwister.NewRound.Shared.ScriptableObjects;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;


namespace TopicTwister.NewRound.Views
{
    public class BeginRoundView : MonoBehaviour
    {
        [SerializeField]
        private NewRoundScriptable _newRoundData;
        [SerializeField]
        private InitialLetterRevealedEventScriptable _InitialLetterRevealedEventContainer;
        [SerializeField]
        private TextMeshProUGUI _roundNumberDisplay;
        [SerializeField]
        private Transform _categoryListRoot;
        [SerializeField]
        private TextMeshProUGUI _initialLetter;
        [SerializeField]
        private LoadSceneEventScriptable _loadSceneEventContainer;

        private void Start()
        {
            _roundNumberDisplay.text = $"Ronda {_newRoundData.RoundNumber}";
            _InitialLetterRevealedEventContainer.InitialLetterRevealed += InitialLetterRevealedEventHandler;
        }

        private void InitialLetterRevealedEventHandler()
        {
            char initialLetter = _initialLetter.text[0];

            _newRoundData.Initialize(initialLetter);
            _loadSceneEventContainer.LoadSceneWithDelay?.Invoke(Scenes.PlayTurn, 2f);
        }
    }
}

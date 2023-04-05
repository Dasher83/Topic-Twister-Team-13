using System.Collections.Generic;
using System;
using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;


namespace TopicTwister.PlayTurn.Views
{
    public class PlayTurnView : MonoBehaviour
    {
        [SerializeField]
        private NewRoundScriptable _newRoundData;

        [SerializeField]
        private TextMeshProUGUI _roundNumber;
        [SerializeField]
        private Transform _categoryListRoot;
        [SerializeField]
        private TextMeshProUGUI _initialLetter;

        private void Start()
        {
            LoadRoundData();
        }

        private void LoadRoundData()
        {
            _roundNumber.text = $"{_roundNumber.text.Split(' ')[0]} {_newRoundData.RoundNumber}";
            _initialLetter.text = _newRoundData.InitialLetter.ToString();
            GameObject child;

            for(int i = 0; i < _categoryListRoot.childCount; i++)
            {
                child = _categoryListRoot.GetChild(i).Find("Category").gameObject;
                child.GetComponent<TextMeshProUGUI>().text = _newRoundData.Categories[i];
            }
        }
    }
}

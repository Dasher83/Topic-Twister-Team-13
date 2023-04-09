using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopicTwister.Shared.ScriptableObjects.FakeMatch;
using TMPro;
using TopicTwister.Shared.ScriptableObjects;

namespace TopicTwister.RoundResults
{
    public class LoadRoundResults : MonoBehaviour
    {
        [SerializeField]
        private FakeMatchScriptable _fakeMatchData;

        [SerializeField]
        private NewRoundScriptable _newRoundData;

        [SerializeField]
        private Transform _cateoryResultList;



        void Start()
        {
            int index = 0;
            foreach (Transform child in _cateoryResultList)
            {

                child.Find("Category").GetComponent<TextMeshProUGUI>().text = _newRoundData.Categories[index].Name;
                child.Find("AnswerPlayerOne").GetComponent<TextMeshProUGUI>().text = _fakeMatchData.Rounds[_newRoundData.RoundNumber -1].UserAnswer[index].Answer;
                child.Find("AnswerPlayerTwo").GetComponent<TextMeshProUGUI>().text = _fakeMatchData.Rounds[_newRoundData.RoundNumber -1].BotAnswer[index].Answer;
                index++;
            }
        }

        
        void Update()
        {
        
        }
    }
}

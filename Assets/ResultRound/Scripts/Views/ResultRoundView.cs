using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TopicTwister.ResultRound.Presenters;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.Structs;
using TopicTwister.ResultRound.Shared.Interfaces;


namespace TopicTwister.ResultRound.Views
{
    public class ResultRoundView : MonoBehaviour, IResultRoundView
    {
        public event Action OnLoad;

        [SerializeField]
        private Transform categoryResultList;

        [SerializeField]
        private RoundAnswersScriptable roundAnswer;

        private List<RoundAnswer> resultRoundViewList;
        private ResultRoundPresenter _resultRoundPresenter;
            
        void Start()
        {
            LoadCategoryResultList();
            _resultRoundPresenter = new ResultRoundPresenter(resultRoundView: this);
            OnLoad?.Invoke();
        }
        
        public void LoadCategoryResultList()
        {
           resultRoundViewList = roundAnswer.GetRoundAnswers();

            for(int i = 0; i < categoryResultList.childCount; i++)
            {
                categoryResultList.transform.GetChild(i).Find("Category").gameObject.GetComponent<TextMeshProUGUI>().text = resultRoundViewList[i].CategoryId;
                categoryResultList.transform.GetChild(i).Find("Answer").gameObject.GetComponent<TextMeshProUGUI>().text = resultRoundViewList[i].UserInput;

            }
        }
    }
}

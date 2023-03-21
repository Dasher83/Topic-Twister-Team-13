using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.Structs;

namespace TopicTwister.ResultRound.Views
{
    public class ResultRoundView : MonoBehaviour, IResultRoundView
    {

        [SerializeField]
        private Transform categoryResultList;

        [SerializeField]
        private RoundAnswersScriptable roundAnswer;

        private List<RoundAnswer> resultRoundViewList;


        void Start()
        {
            LoadCategoryResultList();
        }

        void Update()
        {
        
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

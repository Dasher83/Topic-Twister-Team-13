using System;
using TopicTwister.Home.Presenters;
using TopicTwister.Home.Scripts.Shared.Interfaces;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.ScriptableObjects.FakeMatch;
using UnityEngine;
using UnityEngine.UI;


namespace TopicTwister.Home.Views
{
    public class NewBotMatchView : MonoBehaviour, INewBotMatchView
    {
        public event Action StartMatchVersusBot;

        [SerializeField]
        private LoadSceneEventScriptable _eventContainer;

        [SerializeField]
        private NewRoundScriptable _newRoundData;

        [SerializeField]
        private FakeMatchScriptable _fakeMatchScriptable;

        private INewBotMatchPresenter _presenter;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(StartMatchWithBot);
            _presenter = new NewBotMatchPresenter(view: this);
        }

        public void StartMatchWithBot()
        {
            _fakeMatchScriptable.Initialize();
            _newRoundData.Initialize();
            StartMatchVersusBot?.Invoke();
        }
    }
}
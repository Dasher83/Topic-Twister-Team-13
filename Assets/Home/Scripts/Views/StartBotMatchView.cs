using System;
using TopicTwister.Home.Presenters;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.ScriptableObjects.FakeMatch;
using UnityEngine;
using UnityEngine.UI;


namespace TopicTwister.Home.Views
{
    public class StartBotMatchView : MonoBehaviour, IStartBotMatchView
    {
        public event Action StartMatchVersusBot;

        [SerializeField]
        private LoadSceneEventScriptable _loadSceneEventContainer;

        [SerializeField]
        private FakeMatchScriptable _fakeMatchData;

        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        private IStartBotMatchPresenter _presenter;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(StartMatchWithBot);
            _presenter = new StartBotMatchPresenter(view: this);
        }

        public void StartMatchWithBot()
        {
            _fakeMatchData.Initialize();
            StartMatchVersusBot?.Invoke();
            _loadSceneEventContainer.LoadSceneWithoutDelay(Scenes.BeginRoundScene);
            GetComponent<Button>().onClick.RemoveListener(StartMatchWithBot);
        }

        public void ReceiveUpdate(MatchDto matchDto)
        {
            _matchCacheData.MatchDto = matchDto;
        }
    }
}

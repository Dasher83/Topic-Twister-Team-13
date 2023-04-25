using System;
using TopicTwister.Home.Presenters;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.Constants;
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
        private LoadSceneEventScriptable _loadSceneEventContainer;

        [SerializeField]
        private RoundCacheScriptable _newRoundData;

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
            StartMatchVersusBot?.Invoke();
            _loadSceneEventContainer.LoadSceneWithoutDelay(Scenes.BeginRoundScene);
        }
    }
}

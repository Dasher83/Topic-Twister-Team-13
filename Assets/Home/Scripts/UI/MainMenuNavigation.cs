using TopicTwister.Shared.Constants;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.ScriptableObjects.FakeMatch;
using UnityEngine;
using UnityEngine.UI;


namespace TopicTwister.Home.UI
{
    public class MainMenuNavigation : MonoBehaviour
    {
        [SerializeField]
        private LoadSceneEventScriptable _eventContainer;

        [SerializeField]
        private NewRoundScriptable _newRoundData;

        [SerializeField]
        private FakeMatchScriptable _fakeMatchScriptable;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(StartMatchWithBot);
        }

        public void StartMatchWithBot()
        {
            _newRoundData.Initialize();
            _fakeMatchScriptable.Initialize();
            _eventContainer.LoadSceneWithoutDelay?.Invoke(Scenes.BeginRoundScene);
        }
    }
}

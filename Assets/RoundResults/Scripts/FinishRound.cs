using TopicTwister.Shared.Constants;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


public class FinishRound : MonoBehaviour
{
    [SerializeField]
    private RoundEndedEventScriptable _roundEndedEventContainer;

    [SerializeField]
    private LoadSceneEventScriptable _loadSceneEventContainer;

    [SerializeField]
    private NewRoundScriptable _newRoundData;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickEventHandler);
    }

    public void OnClickEventHandler()
    {
        if(_newRoundData.RoundNumber < 3)
        {
            _roundEndedEventContainer.RoundEnded?.Invoke();
            _loadSceneEventContainer.LoadSceneWithoutDelay?.Invoke(Scenes.BeginRoundScene);
        }
        else
        {
            _loadSceneEventContainer.LoadSceneWithoutDelay?.Invoke(Scenes.Home);
        }
        GetComponent<Button>().onClick.RemoveListener(OnClickEventHandler);
    }
}

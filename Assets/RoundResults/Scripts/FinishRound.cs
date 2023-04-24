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
    private RoundCacheScriptable _roundCache;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickEventHandler);
    }

    public void OnClickEventHandler()
    {
        if(_roundCache.RoundDto.RoundNumber < 3)
        {
            Debug.Log("Aumenté la ronda");
            _roundEndedEventContainer.RoundEnded?.Invoke();
            _loadSceneEventContainer.LoadSceneWithoutDelay?.Invoke(Scenes.BeginRoundScene);
        }
        else
        {
            _loadSceneEventContainer.LoadSceneWithoutDelay?.Invoke(Scenes.MatchResult);
        }
        GetComponent<Button>().onClick.RemoveListener(OnClickEventHandler);
    }
}

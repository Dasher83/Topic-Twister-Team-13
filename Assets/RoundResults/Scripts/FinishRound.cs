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
    private MatchCacheScriptable _matchCacheData;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickEventHandler);
    }

    public void OnClickEventHandler()
    {
        if(_matchCacheData.RoundWithCategoriesDto.RoundDto.RoundNumber < 2)
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

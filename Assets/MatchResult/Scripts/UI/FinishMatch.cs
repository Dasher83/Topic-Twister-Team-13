using TopicTwister.Shared.Constants;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


public class FinishMatch : MonoBehaviour
{
    [SerializeField]
    private LoadSceneEventScriptable _loadSceneEventContainer;
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickEventHandler);
    }

    public void OnClickEventHandler()
    {
        _loadSceneEventContainer.LoadSceneWithoutDelay?.Invoke(Scenes.Home);
    }
}

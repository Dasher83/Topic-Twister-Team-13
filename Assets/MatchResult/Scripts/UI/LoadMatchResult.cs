using TMPro;
using TopicTwister.MatchResult.Shared.ScriptableObjects;
using TopicTwister.Shared.ScriptableObjects.FakeMatch;
using UnityEngine;
using UnityEngine.UI;


namespace TopicTwister.MatchResult.UI
{
    public class LoadMatchResult : MonoBehaviour
    {
        [SerializeField]
        private FakeMatchScriptable _matchData;

        [SerializeField]
        private MatchImageResultScriptable _matchImageReferences;

        [SerializeField]
        private Transform _header;

        [SerializeField]
        private Image _resultMatchImage; 

        private void Start()
        {
            _header.Find("PlayerOne").Find("Points")
                .GetComponentInChildren<TextMeshProUGUI>().text = _matchData.UserPoints.ToString();

            _header.Find("PlayerTwo").Find("Points")
                .GetComponentInChildren<TextMeshProUGUI>().text = _matchData.BotPoints.ToString();

            if(_matchData.UserPoints > _matchData.BotPoints)
            {
                _resultMatchImage.sprite = _matchImageReferences.wonMatch;
            }
            else if(_matchData.BotPoints > _matchData.UserPoints)
            {
                _resultMatchImage.sprite = _matchImageReferences.lostMatch;
            }
            else
            {
                _resultMatchImage.sprite = _matchImageReferences.drawnMatch;
            }
        }
    }
}

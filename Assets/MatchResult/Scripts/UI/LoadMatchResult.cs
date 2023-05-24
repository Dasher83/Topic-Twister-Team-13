using TMPro;
using TopicTwister.MatchResult.Shared.ScriptableObjects;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


namespace TopicTwister.MatchResult.UI
{
    public class LoadMatchResult : MonoBehaviour
    {
        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        [SerializeField]
        private MatchImageResultScriptable _matchImageReferences;

        [SerializeField]
        private Transform _header;

        [SerializeField]
        private Image _resultMatchImage; 

        private void Start()
        {
            _header.Find("PlayerOne").Find("Points")
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = _matchCacheData.EndOfTurnDto.UserWithIniciativeMatchDto.Score.ToString();

            _header.Find("PlayerTwo").Find("Points")
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = _matchCacheData.EndOfTurnDto.UserWithoutIniciativeMatchDto.Score.ToString();

            bool userWithInitiativeWon = _matchCacheData.EndOfTurnDto.UserWithIniciativeMatchDto.IsWinner;
            bool userWithoutInitiativeWon = _matchCacheData.EndOfTurnDto.UserWithoutIniciativeMatchDto.IsWinner;

            if (userWithInitiativeWon && userWithoutInitiativeWon == false)
            {
                _resultMatchImage.sprite = _matchImageReferences.wonMatch;
            }
            else if(userWithoutInitiativeWon && userWithInitiativeWon == false)
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

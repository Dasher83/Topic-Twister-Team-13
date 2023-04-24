using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using TopicTwister.PlayTurn.Shared.DTOs;


namespace TopicTwister.PlayTurn.Views
{
    public class PlayTurnView : MonoBehaviour
    {
        [SerializeField]
        private RoundCacheScriptable _roundCache;

        [SerializeField]
        private TurnAnswersDraftScriptable _turnAnswersDraftData;

        [SerializeField]
        private TextMeshProUGUI _roundNumber;
        [SerializeField]
        private Transform _categoryListRoot;
        [SerializeField]
        private TextMeshProUGUI _initialLetter;

        private void Start()
        {
            LoadRoundData();
        }

        private void LoadRoundData()
        {
            TurnAnswerDraftDTO[] turnAnswerDrafts = new TurnAnswerDraftDTO[_categoryListRoot.childCount];
            _roundNumber.text = $"{_roundNumber.text.Split(' ')[0]} {_roundCache.RoundDto.RoundNumber}";
            _initialLetter.text = _roundCache.RoundDto.InitialLetter.ToString();
            GameObject child;

            for(int i = 0; i < _categoryListRoot.childCount; i++)
            {
                child = _categoryListRoot.GetChild(i).Find("Category").gameObject;
                child.GetComponent<TextMeshProUGUI>().text = _roundCache.Categories[i].Name;
                turnAnswerDrafts[i] = new TurnAnswerDraftDTO(
                    category: _roundCache.Categories[i],
                    order: i);
            }
            _turnAnswersDraftData.Initialize(turnAnswerDrafts);
        }
    }
}

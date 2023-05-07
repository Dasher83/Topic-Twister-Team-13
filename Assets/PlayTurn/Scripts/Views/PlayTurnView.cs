using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Constants;
using TopicTwister.PlayTurn.Presenters;


namespace TopicTwister.PlayTurn.Views
{
    public class PlayTurnView : MonoBehaviour, IPlayTurnView
    {
        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        [SerializeField]
        private TurnAnswersScriptable _turnAnswersData;

        [SerializeField]
        private TextMeshProUGUI _roundNumber;

        [SerializeField]
        private Transform _categoryListRoot;

        [SerializeField]
        private TextMeshProUGUI _initialLetter;

        [SerializeField]
        private TimeOutEventScriptable _timeOutEventContainer;

        [SerializeField]
        private InterruptTurnEventScriptable _interruptTurnEventContainer;

        [SerializeField]
        private LoadSceneEventScriptable _loadSceneEventContainer;

        public event EventDelegates.PlayTurnView.LoadEventHandler OnLoad;

        private void Start()
        {
            new StartTurnPresenter(this);
            LoadRoundData();
            _timeOutEventContainer.TimeOut += CaptureAndSaveDataEventHandler;
            _interruptTurnEventContainer.InterruptTurn += CaptureAndSaveDataEventHandler;
            OnLoad?.Invoke(
                userId: Configuration.TestUserId,
                roundId: _matchCacheData.RoundWithCategoriesDto.RoundDto.Id);
        }

        private void LoadRoundData()
        {
            TurnAnswerDraftDto[] turnAnswerDrafts = new TurnAnswerDraftDto[_categoryListRoot.childCount];
            _roundNumber.text = $"Ronda {_matchCacheData.RoundWithCategoriesDto.RoundDto.RoundNumber + 1}";
            _initialLetter.text = _matchCacheData.RoundWithCategoriesDto.RoundDto.InitialLetter.ToString();
            GameObject child;

            for(int i = 0; i < _categoryListRoot.childCount; i++)
            {
                child = _categoryListRoot.GetChild(i).Find("Category").gameObject;
                child.GetComponent<TextMeshProUGUI>().text = _matchCacheData.RoundWithCategoriesDto.CategoryDtos[i].Name;
                turnAnswerDrafts[i] = new TurnAnswerDraftDto(
                    category: _matchCacheData.RoundWithCategoriesDto.CategoryDtos[i],
                    order: i);
            }
            _matchCacheData.Initialize(turnAnswerDrafts);
        }

        private void CaptureAndSaveDataEventHandler()
        {
            _turnAnswersData.ClearAnswers();

            TurnAnswerDto[] turnAnswers = new TurnAnswerDto[5];
            int index = 0;

            foreach (Transform childTransform in _categoryListRoot)
            {
                string userInput = childTransform.Find("UserInput").gameObject.GetComponent<TextMeshProUGUI>().text.Trim();
                turnAnswers[index] = new TurnAnswerDto(
                    category: _matchCacheData.RoundWithCategoriesDto.CategoryDtos[index],
                    userInput: userInput,
                    order: index);
                index++;
            }

            _turnAnswersData.AddAnswers(turnAnswers);
            _loadSceneEventContainer.LoadSceneWithDelay(
                Configuration.Scenes.TurnResultScene,
                Configuration.TransitionsDuration.FromPlayTurnToTurnResult);
        }

        public void ReceiveUpdate(TurnDto turn)
        {
            throw new System.NotImplementedException();
        }
    }
}

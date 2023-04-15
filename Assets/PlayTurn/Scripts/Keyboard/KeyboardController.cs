using TMPro;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using UnityEngine;


namespace TopicTwister.PlayTurn.Keyboard
{
    public class KeyboardController : MonoBehaviour
    {
        [SerializeField] private TimeOutEventScriptable _timeOutEventContainer;
        [SerializeField] private InterruptTurnEventScriptable _interruptTurnEventContainer;
        [SerializeField] private TurnAnswersDraftScriptable _turnAnswersDraftData;
        [SerializeField] private UserInputPressedEventScriptable _userInputPressedEventContainer;

        private int _currentIndex;

        private bool _blockKeyboard;

        private void CurrentIndex(int index)
        {
            _currentIndex = index;
        }

        public void Start()
        {
            _currentIndex = 0;
            _blockKeyboard = false;
            _timeOutEventContainer.TimeOut += InputEndEventHandler;
            _interruptTurnEventContainer.InterruptTurn += InputEndEventHandler;
            _userInputPressedEventContainer.OnInputPressed += CurrentIndex;
        }

        public void AddLetter(string letter)
        {
            if (_blockKeyboard) return;
            _turnAnswersDraftData.AddUserInput(userInput: letter, index: _currentIndex);
        }

        public void EreaseLetter()
        {
            if (_blockKeyboard) return;
            _turnAnswersDraftData.RemoveUserInput(_currentIndex);
        }

        public void AddSpace()
        {
            if (_blockKeyboard) return;
            _turnAnswersDraftData.AddUserInput(userInput: " ", index: _currentIndex);
        }

        private void InputEndEventHandler()
        {
            _blockKeyboard = true;
        }
    }
}

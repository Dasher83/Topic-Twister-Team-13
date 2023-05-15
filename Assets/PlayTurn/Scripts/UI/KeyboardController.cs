using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;
using TMPro;


namespace TopicTwister.PlayTurn.UI
{
    public class KeyboardController : MonoBehaviour
    {
        [SerializeField] private TimeOutEventScriptable _timeOutEventContainer;
        [SerializeField] private InterruptTurnEventScriptable _interruptTurnEventContainer;
        [SerializeField] private MatchCacheScriptable _matchCacheData;
        [SerializeField] private UserInputPressedEventScriptable _userInputPressedEventContainer;
        [SerializeField] private ChangeKeyboardModeEventScriptable _changeKeyBoardMode;
        private int _currentIndex;

        private bool _numericMode;
        [SerializeField] private GameObject[] _rowsAlpha;
        [SerializeField] private GameObject _rowNumeric;
        [SerializeField] private TextMeshProUGUI _alphaNumericText;
        

        private bool _blockKeyboard;

        private void CurrentIndex(int index)
        {
            _currentIndex = index;
        }

        public void Start()
        {
            _currentIndex = 0;
            _numericMode = false;
            _rowNumeric.SetActive(false);
            _blockKeyboard = false;
            _timeOutEventContainer.TimeOut += InputEndEventHandler;
            _interruptTurnEventContainer.InterruptTurn += InputEndEventHandler;
            _userInputPressedEventContainer.OnInputPressed += CurrentIndex;
            _changeKeyBoardMode.ChangeKeyboardMode += ChangeMode;
        }

        public void ChangeMode()
        {
            _numericMode = !_numericMode;

            _rowNumeric.SetActive(_numericMode);

            foreach (var row in _rowsAlpha)
            {
                row.SetActive(!_numericMode);
            }

            if (_numericMode)
            {
                _alphaNumericText.text = "abc..";
            }
            else
            {
                _alphaNumericText.text = "123..";    
            }

        }

        public void AddLetter(string letter)
        {
            if (_blockKeyboard) return;
            _matchCacheData.AddUserInput(userInput: letter, index: _currentIndex);
        }

        public void EreaseLetter()
        {
            if (_blockKeyboard) return;
            _matchCacheData.RemoveUserInput(_currentIndex);
        }

        public void AddSpace()
        {
            if (_blockKeyboard) return;
            _matchCacheData.AddUserInput(userInput: " ", index: _currentIndex);
        }

        private void InputEndEventHandler()
        {
            _blockKeyboard = true;
        }
    }
}

using UnityEngine;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;


namespace TopicTwister.PlayTurn.Commands
{
    public class FocusInputCommand : MonoBehaviour
    {
        [SerializeField] private UserInputPressedEventScriptable _eventContainer;

        public void Execute()
        {
            _eventContainer.OnInputPressed?.Invoke(gameObject.transform.GetSiblingIndex());
        }
    }
}

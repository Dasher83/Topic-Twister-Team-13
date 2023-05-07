using UnityEngine;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;


namespace TopicTwister.PlayTurn.UI
{
    public class FocusInput : MonoBehaviour
    {
        [SerializeField] private UserInputPressedEventScriptable _eventContainer;

        public void Execute()
        {
            _eventContainer.OnInputPressed?.Invoke(gameObject.transform.GetSiblingIndex());
        }
    }
}

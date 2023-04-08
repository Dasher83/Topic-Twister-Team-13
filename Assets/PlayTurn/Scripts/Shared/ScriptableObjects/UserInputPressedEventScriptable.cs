using System;
using UnityEngine;


namespace TopicTwister.PlayTurn.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "UserInputPressedEvent", menuName = "ScriptableObjects/Events/UserInputPressedEvent")]
    public class UserInputPressedEventScriptable : ScriptableObject
    {
        public Action<int> OnInputPressed;
    }
}

using System;
using UnityEngine;


namespace TopicTwister.PlayTurn.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChangeKeyboardModeEvent", menuName = "ScriptableObjects/Events/ChangeKeyboardModeEvent")]
    public class ChangeKeyboardModeEventScriptable : ScriptableObject
    {
        public Action ChangeKeyboardMode;
    }
}

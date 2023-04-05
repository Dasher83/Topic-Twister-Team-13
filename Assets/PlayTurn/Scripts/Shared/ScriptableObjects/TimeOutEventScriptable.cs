using System;
using UnityEngine;


namespace TopicTwister.PlayTurn.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TimeOutEvent", menuName = "ScriptableObjects/Events/TimeOutEvent")]
    public class TimeOutEventScriptable : ScriptableObject
    {
        public Action TimeOut;
    }
}

using System;
using UnityEngine;


namespace TopicTwister.PlayTurn.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InterruptTurnEvent", menuName = "ScriptableObjects/Events/InterruptTurnEvent")]
    public class InterruptTurnEventScriptable : ScriptableObject
    {
        public Action InterruptTurn;
    }
}

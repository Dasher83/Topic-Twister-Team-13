using System;
using UnityEngine;


namespace TopicTwister.PlayTurn.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InterruptTurnEvent", menuName = "ScriptableObjects/Events/InterruptTurnEvent")]
    public class InterruptTurnEventScriptable : ScriptableObject
    {
        public bool isSubscribedTimer = false;
        public bool isSubscribedPlayTurn = false;
        public Action InterruptTurn;
    }
}

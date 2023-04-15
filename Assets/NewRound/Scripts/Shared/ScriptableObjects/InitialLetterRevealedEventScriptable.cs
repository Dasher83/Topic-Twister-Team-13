using System;
using UnityEngine;


namespace TopicTwister.NewRound.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InitialLetterRevealedEvent", menuName = "ScriptableObjects/Events/InitialLetterRevealedEvent")]
    public class InitialLetterRevealedEventScriptable : ScriptableObject
    {
        public Action InitialLetterRevealed;
    }
}

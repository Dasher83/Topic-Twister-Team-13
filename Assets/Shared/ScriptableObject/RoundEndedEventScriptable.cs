using System;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoundEndedEvent" , menuName = "ScriptableObjects/Events/RoundEndedEvent")]
    public class RoundEndedEventScriptable : ScriptableObject
    {
        public Action RoundEnded; 
    }
}


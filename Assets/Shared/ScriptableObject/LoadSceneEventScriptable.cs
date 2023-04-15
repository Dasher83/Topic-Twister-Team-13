using System;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LoadSceneEvent", menuName = "ScriptableObjects/Events/LoadSceneEvent")]
    public class LoadSceneEventScriptable : ScriptableObject
    {
        public Action<string> LoadSceneWithoutDelay;
        public Action<string, float> LoadSceneWithDelay;
    }
}

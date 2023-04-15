using UnityEngine;


namespace TopicTwister.MatchResult.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MatchImageResultReferences", menuName = "ScriptableObjects/MatchImageResultReferences")]
    public class MatchImageResultScriptable : ScriptableObject
    {
        public Sprite wonMatch;
        public Sprite lostMatch;
        public Sprite drawnMatch;
    }
}

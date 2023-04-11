using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AnswerImageResultReferences", menuName = "ScriptableObjects/AnswerImageResultReferences")]
    public class AnswerImageResultScriptable : ScriptableObject
    {
        public Sprite correctAnswer;
        public Sprite incorrectAnswer;
    }
}

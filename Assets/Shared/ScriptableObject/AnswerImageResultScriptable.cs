using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AnswerImageResultReferences", menuName = "ScriptableObjects/AnswerImageResultReferences", order = 2)]
    public class AnswerImageResultScriptable : ScriptableObject
    {
        public Sprite correctAnswer;
        public Sprite incorrectAnswer;
    }
}

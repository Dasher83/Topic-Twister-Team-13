using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MatchCacheData", menuName = "ScriptableObjects/MatchCacheDate")]
    public class MatchCacheScriptable : ScriptableObject
    {
        private MatchDto _matchDto;
        private RoundWithCategoriesDto _roundWithCategoriesDto;

        public MatchDto MatchDto
        {
            get {  return _matchDto; }
            set { _matchDto = value; }
        }

        public RoundWithCategoriesDto RoundWithCategoriesDto
        {
            get { return _roundWithCategoriesDto; }
            set { _roundWithCategoriesDto = value; }
        }
    }
}

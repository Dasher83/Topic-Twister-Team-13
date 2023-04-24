using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoundCache", menuName = "ScriptableObjects/RoundCache")]
    public class RoundCacheScriptable : ScriptableObject
    {
        [SerializeField]
        private RoundDto _roundDto;

        [SerializeField]
        private List<CategoryDTO> _categoryDtos;

        public RoundDto RoundDto => _roundDto;

        public List<CategoryDTO> Categories => _categoryDtos.ToList();

        public void Initialize(RoundDto roundDto, List<CategoryDTO> categoryDtos)
        {
            _roundDto = roundDto;
            _categoryDtos = categoryDtos.ToList();
        }
    }
}

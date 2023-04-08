using System.Collections.Generic;
using System.Linq;
using TopicTwister.PlayTurn.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.PlayTurn.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TurnAnswersDraftData", menuName = "ScriptableObjects/TurnAnswersDraftData")]
    public class TurnAnswersDraftScriptable : ScriptableObject
    {
        [SerializeField] private List<TurnAnswerDraftDTO> _turnAnswerDrafts;

        public void Initialize(TurnAnswerDraftDTO[] turnAnswerDrafts)
        {
            _turnAnswerDrafts = turnAnswerDrafts.ToList();
        }
    }
}

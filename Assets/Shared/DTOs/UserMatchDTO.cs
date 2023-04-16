using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class UserMatchDTO
    {
        [SerializeField]
        private int _score;
        [SerializeField]
        private bool _isWinner;
        [SerializeField]
        private bool _hasInitiative;

        [SerializeField]
        private int _userId;
        [SerializeField]
        private int _matchId;

        public UserMatchDTO(int score, bool isWinner, bool hasInitiative, int userId, int matchId)
        {
            _score = score;
            _isWinner = isWinner;
            _hasInitiative = hasInitiative;
            _userId = userId;
            _matchId = matchId;
        }
    }
}

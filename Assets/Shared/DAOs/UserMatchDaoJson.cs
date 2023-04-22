﻿using System;
using UnityEngine;


namespace TopicTwister.Shared.DAOs
{
    [Serializable]
    public class UserMatchDaoJson
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

        public int Score => _score;
        public bool IsWinner => _isWinner;
        public bool HasInitiative => _hasInitiative;
        public int UserId => _userId;
        public int MatchId => _matchId;

        public UserMatchDaoJson(int score, bool isWinner, bool hasInitiative, int userId, int matchId)
        {
            _score = score;
            _isWinner = isWinner;
            _hasInitiative = hasInitiative;
            _userId = userId;
            _matchId = matchId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            UserMatchDaoJson other = (UserMatchDaoJson)obj;

            return _userId == other._userId &&
                _matchId == other._matchId &&
                _score == other._score &&
                _isWinner == other._isWinner &&
                _hasInitiative == other._hasInitiative;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_userId, _matchId, _score, _isWinner, _hasInitiative);
        }

    }
}
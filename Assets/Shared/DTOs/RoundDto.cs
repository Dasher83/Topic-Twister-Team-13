using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class RoundDto
    {
        [SerializeField] private int _id;

        [SerializeField] private int _roundNumber;

        [SerializeField] private char _initialLetter;

        [SerializeField] private bool _isActive;

        [SerializeField] private int _matchId;

        public int Id => _id;
        public int RoundNumber => _roundNumber;
        public char InitialLetter => _initialLetter;
        public bool IsActive => _isActive;
        public int MatchId => _matchId;

        public RoundDto(int roundNumber, char initialLetter, bool isActive, int matchId)
        {
            _roundNumber = roundNumber;
            _initialLetter = initialLetter;
            _isActive = isActive;
            _matchId = matchId;
        }

        public RoundDto(int id, int roundNumber, char initialLetter, bool isActive, int matchId) : this(roundNumber, initialLetter, isActive, matchId)
        {
            _id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            RoundDto other = (RoundDto)obj;

            bool idEquals = _id == other._id;
            bool roundNumberEquals = _roundNumber == other._roundNumber;
            bool initialLetterEquals = _initialLetter == other._initialLetter;
            bool isActiveEquals = _isActive == other._isActive;
            bool isMatchIdEquals = _matchId == other._matchId;

            return idEquals && roundNumberEquals && initialLetterEquals && isActiveEquals && isMatchIdEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _roundNumber, _initialLetter, _isActive);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TopicTwister.Shared.DAOs
{
    [Serializable]
    public class RoundDaoJson
    {
        [SerializeField] private int _id;
        [SerializeField] private int _roundNumber;
        [SerializeField] private char _initialLetter;
        [SerializeField] private bool _isActive;
        [SerializeField] private int _matchId;
        [SerializeField] private List<int> _categoryIds;

        public int Id => _id;
        public int RoundNumber => _roundNumber;
        public char InitialLetter => _initialLetter;
        public bool IsActive => _isActive;
        public int MatchId => _matchId;
        public List<int> CategoryIds => _categoryIds.ToList();

        public RoundDaoJson(int roundNumber, char initialLetter, bool isActive, int matchId, List<int> categoryIds)
        {
            _roundNumber = roundNumber;
            _initialLetter = initialLetter;
            _isActive = isActive;
            _matchId = matchId;
            _categoryIds = categoryIds;
        }

        public RoundDaoJson(int id, int roundNumber, char initialLetter, bool isActive, int matchId, List<int> categoryIds) :
            this(roundNumber, initialLetter, isActive, matchId, categoryIds)
        {
            _id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            RoundDaoJson other = (RoundDaoJson)obj;

            bool idEquals = _id == other._id;
            bool roundNumberEquals = _roundNumber == other._roundNumber;
            bool initialLetterEquals = _initialLetter == other._initialLetter;
            bool isActiveEquals = _isActive == other._isActive;
            bool isMatchIdEquals = _matchId == other._matchId;
            bool areCategoryIdsEquals = Enumerable.SequenceEqual(_categoryIds, other._categoryIds);

            return idEquals &&
                roundNumberEquals &&
                initialLetterEquals &&
                isActiveEquals &&
                isMatchIdEquals &&
                areCategoryIdsEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _roundNumber, _initialLetter, _isActive, _matchId, _categoryIds);
        }
    }
}

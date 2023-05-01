using System;
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

        public int Id => _id;
        public int RoundNumber => _roundNumber;
        public char InitialLetter => _initialLetter;
        public bool IsActive => _isActive;

        public RoundDaoJson(int roundNumber, char initialLetter, bool isActive)
        {
            _roundNumber = roundNumber;
            _initialLetter = initialLetter;
            _isActive = isActive;
        }

        public RoundDaoJson(int id, int roundNumber, char initialLetter, bool isActive) : this(roundNumber, initialLetter, isActive)
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

            return idEquals && roundNumberEquals && initialLetterEquals && isActiveEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _roundNumber, _initialLetter, _isActive);
        }
    }
}
using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class RoundDto
    {
        [SerializeField] private string _id;

        [SerializeField] private int _roundNumber;

        [SerializeField] private char _initialLetter;

        [SerializeField] private bool _isActive;

        public int RoundNumber => _roundNumber;
        public char InitialLetter => _initialLetter;

        public RoundDto(int roundNumber, char initialLetter, bool isActive)
        {
            _roundNumber = roundNumber;
            _initialLetter = initialLetter;
            _isActive = isActive;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace TopicTwister.NewRound.Models
{
    public class Round
    {
        private int _id;
        private int _roundNumber;
        private char _initialLetter;
        private bool _isActive;
        private List<Category> _categories;

        public int Id => _id;
        public int RoundNumber => _roundNumber;
        public char InitialLetter => _initialLetter;
        public bool IsActive => _isActive;
        public List<Category> Categories => _categories.ToList();

        public Round(int roundNumber, char initialLetter, bool isActive, List<Category> categories)
        {
            _roundNumber = roundNumber;
            _initialLetter = initialLetter;
            _isActive = isActive;
            _categories = categories;
        }

        public Round(int id, int roundNumber, char initialLetter, bool isActive, List<Category> categories) :
            this(roundNumber, initialLetter, isActive, categories)
        {
            _id = id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Repositories
{
    public class LetterReadOnlyRepositoryInMemory : ILetterReadOnlyRepository
    {
        private List<char> _readCache;
        private Random _random;

        public LetterReadOnlyRepositoryInMemory()
        {
            _readCache = new List<char>()
            {
                'a', 'b', 'c', 'd', 'e',
                'f', 'g', 'h', 'i', 'j',
                'k', 'l', 'm', 'n', 'ñ',
                'o', 'p', 'q', 'r', 's',
                't', 'u', 'v', 'w', 'x',
                'y', 'z'
            };

            _readCache.Select(letter => letter.ToString().ToLower()[0]).ToList();
            _random = new Random();
        }

        public Operation<char> GetRandomLetter()
        {
            char randomLetter = _readCache.ToList().OrderBy(_ => _random.Next()).First();
            return Operation<char>.Success(result: randomLetter);
        }
    }
}

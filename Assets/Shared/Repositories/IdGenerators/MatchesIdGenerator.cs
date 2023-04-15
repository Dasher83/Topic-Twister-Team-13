using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Shared.Repositories.IdGenerators
{
    public class MatchesIdGenerator: IUniqueIdGenerator
    {
        private int _currentId;
        private IMatchesRepository _matchesRepository;
        private List<MatchDTO> _matches;

        public MatchesIdGenerator(IMatchesRepository matchesRepository)
        {
            _matchesRepository = matchesRepository;
        }

        public int GetNextId()
        {
            UpdateCurrentId();
            return Interlocked.Increment(ref _currentId);
        }

        private void UpdateCurrentId()
        {
            _matches = _matchesRepository.GetAll();
            if (_matches != null && _matches.Any())
            {
                _currentId = _matches.OrderByDescending(match => match.Id).First().Id + 1;
            }
            else
            {
                _currentId = 0;
            }
        }
    }
}
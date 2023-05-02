using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.NewRound.Repositories.IdGenerators
{
    public class RoundsIdGenerator : IUniqueIdGenerator
    {
        private int _currentId;
        private IRoundsReadOnlyRepository _roundsRepository;
        private List<Round> _rounds;

        public RoundsIdGenerator(IRoundsReadOnlyRepository roundsRepository)
        {
            _roundsRepository = roundsRepository;
        }

        public int GetNextId()
        {
            UpdateCurrentId();
            return Interlocked.Increment(ref _currentId);
        }

        private void UpdateCurrentId()
        {
            _rounds = _roundsRepository.GetAll().Outcome;
            if (_rounds != null && _rounds.Any())
            {
                _currentId = _rounds.OrderByDescending(round => round.Id).First().Id + 1;
            }
            else
            {
                _currentId = 0;
            }
        }
    }
}
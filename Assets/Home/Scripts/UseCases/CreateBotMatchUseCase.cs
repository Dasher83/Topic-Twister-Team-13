using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;


namespace TopicTwister.Home.UseCases
{
    public class CreateBotMatchUseCase : ICreateBotMatchUseCase
    {
        private IMatchesRepository _matchesRepository;
        private IUserMatchesRepository _userMatchesRepository;
        private const int BotId = 2;
        private MatchMapper _mapper;

        public CreateBotMatchUseCase(IMatchesRepository matchesRepository, IUserMatchesRepository userMatchesRepository)
        {
            _matchesRepository = matchesRepository;
            _userMatchesRepository = userMatchesRepository;
            _mapper = new MatchMapper();
        }

        public MatchDTO Create(int userId)
        {
            Match match = new Match();
            match = _matchesRepository.Persist(match);
            _userMatchesRepository.Create(userId: userId, matchId: match.Id, hasInitiative: true);
            _userMatchesRepository.Create(userId: BotId, matchId: match.Id, hasInitiative: false);
            return _mapper.ToDTO(match);
        }
    }
}

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

            UserMatch userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: true,
                userId: userId,
                match: match);

            _userMatchesRepository.Persist(userMatch);

            userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: false,
                userId: BotId,
                match: match);

            _userMatchesRepository.Persist(userMatch);

            return _mapper.ToDTO(match);
        }
    }
}

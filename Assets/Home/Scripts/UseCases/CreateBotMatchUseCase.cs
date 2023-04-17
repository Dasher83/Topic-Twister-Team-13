using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Home.UseCases
{
    public class CreateBotMatchUseCase : ICreateBotMatchUseCase
    {
        private IMatchesRepository _matchesRepository;
        private IUserMatchesRepository _userMatchesRepository;
        private const int BotId = 2;

        public CreateBotMatchUseCase(IMatchesRepository matchesRepository, IUserMatchesRepository userMatchesRepository)
        {
            _matchesRepository = matchesRepository;
            _userMatchesRepository = userMatchesRepository;
        }

        public MatchDTO Create(int userId)
        {
            MatchDTO newMatch = _matchesRepository.Create(userOneId: userId, userTwoId: BotId);
            _userMatchesRepository.Create(userId: userId, matchId: newMatch.Id, hasInitiative: true);
            _userMatchesRepository.Create(userId: BotId, matchId: newMatch.Id, hasInitiative: false);
            return newMatch;
        }
    }
}

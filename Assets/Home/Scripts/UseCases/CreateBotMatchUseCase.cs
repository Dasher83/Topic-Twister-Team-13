using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Home.UseCases
{
    public class CreateBotMatchUseCase : ICreateBotMatchUseCase
    {
        private IMatchesRepository _matchesRepository;
        private const int BotId = 2;

        public CreateBotMatchUseCase(IMatchesRepository matchesRepository)
        {
            _matchesRepository = matchesRepository;
        }

        public MatchDTO Create(int userId)
        {
            return _matchesRepository.Create(userOneId: userId, userTwoId: BotId);
        }
    }
}

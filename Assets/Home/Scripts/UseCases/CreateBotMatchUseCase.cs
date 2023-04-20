using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;


namespace TopicTwister.Home.UseCases
{
    public class CreateBotMatchUseCase : ICreateBotMatchUseCase
    {
        private IMatchesRepository _matchesRepository;
        private IUserMatchesRepository _userMatchesRepository;
        private IUserRepository _userRepository;
        private const int BotId = 2;
        private MatchMapper _mapper;

        public CreateBotMatchUseCase(
            IMatchesRepository matchesRepository,
            IUserMatchesRepository userMatchesRepository,
            IUserRepository userRespository)
        {
            _matchesRepository = matchesRepository;
            _userMatchesRepository = userMatchesRepository;
            _userRepository = userRespository;
            _mapper = new MatchMapper();
        }

        public MatchDTO Create(int userId)
        {
            try
            {
                _userRepository.Get(userId);
            }
            catch (UserNotFoundByRepositoryException)
            {
                throw new UserNotFoundInUseCaseException(message: $"userId: {userId}");
            }

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

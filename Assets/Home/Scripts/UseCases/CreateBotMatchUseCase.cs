using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
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
        private IdtoMapper<Match, MatchDTO> _mapper;

        public CreateBotMatchUseCase(
            IMatchesRepository matchesRepository,
            IUserMatchesRepository userMatchesRepository,
            IUserRepository userRespository,
            IdtoMapper<Match, MatchDTO> mapper)
        {
            _matchesRepository = matchesRepository;
            _userMatchesRepository = userMatchesRepository;
            _userRepository = userRespository;
            _mapper = mapper;
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
            try
            {
                match = _matchesRepository.Persist(match);
            }
            catch (MatchNotPersistedByRepositoryException ex)
            {
                throw new MatchNotCreatedInUseCaseException(inner: ex);
            }

            UserMatch userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: true,
                userId: userId,
                match: match);

            try
            {
                _userMatchesRepository.Persist(userMatch);
            }
            catch(UserMatchNotPersistedByRepositoryException ex)
            {
                _matchesRepository.Delete(match.Id);
                throw new MatchNotCreatedInUseCaseException(inner: ex);
            }

            userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: false,
                userId: BotId,
                match: match);

            try
            {
                _userMatchesRepository.Persist(userMatch);
            }
            catch (UserMatchNotPersistedByRepositoryException ex)
            {
                _matchesRepository.Delete(match.Id);
                throw new MatchNotCreatedInUseCaseException(inner: ex);
            }

            return _mapper.ToDTO(match);
        }
    }
}

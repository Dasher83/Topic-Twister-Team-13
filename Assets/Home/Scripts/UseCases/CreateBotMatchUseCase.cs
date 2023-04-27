using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;
using TopicTwister.Shared.UseCases.Utils;


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

        public UseCaseResult<MatchDTO> Create(int userId)
        {
            User user;
            try
            {
               user = _userRepository.Get(userId);
            }
            catch (UserNotFoundByRepositoryException)
            {
                throw new UserNotFoundInUseCaseException(message: $"userId: {userId}");
            }

            Match match = new Match();
            try
            {
                match = _matchesRepository.Save(match);
            }
            catch (MatchNotSavedByRepositoryException ex)
            {
                throw new MatchNotCreatedInUseCaseException(inner: ex);
            }

            UserMatch userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: true,
                user: user,
                match: match);

            try
            {
                _userMatchesRepository.Save(userMatch);
            }
            catch(UserMatchNotSabedByRepositoryException ex)
            {
                _matchesRepository.Delete(match.Id);
                throw new MatchNotCreatedInUseCaseException(inner: ex);
            }

            userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: false,
                user: _userRepository.Get(BotId),
                match: match);

            try
            {
                _userMatchesRepository.Save(userMatch);
            }
            catch (UserMatchNotSabedByRepositoryException ex)
            {
                _matchesRepository.Delete(match.Id);
                throw new MatchNotCreatedInUseCaseException(inner: ex);
            }

            UseCaseResult<MatchDTO> useCaseResult = UseCaseResult<MatchDTO>.Success(outcome: _mapper.ToDTO(match));
            return useCaseResult;
        }
    }
}

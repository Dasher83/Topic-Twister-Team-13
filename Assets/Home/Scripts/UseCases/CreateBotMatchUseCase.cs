using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


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

        public Result<MatchDTO> Create(int userId)
        {
            Result<User> getUserOperationResult = _userRepository.Get(userId);

            if (getUserOperationResult.WasOk == false)
            {
                return Result<MatchDTO>.Failure(errorMessage: getUserOperationResult.ErrorMessage);
            }

            User user = getUserOperationResult.Outcome;
            Match match = new Match();
            Result<Match> saveMatchOperation = _matchesRepository.Save(match);

            if(saveMatchOperation.WasOk == false)
            {
                return Result<MatchDTO>.Failure(errorMessage: saveMatchOperation.ErrorMessage);
            }

            match = saveMatchOperation.Outcome;

            UserMatch userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: true,
                user: user,
                match: match);

            Result<UserMatch> saveUserMatchOperation = _userMatchesRepository.Save(userMatch);

            if(saveUserMatchOperation.WasOk == false)
            {
                _matchesRepository.Delete(match.Id);
                return Result<MatchDTO>.Failure(errorMessage: saveUserMatchOperation.ErrorMessage);
            }

            userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: false,
                user: _userRepository.Get(BotId).Outcome,
                match: match);

            saveUserMatchOperation = _userMatchesRepository.Save(userMatch);

            if (saveUserMatchOperation.WasOk == false)
            {
                _matchesRepository.Delete(match.Id);
                return Result<MatchDTO>.Failure(errorMessage: saveUserMatchOperation.ErrorMessage);
            }

            Result<MatchDTO> useCaseResult = Result<MatchDTO>.Success(outcome: _mapper.ToDTO(match));
            return useCaseResult;
        }
    }
}

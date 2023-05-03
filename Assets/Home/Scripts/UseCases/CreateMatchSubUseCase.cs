using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.UseCases
{
    public class CreateMatchSubUseCase: ICreateMatchSubUseCase
    {
        private IMatchesRepository _matchesRepository;
        private IUserMatchesRepository _userMatchesRepository;
        private IUserReadOnlyRepository _userRepository;
        private IdtoMapper<Match, MatchDto> _matchDtoMapper;

        public CreateMatchSubUseCase(
            IMatchesRepository matchesRepository,
            IUserMatchesRepository userMatchesRepository,
            IUserReadOnlyRepository userRespository,
            IdtoMapper<Match, MatchDto> matchDtoMapper)
        {
            _matchesRepository = matchesRepository;
            _userMatchesRepository = userMatchesRepository;
            _userRepository = userRespository;
            _matchDtoMapper = matchDtoMapper;
        }

        public Operation<MatchDto> Create(int userIdPlayerOne, int userIdPlayerTwo)
        {
            Match match = new Match();
            Operation<Match> saveMatchOperation = _matchesRepository.Save(match);

            if (saveMatchOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: saveMatchOperation.ErrorMessage);
            }

            match = saveMatchOperation.Outcome;

            Operation<UserMatch> createUserMatchOperation = CreateUserMatch(userId: userIdPlayerOne, match: match);
            
            if(createUserMatchOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: createUserMatchOperation.ErrorMessage);
            }
            
            createUserMatchOperation = CreateUserMatch(userId: userIdPlayerTwo, match: match);

            if (createUserMatchOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: createUserMatchOperation.ErrorMessage);
            }

            Operation<MatchDto> useCaseResult = Operation<MatchDto>.Success(outcome: _matchDtoMapper.ToDTO(match));
            return useCaseResult;
        }

        private Operation<UserMatch> CreateUserMatch(int userId, Match match)
        {
            Operation<User> getUserOperation = _userRepository.Get(userId);

            if (getUserOperation.WasOk == false)
            {
                return Operation<UserMatch>.Failure(errorMessage: getUserOperation.ErrorMessage);
            }

            User user = getUserOperation.Outcome;

            UserMatch userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: true,
                user: user,
                match: match);

            Operation<UserMatch> saveUserMatchOperation = _userMatchesRepository.Save(userMatch);

            if (saveUserMatchOperation.WasOk == false)
            {
                _matchesRepository.Delete(match.Id);
                return Operation<UserMatch>.Failure(errorMessage: saveUserMatchOperation.ErrorMessage);
            }

            return Operation<UserMatch>.Success(outcome: userMatch);
        }
    }
}
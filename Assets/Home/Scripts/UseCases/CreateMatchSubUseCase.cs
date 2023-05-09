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
        private IUsersReadOnlyRepository _usersReadOnlyRepository;
        private IdtoMapper<Match, MatchDto> _matchDtoMapper;

        public CreateMatchSubUseCase(
            IMatchesRepository matchesRepository,
            IUserMatchesRepository userMatchesRepository,
            IUsersReadOnlyRepository usersReadOnlyRespository,
            IdtoMapper<Match, MatchDto> matchDtoMapper)
        {
            _matchesRepository = matchesRepository;
            _userMatchesRepository = userMatchesRepository;
            _usersReadOnlyRepository = usersReadOnlyRespository;
            _matchDtoMapper = matchDtoMapper;
        }

        public Operation<MatchDto> Create(int userWithIniciative, int userWithoutIniciative)
        {
            if(userWithIniciative == userWithoutIniciative)
            {
                return Operation<MatchDto>.Failure(
                    errorMessage: $"A user cannot have a match with itself. User id: {userWithIniciative}");
            }

            Match match = new Match();
            Operation<Match> saveMatchOperation = _matchesRepository.Insert(match);

            if (saveMatchOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: saveMatchOperation.ErrorMessage);
            }

            match = saveMatchOperation.Result;

            Operation<UserMatch> createUserMatchOperation = CreateUserMatch(
                userId: userWithIniciative,
                match: match,
                hasInitiative: true);
            
            if(createUserMatchOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: createUserMatchOperation.ErrorMessage);
            }
            
            createUserMatchOperation = CreateUserMatch(
                userId: userWithoutIniciative,
                match: match,
                hasInitiative: false);

            if (createUserMatchOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: createUserMatchOperation.ErrorMessage);
            }

            Operation<MatchDto> useCaseResult = Operation<MatchDto>.Success(result: _matchDtoMapper.ToDTO(match));
            return useCaseResult;
        }

        private Operation<UserMatch> CreateUserMatch(int userId, Match match, bool hasInitiative)
        {
            Operation<User> getUserOperation = _usersReadOnlyRepository.Get(userId);

            if (getUserOperation.WasOk == false)
            {
                return Operation<UserMatch>.Failure(errorMessage: getUserOperation.ErrorMessage);
            }

            User user = getUserOperation.Result;

            UserMatch userMatch = new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: hasInitiative,
                user: user,
                match: match);

            Operation<UserMatch> saveUserMatchOperation = _userMatchesRepository.Insert(userMatch);

            if (saveUserMatchOperation.WasOk == false)
            {
                _matchesRepository.Delete(match.Id);
                return Operation<UserMatch>.Failure(errorMessage: saveUserMatchOperation.ErrorMessage);
            }

            return Operation<UserMatch>.Success(result: userMatch);
        }
    }
}

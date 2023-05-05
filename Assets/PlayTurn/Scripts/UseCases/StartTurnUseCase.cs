using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


public class StartTurnUseCase : IStartTurnUseCase
{
    public IUsersReadOnlyRepository _usersReadOnlyRepository;
    public IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    public IUserMatchesRepository _userMatchesRepository;

    public StartTurnUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IMatchesReadOnlyRepository matchesReadOnlyRepository,
        IUserMatchesRepository userMatchesRepository)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _matchesReadOnlyRepository = matchesReadOnlyRepository;
        _userMatchesRepository = userMatchesRepository;
    }

    public Operation<bool> Execute(int userId, int matchId)
    {
        Operation<User> getUserOperation = _usersReadOnlyRepository.Get(id: userId);

        if(getUserOperation.WasOk == false)
        {
            return Operation<bool>.Failure(errorMessage: getUserOperation.ErrorMessage);
        }

        Operation<Match> getMatchOperation = _matchesReadOnlyRepository.Get(id: matchId);

        if (getMatchOperation.WasOk == false)
        {
            return Operation<bool>.Failure(errorMessage: getMatchOperation.ErrorMessage);
        }

        Operation<UserMatch> getUserMatchOperation = _userMatchesRepository.Get(userId: userId, matchId: matchId);

        if(getUserMatchOperation.WasOk == false)
        {
            return Operation<bool>.Failure(errorMessage: $"User with id {userId} is not involved in match with id {matchId}");
        }

        throw new System.NotImplementedException();
    }
}

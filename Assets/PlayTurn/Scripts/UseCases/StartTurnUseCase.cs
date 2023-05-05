using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


public class StartTurnUseCase : IStartTurnUseCase
{
    public IUsersReadOnlyRepository _usersReadOnlyRepository;
    public IMatchesReadOnlyRepository _matchesReadOnlyRepository;

    public StartTurnUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IMatchesReadOnlyRepository matchesReadOnlyRepository)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _matchesReadOnlyRepository = matchesReadOnlyRepository;
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

        throw new System.NotImplementedException();
    }
}

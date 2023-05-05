using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


public class StartTurnUseCase : IStartTurnUseCase
{
    public IUsersReadOnlyRepository _userReadOnlyRepository;

    public StartTurnUseCase(IUsersReadOnlyRepository userReadOnlyRepository)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public Operation<bool> Execute(int userId, MatchDto matchDto)
    {
        Operation<User> getUserOperation = _userReadOnlyRepository.Get(userId: userId);

        if(getUserOperation.WasOk == false)
        {
            return Operation<bool>.Failure(errorMessage: getUserOperation.ErrorMessage);
        }

        throw new System.NotImplementedException();
    }
}

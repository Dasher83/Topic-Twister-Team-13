using System;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


public class EndTurnUseCase : IEndTurnUseCase
{
    private IUsersReadOnlyRepository _usersReadOnlyRepository;

    public EndTurnUseCase(IUsersReadOnlyRepository usersReadOnlyRepository)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
    }

    public Operation<TurnWithEvaluatedAnswersDto> Execute(int userId, int matchId, AnswerDto[] answerDtos)
    {
        Operation<User> getUserOperation = _usersReadOnlyRepository.Get(id: userId);

        if (getUserOperation.WasOk == false)
        {
            return Operation<TurnWithEvaluatedAnswersDto>.Failure(errorMessage: getUserOperation.ErrorMessage);
        }

        throw new NotImplementedException();
    }
}

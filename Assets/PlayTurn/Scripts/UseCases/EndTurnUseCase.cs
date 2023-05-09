using System;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


public class EndTurnUseCase : IEndTurnUseCase
{
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;

    public EndTurnUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IMatchesReadOnlyRepository matchesReadOnlyRepository)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _matchesReadOnlyRepository = matchesReadOnlyRepository;
    }

    public Operation<TurnWithEvaluatedAnswersDto> Execute(int userId, int matchId, AnswerDto[] answerDtos)
    {
        Operation<User> getUserOperation = _usersReadOnlyRepository.Get(id: userId);

        if (getUserOperation.WasOk == false)
        {
            return Operation<TurnWithEvaluatedAnswersDto>.Failure(errorMessage: getUserOperation.ErrorMessage);
        }

        Operation<Match> getMatchOperation = _matchesReadOnlyRepository.Get(id: matchId);

        if (getMatchOperation.WasOk == false)
        {
            return Operation<TurnWithEvaluatedAnswersDto>.Failure(errorMessage: getMatchOperation.ErrorMessage);
        }

        throw new NotImplementedException();
    }
}

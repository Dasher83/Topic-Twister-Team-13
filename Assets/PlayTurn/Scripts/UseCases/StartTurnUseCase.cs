using System.Collections.Generic;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


public class StartTurnUseCase : IStartTurnUseCase
{
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IUserMatchesRepository _userMatchesRepository;
    private ITurnsReadOnlyRepository _turnsReadOnlyRepository;
    private IRoundsReadOnlyRepository _roundsReadOnlyRepository;

    public StartTurnUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IMatchesReadOnlyRepository matchesReadOnlyRepository,
        IUserMatchesRepository userMatchesRepository,
        ITurnsReadOnlyRepository turnsReadOnlyRepository,
        IRoundsReadOnlyRepository roundsReadOnlyRepository)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _matchesReadOnlyRepository = matchesReadOnlyRepository;
        _userMatchesRepository = userMatchesRepository;
        _turnsReadOnlyRepository = turnsReadOnlyRepository;
        _roundsReadOnlyRepository = roundsReadOnlyRepository;
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

        Match match = getMatchOperation.Outcome;
        List<Round> rounds = _roundsReadOnlyRepository.GetMany(match.Id).Outcome;

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: rounds);

        int activeRoundId = match.ActiveRound.Id;

        Operation<UserMatch> getUserMatchOperation = _userMatchesRepository.Get(userId: userId, matchId: matchId);

        if(getUserMatchOperation.WasOk == false)
        {
            return Operation<bool>.Failure(errorMessage: $"User with id {userId} is not involved in match with id {matchId}");
        }

        Operation<Turn> getTurnOperation = _turnsReadOnlyRepository.Get(userId: userId, roundId: activeRoundId);
        
        if(getTurnOperation.WasOk == true)
        {
            return Operation<bool>.Failure(
                errorMessage: $"Turn already exists for user with id {userId} " +
                $"in round with id {activeRoundId} in match with id {matchId}");
        }

        throw new System.NotImplementedException();
    }
}

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

        Match match = getMatchOperation.Result;
        List<Round> rounds = _roundsReadOnlyRepository.GetMany(match.Id).Result;

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

        Operation<UserMatch[]> getUserMatchesOperation = _userMatchesRepository.GetMany(matchId: matchId);

        if (getUserMatchesOperation.WasOk == false)
        {
            return Operation<bool>.Failure(errorMessage: getUserMatchesOperation.ErrorMessage);
        }

        UserMatch[] userMatches = getUserMatchesOperation.Result;
        UserMatch opponentUserMatch;

        if (userMatches[0].User.Id != userId)
        {
            opponentUserMatch = userMatches[0];
        }
        else
        {
            opponentUserMatch = userMatches[1];
        }

        if (opponentUserMatch.HasInitiative)
        {
            Operation<Turn> getOpponentTurnOperation = _turnsReadOnlyRepository
                .Get(userId: opponentUserMatch.User.Id, roundId: activeRoundId);

            if(getOpponentTurnOperation.WasOk == false)
            {
                string message = $"Turn can not be created for user with id {userId} " +
                $"in round with id {activeRoundId} in match with id {matchId} " +
                $"since user with id {opponentUserMatch.User.Id} has not finished his turn yet";

                return Operation<bool>.Failure(errorMessage: message);
            }
        }

        throw new System.NotImplementedException();
    }
}

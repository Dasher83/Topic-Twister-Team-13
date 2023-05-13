using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Utils;


public class EndTurnUseCase : IEndTurnUseCase
{
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IUserMatchesRepository _userMatchesRepository;
    private ITurnsRepository _turnsRepository;
    private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
    private IdtoMapper<Match, MatchDto> _matchDtoMapper;
    private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
    private IdtoMapper<UserMatch, UserMatchDto> _userMatchDtoMapper;
    private IdtoMapper<Turn, TurnDto> _turnDtoMapper;
    private IdtoMapper<Answer, AnswerDto> _answerDtoMapper;

    public EndTurnUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IMatchesReadOnlyRepository matchesReadOnlyRepository,
        IUserMatchesRepository userMatchesRepository,
        ITurnsRepository turnsRepository,
        IRoundsReadOnlyRepository roundsReadOnlyRepository,
        IdtoMapper<Match, MatchDto> matchDtoMapper,
        IdtoMapper<Round, RoundWithCategoriesDto> roundWithCategoriesDtoMapper,
        IdtoMapper<UserMatch, UserMatchDto> userMatchDtoMapper,
        IdtoMapper<Turn, TurnDto> turnDtoMapper,
        IdtoMapper<Answer, AnswerDto> answerDtoMapper)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _matchesReadOnlyRepository = matchesReadOnlyRepository;
        _userMatchesRepository = userMatchesRepository;
        _turnsRepository = turnsRepository;
        _roundsReadOnlyRepository = roundsReadOnlyRepository;
        _matchDtoMapper = matchDtoMapper;
        _roundWithCategoriesDtoMapper = roundWithCategoriesDtoMapper;
        _userMatchDtoMapper = userMatchDtoMapper;
        _turnDtoMapper = turnDtoMapper;
        _answerDtoMapper = answerDtoMapper;
    }

    public Operation<MatchFullStateDto> Execute(int userId, int matchId, AnswerDto[] answerDtos)
    {
        Operation<User> getUserOperation = _usersReadOnlyRepository.Get(id: userId);

        if (getUserOperation.WasOk == false)
        {
            return Operation<MatchFullStateDto>.Failure(errorMessage: getUserOperation.ErrorMessage);
        }

        User user = getUserOperation.Result;

        Operation<Match> getMatchOperation = _matchesReadOnlyRepository.Get(id: matchId);

        if (getMatchOperation.WasOk == false)
        {
            return Operation<MatchFullStateDto>.Failure(errorMessage: getMatchOperation.ErrorMessage);
        }

        Match match = getMatchOperation.Result;

        Operation<UserMatch[]> getUserMatchesOperation = _userMatchesRepository.GetMany(matchId: matchId);

        if (getUserMatchesOperation.WasOk == false)
        {
            return Operation<MatchFullStateDto>.Failure(errorMessage: getUserMatchesOperation.ErrorMessage);
        }

        UserMatch[] userMatches = getUserMatchesOperation.Result;

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: null,
            userMatches: userMatches);

        UserMatch requesterUserMatch;
        UserMatch opponentUserMatch;

        Operation<bool> userIsInMatchOperation = match.UserIsInMatch(userId: userId);

        if (userIsInMatchOperation.WasOk == false)
        {
            return Operation<MatchFullStateDto>.Failure(errorMessage: userIsInMatchOperation.ErrorMessage);
        }

        if (userMatches[0].User.Id != userId)
        {
            opponentUserMatch = userMatches[0];
            requesterUserMatch = userMatches[1];
        }
        else
        {
            requesterUserMatch = userMatches[0];
            opponentUserMatch = userMatches[1];
        }

        Operation<List<Round>> getRoundsOperation = _roundsReadOnlyRepository.GetMany(matchId: match.Id);

        if (getRoundsOperation.WasOk == false)
        {
            return Operation<MatchFullStateDto>.Failure(errorMessage: getRoundsOperation.ErrorMessage);
        }

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: getRoundsOperation.Result);

        Round activeRound = match.ActiveRound;

        Operation<Turn> getTurnOperation = _turnsRepository.Get(userId: userId, roundId: activeRound.Id);

        if (getTurnOperation.WasOk == false)
        {
            return Operation<MatchFullStateDto>.Failure(
                errorMessage: $"Turn not found for user with id {user.Id} " +
                $"in round with id {activeRound.Id} in match with id {match.Id}");
        }

        Turn turn = getTurnOperation.Result;

        if (turn.HasEnded)
        {
            return Operation<MatchFullStateDto>.Failure(errorMessage: getTurnOperation.ErrorMessage);
        }

        List<AnswerDto> cleanAnswerDtos = answerDtos.Where(answerDto => answerDto != null).ToList();

        List<int> answerCategoryIds = cleanAnswerDtos.Select(answerDto => answerDto.CategoryDto.Id).Distinct().ToList();
        
        
        if(answerCategoryIds.Count > Configuration.CategoriesPerRound)
        {
            string errorMessage = $"Too many answers for turn of user with id {user.Id} " +
                $"for round with id {activeRound.Id} for match with id {match.Id}";

            return Operation<MatchFullStateDto>.Failure(errorMessage: errorMessage);
        }

        if (answerCategoryIds.Count < Configuration.CategoriesPerRound)
        {
            string errorMessage = $"Too few answers for turn of user with id {user.Id} " +
                $"for round with id {activeRound.Id} for match with id {match.Id}";

            return Operation<MatchFullStateDto>.Failure(errorMessage: errorMessage);
        }

        List<int> roundCategoryIds = activeRound.Categories.Select(category => category.Id).Distinct().ToList();

        if (answerCategoryIds.Except(roundCategoryIds).Any() || roundCategoryIds.Except(answerCategoryIds).Any())
        {
            string errorMessage =
                $"Some of your answers don't match the categories for " +
                $"round with id {activeRound.Id} in match with id {match.Id}";

            return Operation<MatchFullStateDto>.Failure(errorMessage: errorMessage);
        }

        List<Answer> requesterAnswers = new List<Answer>();

        turn = new Turn(
            user: turn.User,
            round: turn.Round,
            startDateTime: turn.StartDateTime,
            endDateTime: DateTime.UtcNow);

        foreach (AnswerDto answerDto in answerDtos)
        {
            Answer answer = new Answer(
                userInput: answerDto.UserInput,
                order: answerDto.Order,
                category: activeRound.Categories.Single(category => category.Id == answerDto.CategoryDto.Id),
                turn: turn);

            requesterAnswers.Add(answer);
        }

        turn = new Turn(
            user: turn.User,
            round: turn.Round,
            startDateTime: turn.StartDateTime,
            endDateTime: turn.EndDateTime,
            answers: requesterAnswers);

        Operation<Turn> updateTurnOperation = _turnsRepository.Update(turn);

        if(updateTurnOperation.WasOk == false)
        {
            return Operation<MatchFullStateDto>.Failure(errorMessage: updateTurnOperation.ErrorMessage);
        }

        UserMatchDto userWithInitiativeMatchDto;
        UserMatchDto userWithoutInitiativeMatchDto;

        List<AnswerDto> answerDtosOfUserWithInitiative;
        List<AnswerDto> answerDtosOfUserWithoutInitiative;

        int userWithInitiativeId;
        int userWithoutInitiativeId;

        if (requesterUserMatch.HasInitiative)
        {
            userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(requesterUserMatch);
            userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(opponentUserMatch);
            answerDtosOfUserWithInitiative = _answerDtoMapper.ToDTOs(requesterAnswers);
            answerDtosOfUserWithoutInitiative = new List<AnswerDto>();
            userWithInitiativeId = requesterUserMatch.User.Id;
            userWithoutInitiativeId = opponentUserMatch.User.Id;
        }
        else
        {
            userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(opponentUserMatch);
            userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(requesterUserMatch);
            answerDtosOfUserWithInitiative = new List<AnswerDto>();
            answerDtosOfUserWithoutInitiative = _answerDtoMapper.ToDTOs(requesterAnswers);
            userWithInitiativeId = opponentUserMatch.User.Id;
            userWithoutInitiativeId = requesterUserMatch.User.Id;
        }

        Operation<List<Turn>> getTurnsWithIniciativeOperation = _turnsRepository.GetMany(userId: userWithInitiativeId, matchId: match.Id);

        List<TurnDto> turnDtosOfUserWithInitiative = getTurnsWithIniciativeOperation
            .Result
            .Select(turn => _turnDtoMapper.ToDTO(turn))
            .ToList();

        Operation<List<Turn>> getTurnsWithoutInitiativeOperation = _turnsRepository.GetMany(userId: userWithoutInitiativeId, matchId: match.Id);

        List<TurnDto> turnDtosOfUserWithoutInitiative = getTurnsWithoutInitiativeOperation
            .Result
            .Select(turn => _turnDtoMapper.ToDTO(turn))
            .ToList();

        MatchFullStateDto matchFullStateDto = new MatchFullStateDto(
            matchDto: _matchDtoMapper.ToDTO(match),
            roundWithCategoriesDtos: _roundWithCategoriesDtoMapper.ToDTOs(getRoundsOperation.Result),
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutInitiativeMatchDto,
            userWithInitiativeRoundDtos: new List<UserRoundDto>(),
            userWithoutInitiativeRoundDtos: new List<UserRoundDto>(),
            answerDtosOfUserWithInitiative: answerDtosOfUserWithInitiative,
            answerDtosOfUserWithoutInitiative: answerDtosOfUserWithoutInitiative,
            turnDtosOfUserWithInitiative: turnDtosOfUserWithInitiative,
            turnDtosOfUserWithoutInitiative: turnDtosOfUserWithoutInitiative);

        return Operation<MatchFullStateDto>.Success(result: matchFullStateDto);
    }
}

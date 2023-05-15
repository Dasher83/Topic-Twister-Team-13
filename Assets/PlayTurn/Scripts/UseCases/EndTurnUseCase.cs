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
    private IdtoMapper<Answer, AnswerDto> _answerDtoMapper;
    private IAnswersRepository _answersRepository;
    private IWordsRepository _wordsRepository;

    public EndTurnUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IMatchesReadOnlyRepository matchesReadOnlyRepository,
        IUserMatchesRepository userMatchesRepository,
        ITurnsRepository turnsRepository,
        IRoundsReadOnlyRepository roundsReadOnlyRepository,
        IdtoMapper<Match, MatchDto> matchDtoMapper,
        IdtoMapper<Round, RoundWithCategoriesDto> roundWithCategoriesDtoMapper,
        IdtoMapper<UserMatch, UserMatchDto> userMatchDtoMapper,
        IdtoMapper<Answer, AnswerDto> answerDtoMapper,
        IAnswersRepository answersRepository,
        IWordsRepository wordsRepository)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _matchesReadOnlyRepository = matchesReadOnlyRepository;
        _userMatchesRepository = userMatchesRepository;
        _turnsRepository = turnsRepository;
        _roundsReadOnlyRepository = roundsReadOnlyRepository;
        _matchDtoMapper = matchDtoMapper;
        _roundWithCategoriesDtoMapper = roundWithCategoriesDtoMapper;
        _userMatchDtoMapper = userMatchDtoMapper;
        _answerDtoMapper = answerDtoMapper;
        _answersRepository = answersRepository;
        _wordsRepository = wordsRepository;
    }

    public Operation<EndOfTurnDto> Execute(int userId, int matchId, AnswerDto[] answerDtos)
    {
        Operation<User> getUserOperation = _usersReadOnlyRepository.Get(id: userId);

        if (getUserOperation.WasOk == false)
        {
            return Operation<EndOfTurnDto>.Failure(errorMessage: getUserOperation.ErrorMessage);
        }

        User user = getUserOperation.Result;

        Operation<Match> getMatchOperation = _matchesReadOnlyRepository.Get(id: matchId);

        if (getMatchOperation.WasOk == false)
        {
            return Operation<EndOfTurnDto>.Failure(errorMessage: getMatchOperation.ErrorMessage);
        }

        Match match = getMatchOperation.Result;

        Operation<UserMatch[]> getUserMatchesOperation = _userMatchesRepository.GetMany(matchId: matchId);

        if (getUserMatchesOperation.WasOk == false)
        {
            return Operation<EndOfTurnDto>.Failure(errorMessage: getUserMatchesOperation.ErrorMessage);
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
            return Operation<EndOfTurnDto>.Failure(errorMessage: userIsInMatchOperation.ErrorMessage);
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
            return Operation<EndOfTurnDto>.Failure(errorMessage: getRoundsOperation.ErrorMessage);
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
            return Operation<EndOfTurnDto>.Failure(
                errorMessage: $"Turn not found for user with id {user.Id} " +
                $"in round with id {activeRound.Id} in match with id {match.Id}");
        }

        Turn turn = getTurnOperation.Result;

        if (turn.HasEnded)
        {
            string errorMessage = $"Turn already ended for user with id {user.Id} " +
                    $"in round with id {activeRound.Id} in match with id {match.Id}";

            return Operation<EndOfTurnDto>.Failure(errorMessage: errorMessage);
        }

        List<AnswerDto> cleanAnswerDtos = answerDtos.Where(answerDto => answerDto != null).ToList();

        List<int> answerCategoryIds = cleanAnswerDtos.Select(answerDto => answerDto.CategoryDto.Id).Distinct().ToList();
        
        if(answerCategoryIds.Count > Configuration.CategoriesPerRound)
        {
            string errorMessage = $"Too many answers for turn of user with id {user.Id} " +
                $"for round with id {activeRound.Id} for match with id {match.Id}";

            return Operation<EndOfTurnDto>.Failure(errorMessage: errorMessage);
        }

        if (answerCategoryIds.Count < Configuration.CategoriesPerRound)
        {
            string errorMessage = $"Too few answers for turn of user with id {user.Id} " +
                $"for round with id {activeRound.Id} for match with id {match.Id}";

            return Operation<EndOfTurnDto>.Failure(errorMessage: errorMessage);
        }

        List<int> roundCategoryIds = activeRound.Categories.Select(category => category.Id).Distinct().ToList();

        if (answerCategoryIds.Except(roundCategoryIds).Any() || roundCategoryIds.Except(answerCategoryIds).Any())
        {
            string errorMessage =
                $"Some of your answers don't match the categories for " +
                $"round with id {activeRound.Id} in match with id {match.Id}";

            return Operation<EndOfTurnDto>.Failure(errorMessage: errorMessage);
        }

        List<Answer> requesterNewAnswers = new List<Answer>();

        turn = new Turn(
            user: turn.User,
            round: turn.Round,
            startDateTime: turn.StartDateTime,
            endDateTime: DateTime.UtcNow);

        Operation<Turn> updateTurnOperation = _turnsRepository.Update(turn);

        if (updateTurnOperation.WasOk == false)
        {
            return Operation<EndOfTurnDto>.Failure(errorMessage: updateTurnOperation.ErrorMessage);
        }

        turn = updateTurnOperation.Result;

        foreach (AnswerDto answerDto in answerDtos)
        {
            Answer answer = new Answer(
                userInput: answerDto.UserInput,
                order: answerDto.Order,
                category: activeRound.Categories.Single(category => category.Id == answerDto.CategoryDto.Id),
                turn: turn);

            Operation<Answer> insertAnswerOperation = _answersRepository.Insert(answer);

            if(insertAnswerOperation.WasOk == false)
            {
                return Operation<EndOfTurnDto>.Failure(errorMessage: insertAnswerOperation.ErrorMessage);
            }

            requesterNewAnswers.Add(insertAnswerOperation.Result);
        }

        turn = new Turn(
            user: turn.User,
            round: turn.Round,
            startDateTime: turn.StartDateTime,
            endDateTime: turn.EndDateTime,
            answers: requesterNewAnswers,
            wordsRepository: _wordsRepository);

        updateTurnOperation = _turnsRepository.Update(turn);

        if (updateTurnOperation.WasOk == false)
        {
            return Operation<EndOfTurnDto>.Failure(errorMessage: updateTurnOperation.ErrorMessage);
        }

        turn = updateTurnOperation.Result;

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
            answerDtosOfUserWithInitiative = _answerDtoMapper.ToDTOs(requesterNewAnswers);
            answerDtosOfUserWithoutInitiative = new List<AnswerDto>();
            userWithInitiativeId = requesterUserMatch.User.Id;
            userWithoutInitiativeId = opponentUserMatch.User.Id;
        }
        else
        {
            userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(opponentUserMatch);
            userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(requesterUserMatch);
            answerDtosOfUserWithInitiative = new List<AnswerDto>();
            answerDtosOfUserWithoutInitiative = _answerDtoMapper.ToDTOs(requesterNewAnswers);
            userWithInitiativeId = opponentUserMatch.User.Id;
            userWithoutInitiativeId = requesterUserMatch.User.Id;
        }

        EndOfTurnDto matchFullStateDto = new EndOfTurnDto(
            matchDto: _matchDtoMapper.ToDTO(match),
            roundWithCategoriesDto: _roundWithCategoriesDtoMapper.ToDTO(turn.Round),
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutInitiativeMatchDto,
            userWithInitiativeRoundDtos: new List<UserRoundDto>(),
            userWithoutInitiativeRoundDtos: new List<UserRoundDto>(),
            answerDtosOfUserWithInitiative: answerDtosOfUserWithInitiative,
            answerDtosOfUserWithoutInitiative: answerDtosOfUserWithoutInitiative);

        return Operation<EndOfTurnDto>.Success(result: matchFullStateDto);
    }
}

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
    private IUserRoundsRepository _userRoundsRepository;
    private IdtoMapper<UserRound, UserRoundDto> _userRoundDtoMapper;

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
        IWordsRepository wordsRepository,
        IUserRoundsRepository userRoundsRepository,
        IdtoMapper<UserRound, UserRoundDto> userRoundDtoMapper)
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
        _userRoundsRepository = userRoundsRepository;
        _userRoundDtoMapper = userRoundDtoMapper;
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

        Turn requesterTurn = getTurnOperation.Result;

        if (requesterTurn.HasEnded)
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

        requesterTurn = new Turn(
            user: requesterTurn.User,
            round: requesterTurn.Round,
            startDateTime: requesterTurn.StartDateTime,
            endDateTime: DateTime.UtcNow);

        Operation<Turn> updateTurnOperation = _turnsRepository.Update(requesterTurn);

        if (updateTurnOperation.WasOk == false)
        {
            return Operation<EndOfTurnDto>.Failure(errorMessage: updateTurnOperation.ErrorMessage);
        }

        requesterTurn = updateTurnOperation.Result;

        foreach (AnswerDto answerDto in answerDtos)
        {
            Answer answer = new Answer(
                userInput: answerDto.UserInput,
                order: answerDto.Order,
                category: activeRound.Categories.Single(category => category.Id == answerDto.CategoryDto.Id),
                turn: requesterTurn);

            Operation<Answer> insertAnswerOperation = _answersRepository.Insert(answer);

            if(insertAnswerOperation.WasOk == false)
            {
                return Operation<EndOfTurnDto>.Failure(errorMessage: insertAnswerOperation.ErrorMessage);
            }

            requesterNewAnswers.Add(insertAnswerOperation.Result);
        }

        requesterTurn = new Turn(
            user: requesterTurn.User,
            round: requesterTurn.Round,
            startDateTime: requesterTurn.StartDateTime,
            endDateTime: requesterTurn.EndDateTime,
            answers: requesterNewAnswers,
            wordsRepository: _wordsRepository);

        updateTurnOperation = _turnsRepository.Update(requesterTurn);

        if (updateTurnOperation.WasOk == false)
        {
            return Operation<EndOfTurnDto>.Failure(errorMessage: updateTurnOperation.ErrorMessage);
        }

        requesterTurn = updateTurnOperation.Result;

        UserMatchDto userWithInitiativeMatchDto;
        UserMatchDto userWithoutInitiativeMatchDto;

        List<AnswerDto> answerDtosOfUserWithInitiative;
        List<AnswerDto> answerDtosOfUserWithoutInitiative;

        int userWithInitiativeId;
        int userWithoutInitiativeId;

        List <UserRoundDto> userWithInitiativeRoundDtos = new List<UserRoundDto>();
        List <UserRoundDto> userWithoutInitiativeRoundDtos = new List<UserRoundDto>();

        if (requesterUserMatch.HasInitiative)
        {
            userWithInitiativeId = requesterUserMatch.User.Id;
            userWithoutInitiativeId = opponentUserMatch.User.Id;

            userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(requesterUserMatch);
            userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(opponentUserMatch);
            answerDtosOfUserWithInitiative = _answerDtoMapper.ToDTOs(requesterNewAnswers);
            answerDtosOfUserWithoutInitiative = new List<AnswerDto>();
        }
        else
        {
            userWithInitiativeId = opponentUserMatch.User.Id;
            userWithoutInitiativeId = requesterUserMatch.User.Id;

            userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(opponentUserMatch);
            userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(requesterUserMatch);

            answerDtosOfUserWithInitiative = _answerDtoMapper.ToDTOs(
                _answersRepository.GetMany(userId: userWithInitiativeId, roundId: requesterTurn.Round.Id).Result);
            
            answerDtosOfUserWithoutInitiative = _answerDtoMapper.ToDTOs(requesterNewAnswers);

            Turn turnOfOpponent = _turnsRepository
                .Get(userId: opponentUserMatch.User.Id, roundId: requesterTurn.Round.Id).Result;

            UserRound userWithInitiativeRound = new UserRound(
                user: opponentUserMatch.User,
                round: requesterTurn.Round,
                isWinner: turnOfOpponent.Points >= requesterTurn.Points,
                points: requesterTurn.Points);

            Operation<UserRound> insertUserRoundOperation = _userRoundsRepository.Insert(userRound: userWithInitiativeRound);

            if(insertUserRoundOperation.WasOk == false)
            {
                return Operation<EndOfTurnDto>.Failure(errorMessage: insertUserRoundOperation.ErrorMessage);
            }

            UserRound userWithoutInitiativeRound = new UserRound(
                user: requesterUserMatch.User,
                round: requesterTurn.Round,
                isWinner: requesterTurn.Points >= turnOfOpponent.Points,
                points: requesterTurn.Points);

            insertUserRoundOperation = _userRoundsRepository.Insert(userRound: userWithoutInitiativeRound);

            if (insertUserRoundOperation.WasOk == false)
            {
                return Operation<EndOfTurnDto>.Failure(errorMessage: insertUserRoundOperation.ErrorMessage);
            }

            List<UserRound> userWithInitiativeRounds = _userRoundsRepository.GetMany(
                userId: userWithInitiativeId, roundIds: match.Rounds.Select(round => round.Id).ToList()).Result;

            userWithInitiativeRoundDtos = _userRoundDtoMapper.ToDTOs(userWithInitiativeRounds);

            List<UserRound> userWithoutInitiativeRounds = _userRoundsRepository.GetMany(
                userId: userWithoutInitiativeId, roundIds: match.Rounds.Select(round => round.Id).ToList()).Result;

            userWithoutInitiativeRoundDtos = _userRoundDtoMapper.ToDTOs(userWithoutInitiativeRounds);
        }

        EndOfTurnDto matchFullStateDto = new EndOfTurnDto(
            matchDto: _matchDtoMapper.ToDTO(match),
            roundWithCategoriesDto: _roundWithCategoriesDtoMapper.ToDTO(requesterTurn.Round),
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutInitiativeMatchDto,
            userWithInitiativeRoundDtos: userWithInitiativeRoundDtos,
            userWithoutInitiativeRoundDtos: userWithoutInitiativeRoundDtos,
            answerDtosOfUserWithInitiative: answerDtosOfUserWithInitiative,
            answerDtosOfUserWithoutInitiative: answerDtosOfUserWithoutInitiative);

        return Operation<EndOfTurnDto>.Success(result: matchFullStateDto);
    }
}

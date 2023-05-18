using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.PlayTurn.UseCases;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Utils;


public class EndTurnUseCaseUnitTests
{
    private IEndTurnUseCase _useCase;
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IUserMatchesRepository _userMatchesRepository;
    private ITurnsRepository _turnsRepository;
    private IRoundsRepository _roundsRepository;
    private IdtoMapper<Match, MatchDto> _matchDtoMapper;
    private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
    private IdtoMapper<UserMatch, UserMatchDto> _userMatchDtoMapper;
    private IdtoMapper<Turn, TurnDto> _turnDtoMapper;
    private IdtoMapper<Answer, AnswerDto> _answerDtoMapper;
    private IAnswersRepository _answersRepository;
    private IWordsRepository _wordsRepository;
    private IUserRoundsRepository _userRoundsRepository;
    private IdtoMapper<UserRound, UserRoundDto> _userRoundDtoMapper;

    [SetUp]
    public void SetUp()
    {
        _usersReadOnlyRepository = Substitute.For<IUsersReadOnlyRepository>();
        _matchesReadOnlyRepository = Substitute.For<IMatchesReadOnlyRepository>();
        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _turnsRepository = Substitute.For<ITurnsRepository>();
        _roundsRepository = Substitute.For<IRoundsRepository>();
        _matchDtoMapper = Substitute.For<IdtoMapper<Match, MatchDto>>();
        _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();
        _userMatchDtoMapper = Substitute.For<IdtoMapper<UserMatch, UserMatchDto>>();
        _turnDtoMapper = Substitute.For<IdtoMapper<Turn, TurnDto>>();
        _answerDtoMapper = Substitute.For<IdtoMapper<Answer, AnswerDto>>();
        _answersRepository = Substitute.For<IAnswersRepository>();
        _wordsRepository = Substitute.For<IWordsRepository>();
        _userRoundsRepository = Substitute.For<IUserRoundsRepository>();
        _userRoundDtoMapper = Substitute.For<IdtoMapper<UserRound, UserRoundDto>>();

        _useCase = new EndTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userMatchesRepository: _userMatchesRepository,
            turnsRepository: _turnsRepository,
            roundsRepository: _roundsRepository,
            matchDtoMapper: _matchDtoMapper,
            roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper,
            userMatchDtoMapper: _userMatchDtoMapper,
            answerDtoMapper: _answerDtoMapper,
            answersRepository: _answersRepository,
            wordsRepository: _wordsRepository,
            userRoundsRepository: _userRoundsRepository,
            userRoundDtoMapper: _userRoundDtoMapper);
    }

    [Test]
    public void Test_ok_for_user_with_initiative_inside_time_limit()
    {
        #region -- Arrange --
        int userWithInitiativeId = Configuration.TestUserId;
        int userWithoutInitiativeId = Configuration.TestBotId;
        int matchId = 0;
        int roundId = 0;
        Match match;
        Round round;

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                return Operation<User>.Success(result: new User(id: lambdaUserId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'j',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: new List<Round>() { round });

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                int lambdaMatchId = (int)args[1];

                UserMatch lambdaUserMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: lambdaUserId == userWithInitiativeId,
                    user: _usersReadOnlyRepository.Get(id: lambdaUserId).Result,
                    match: _matchesReadOnlyRepository.Get(lambdaMatchId).Result);

                return Operation<UserMatch>.Success(result: lambdaUserMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userWithInitiativeId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(userWithoutInitiativeId, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        List<Category> categories = new List<Category>
        {
            new Category(id: 0, name: ""),
            new Category(id: 1, name: ""),
            new Category(id: 2, name: ""),
            new Category(id: 3, name: ""),
            new Category(id: 4, name: "")
        };

        _roundsRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: categories);

                return Operation<Round>.Success(result: round);
            });

        _roundsRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> rounds = new List<Round>();
                Round activeRound = _roundsRepository.Get(roundId).Result;
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsRepository.Get(userWithInitiativeId, roundId).Returns(
            (args) =>
            {
                DateTime lamdaTurnStartDateTime = DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds);

                Turn lambdaTurn = new Turn(
                    user: _usersReadOnlyRepository.Get(id: userWithInitiativeId).Result,
                    round: _roundsRepository.Get(id: roundId).Result,
                    startDateTime: lamdaTurnStartDateTime);

                return Operation<Turn>.Success(result: lambdaTurn);
            });

        _wordsRepository.Exists(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<char>()).Returns(
            (args) =>
            {
                string text = (string)args[0];
                char initialLetter = (char)args[2];
                string other = $"{initialLetter} TEST".ToLower();
                bool result = text.ToLower() == other;
                return Operation<bool>.Success(result: result);
            });

        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        List<CategoryDto> categoryDtos = categories
            .Select((category, index) => new CategoryDto(id: categories[index].Id, name: categories[index].Name))
            .ToList();

        for (int i = 0; i < 3; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: categoryDtos[i],
                userInput: $"{_roundsRepository.Get(roundId).Result.InitialLetter} TEST",
                order: i);
        }

        for (int i = 3; i < answerDtos.Length; i++)
        {
            answerDtos[i] = new AnswerDto(categoryDto: categoryDtos[i], userInput: "Something", order: i);
        }

        _answerDtoMapper.ToDTO(Arg.Any<Answer>()).Returns(
            (args) =>
            {
                Answer lambdaAnswer = (Answer)args[0];

                AnswerDto lambdaAnswerDto = new AnswerDto(
                    categoryDto: categoryDtos.Single(categoryDto => categoryDto.Id == lambdaAnswer.Category.Id),
                    userInput: lambdaAnswer.UserInput,
                    order: lambdaAnswer.Order);

                return lambdaAnswerDto;
            });

        _answerDtoMapper.ToDTOs(Arg.Any<List<Answer>>()).Returns(
            (args) =>
            {
                return ((List<Answer>)args[0]).Select(answer => _answerDtoMapper.ToDTO(answer)).ToList();
            });

        _turnsRepository.Update(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                Turn lambdaTurn = (Turn)args[0];

                lambdaTurn = new Turn(
                    user: lambdaTurn.User,
                    round: lambdaTurn.Round,
                    startDateTime: lambdaTurn.StartDateTime,
                    endDateTime: DateTime.UtcNow);

                return Operation<Turn>.Success(result: lambdaTurn);
            });

        _roundWithCategoriesDtoMapper.ToDTO(Arg.Any<Round>()).Returns(
            (args) =>
            {
                Round round = (Round)args[0];

                RoundDto roundDto = new RoundDto(
                    id: round.Id,
                    roundNumber: round.RoundNumber,
                    initialLetter: round.InitialLetter,
                    isActive: round.IsActive,
                    matchId: round.Match.Id);

                List<CategoryDto> categoryDtos = round.Categories
                    .Select(category => new CategoryDto(id: category.Id, name: category.Name))
                    .ToList();

                return new RoundWithCategoriesDto(roundDto: roundDto, categoryDtos: categoryDtos);
            });

        _roundWithCategoriesDtoMapper.ToDTOs(Arg.Any<List<Round>>()).Returns(
            (args) =>
            {
                List<Round> rounds = (List<Round>)args[0];
                return rounds.Select(_roundWithCategoriesDtoMapper.ToDTO).ToList();
            });

        _turnDtoMapper.ToDTO(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                Turn turn = (Turn)args[0];

                TurnDto turnDto = new TurnDto(
                    userId: turn.User.Id,
                    roundId: turn.Round.Id,
                    points: turn.Points,
                    startDateTime: turn.StartDateTime,
                    endDateTime: turn.EndDateTime);

                return turnDto;
            });

        _matchDtoMapper.ToDTO(Arg.Any<Match>()).Returns(
            (args) =>
            {
                Match lambaMatch = (Match)args[0];
                MatchDto lambaMatchDto = new MatchDto(
                    id: lambaMatch.Id,
                    startDateTime: lambaMatch.StartDateTime,
                    endDateTime: lambaMatch.EndDateTime);
                return lambaMatchDto;
            });

        _userMatchDtoMapper.ToDTO(Arg.Any<UserMatch>()).Returns(
            (args) =>
            {
                UserMatch lambaUserMatch = (UserMatch)args[0];

                UserMatchDto lambaUserMatchDto = new UserMatchDto(
                    score: lambaUserMatch.Score,
                    isWinner: lambaUserMatch.IsWinner,
                    hasInitiative: lambaUserMatch.HasInitiative,
                    userId: lambaUserMatch.User.Id,
                    matchId: lambaUserMatch.Match.Id);

                return lambaUserMatchDto;
            });

        _answersRepository.Insert(Arg.Any<Answer>()).Returns(
            (args) =>
            {
                Answer answer = (Answer)args[0];
                return Operation<Answer>.Success(result: answer);
            });

        match = _matchesReadOnlyRepository.Get(matchId).Result;

        MatchDto matchDto = new MatchDto(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime);

        round = _roundsRepository.Get(matchId).Result;

        RoundDto roundDto = new RoundDto(
            id: round.Id,
            roundNumber: round.RoundNumber,
            initialLetter: round.InitialLetter,
            isActive: round.IsActive,
            matchId: round.Match.Id);

        UserMatch userWithInitiativeMatch = _userMatchesRepository.Get(
            userId: userWithInitiativeId, matchId: match.Id).Result;

        UserMatchDto userWithInitiativeMatchDto = new UserMatchDto(
            score: userWithInitiativeMatch.Score,
            isWinner: userWithInitiativeMatch.IsWinner,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            userId: userWithInitiativeMatch.User.Id,
            matchId: userWithInitiativeMatch.Match.Id);

        UserMatch userWithoutInitiativeMatch = _userMatchesRepository.Get(
            userId: userWithoutInitiativeId, matchId: match.Id).Result;

        UserMatchDto userWithoutIniciativeMatchDto = new UserMatchDto(
            score: userWithoutInitiativeMatch.Score,
            isWinner: userWithoutInitiativeMatch.IsWinner,
            hasInitiative: userWithoutInitiativeMatch.HasInitiative,
            userId: userWithoutInitiativeMatch.User.Id,
            matchId: userWithoutInitiativeMatch.Match.Id);

        RoundWithCategoriesDto roundWithCategoriesDto = new RoundWithCategoriesDto(
            roundDto: roundDto, categoryDtos: categoryDtos);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(
                userId: userWithInitiativeId,
                matchId: matchId,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        EndOfTurnDto expectedDto = new EndOfTurnDto(
            matchDto: matchDto,
            roundWithCategoriesDto: roundWithCategoriesDto,
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutIniciativeMatchDto,
            userWithInitiativeRoundDtos: new List<UserRoundDto>(),
            userWithoutInitiativeRoundDtos: new List<UserRoundDto>(),
            answerDtosOfUserWithInitiative: answerDtos.ToList(),
            answerDtosOfUserWithoutInitiative: new List<AnswerDto>());

        Assert.IsTrue(useCaseOperation.WasOk);
        Assert.AreEqual(expected: expectedDto, actual: useCaseOperation.Result);
        #endregion
    }

    [Test]
    public void Test_ok_for_user_with_initiative_outside_time_limit()
    {
        #region -- Arrange --
        int userWithInitiativeId = Configuration.TestUserId;
        int userWithoutInitiativeId = Configuration.TestBotId;
        int matchId = 0;
        int roundId = 0;
        Match match;
        Round round;

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                return Operation<User>.Success(result: new User(id: lambdaUserId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'j',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: new List<Round>() { round });

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                int lambdaMatchId = (int)args[1];

                UserMatch lambdaUserMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: lambdaUserId == userWithInitiativeId,
                    user: _usersReadOnlyRepository.Get(id: lambdaUserId).Result,
                    match: _matchesReadOnlyRepository.Get(lambdaMatchId).Result);

                return Operation<UserMatch>.Success(result: lambdaUserMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userWithInitiativeId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(userWithoutInitiativeId, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        List<Category> categories = new List<Category>
        {
            new Category(id: 0, name: ""),
            new Category(id: 1, name: ""),
            new Category(id: 2, name: ""),
            new Category(id: 3, name: ""),
            new Category(id: 4, name: "")
        };

        _roundsRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: categories);

                return Operation<Round>.Success(result: round);
            });

        _roundsRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> rounds = new List<Round>();
                Round activeRound = _roundsRepository.Get(roundId).Result;
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsRepository.Get(userWithInitiativeId, roundId).Returns(
            (args) =>
            {
                DateTime lamdaTurnStartDateTime =
                    DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSecondsPlusTolerance + 10);

                Turn lambdaTurn = new Turn(
                    user: _usersReadOnlyRepository.Get(id: userWithInitiativeId).Result,
                    round: _roundsRepository.Get(id: roundId).Result,
                    startDateTime: lamdaTurnStartDateTime);

                return Operation<Turn>.Success(result: lambdaTurn);
            });

        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        List<CategoryDto> categoryDtos = categories
            .Select((category, index) => new CategoryDto(id: categories[index].Id, name: categories[index].Name))
            .ToList();

        for (int i = 0; i < answerDtos.Length; i++)
        {
            answerDtos[i] = new AnswerDto(categoryDto: categoryDtos[i], userInput: "Something", order: i);
        }

        List<AnswerDto> emptyAnswerDtos = answerDtos
            .Select(answerDto => new AnswerDto(categoryDto: answerDto.CategoryDto, userInput: "", order: answerDto.Order))
            .ToList();

        _answerDtoMapper.ToDTO(Arg.Any<Answer>()).Returns(
            (args) =>
            {
                Answer lambdaAnswer = (Answer)args[0];

                AnswerDto lambdaAnswerDto = new AnswerDto(
                    categoryDto: categoryDtos.Single(categoryDto => categoryDto.Id == lambdaAnswer.Category.Id),
                    userInput: lambdaAnswer.UserInput,
                    order: lambdaAnswer.Order);

                return lambdaAnswerDto;
            });

        _answerDtoMapper.ToDTOs(Arg.Any<List<Answer>>()).Returns(
            (args) =>
            {
                return ((List<Answer>)args[0]).Select(answer => _answerDtoMapper.ToDTO(answer)).ToList();
            });

        _wordsRepository.Exists(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<char>()).Returns(
            (args) =>
            {
                string text = (string)args[0];
                char initialLetter = (char)args[2];
                string other = $"{initialLetter} TEST".ToLower();
                bool result = text.ToLower() == other;
                return Operation<bool>.Success(result: result);
            });

        _turnsRepository.Update(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                Turn lambdaTurn = (Turn)args[0];

                List<Answer> lambdaEmptyAnswers = emptyAnswerDtos
                .Select(
                    answerDto =>
                    new Answer(
                        userInput: "",
                        order: answerDto.Order,
                        category: categories.Single(category => category.Id == answerDto.CategoryDto.Id),
                        turn: lambdaTurn))
                .ToList();

                lambdaTurn = new Turn(
                    user: lambdaTurn.User,
                    round: lambdaTurn.Round,
                    startDateTime: lambdaTurn.StartDateTime,
                    endDateTime: DateTime.UtcNow,
                    answers: lambdaEmptyAnswers,
                    wordsRepository: _wordsRepository);

                return Operation<Turn>.Success(result: lambdaTurn);
            });

        _roundWithCategoriesDtoMapper.ToDTO(Arg.Any<Round>()).Returns(
            (args) =>
            {
                Round round = (Round)args[0];

                RoundDto roundDto = new RoundDto(
                    id: round.Id,
                    roundNumber: round.RoundNumber,
                    initialLetter: round.InitialLetter,
                    isActive: round.IsActive,
                    matchId: round.Match.Id);

                List<CategoryDto> categoryDtos = round.Categories
                    .Select(category => new CategoryDto(id: category.Id, name: category.Name))
                    .ToList();

                return new RoundWithCategoriesDto(roundDto: roundDto, categoryDtos: categoryDtos);
            });

        _roundWithCategoriesDtoMapper.ToDTOs(Arg.Any<List<Round>>()).Returns(
            (args) =>
            {
                List<Round> rounds = (List<Round>)args[0];
                return rounds.Select(_roundWithCategoriesDtoMapper.ToDTO).ToList();
            });

        _turnDtoMapper.ToDTO(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                Turn turn = (Turn)args[0];

                TurnDto turnDto = new TurnDto(
                    userId: turn.User.Id,
                    roundId: turn.Round.Id,
                    points: turn.Points,
                    startDateTime: turn.StartDateTime,
                    endDateTime: turn.EndDateTime);

                return turnDto;
            });

        _matchDtoMapper.ToDTO(Arg.Any<Match>()).Returns(
            (args) =>
            {
                Match lambaMatch = (Match)args[0];
                MatchDto lambaMatchDto = new MatchDto(
                    id: lambaMatch.Id,
                    startDateTime: lambaMatch.StartDateTime,
                    endDateTime: lambaMatch.EndDateTime);
                return lambaMatchDto;
            });

        _userMatchDtoMapper.ToDTO(Arg.Any<UserMatch>()).Returns(
            (args) =>
            {
                UserMatch lambaUserMatch = (UserMatch)args[0];

                UserMatchDto lambaUserMatchDto = new UserMatchDto(
                    score: lambaUserMatch.Score,
                    isWinner: lambaUserMatch.IsWinner,
                    hasInitiative: lambaUserMatch.HasInitiative,
                    userId: lambaUserMatch.User.Id,
                    matchId: lambaUserMatch.Match.Id);

                return lambaUserMatchDto;
            });

        _answersRepository.Insert(Arg.Any<Answer>()).Returns(
            (args) =>
            {
                Answer answer = (Answer)args[0];
                return Operation<Answer>.Success(result: answer);
            });

        match = _matchesReadOnlyRepository.Get(matchId).Result;

        MatchDto matchDto = new MatchDto(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime);

        round = _roundsRepository.Get(matchId).Result;

        RoundDto roundDto = new RoundDto(
            id: round.Id,
            roundNumber: round.RoundNumber,
            initialLetter: round.InitialLetter,
            isActive: round.IsActive,
            matchId: round.Match.Id);

        UserMatch userWithInitiativeMatch = _userMatchesRepository.Get(
            userId: userWithInitiativeId, matchId: match.Id).Result;

        UserMatchDto userWithInitiativeMatchDto = new UserMatchDto(
            score: userWithInitiativeMatch.Score,
            isWinner: userWithInitiativeMatch.IsWinner,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            userId: userWithInitiativeMatch.User.Id,
            matchId: userWithInitiativeMatch.Match.Id);

        UserMatch userWithoutInitiativeMatch = _userMatchesRepository.Get(
            userId: userWithoutInitiativeId, matchId: match.Id).Result;

        UserMatchDto userWithoutIniciativeMatchDto = new UserMatchDto(
            score: userWithoutInitiativeMatch.Score,
            isWinner: userWithoutInitiativeMatch.IsWinner,
            hasInitiative: userWithoutInitiativeMatch.HasInitiative,
            userId: userWithoutInitiativeMatch.User.Id,
            matchId: userWithoutInitiativeMatch.Match.Id);

        RoundWithCategoriesDto roundWithCategoriesDto = new RoundWithCategoriesDto(
            roundDto: roundDto, categoryDtos: categoryDtos);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(
                userId: userWithInitiativeId,
                matchId: matchId,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        EndOfTurnDto expectedDto = new EndOfTurnDto(
            matchDto: matchDto,
            roundWithCategoriesDto: roundWithCategoriesDto,
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutIniciativeMatchDto,
            userWithInitiativeRoundDtos: new List<UserRoundDto>(),
            userWithoutInitiativeRoundDtos: new List<UserRoundDto>(),
            answerDtosOfUserWithInitiative: emptyAnswerDtos,
            answerDtosOfUserWithoutInitiative: new List<AnswerDto>());

        Assert.IsTrue(useCaseOperation.WasOk);
        Assert.AreEqual(expected: expectedDto, actual: useCaseOperation.Result);
        #endregion
    }

    [Test]
    public void Test_ok_for_user_without_initiative_end_of_non_final_round()
    {
        #region -- Arrange --
        int userWithInitiativeId = Configuration.TestUserId;
        int userWithoutInitiativeId = Configuration.TestBotId;
        int matchId = 0;
        int roundId = 0;
        Match match;
        Round round;

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                return Operation<User>.Success(result: new User(id: lambdaUserId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'j',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: new List<Round>() { round });

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                int lambdaMatchId = (int)args[1];

                UserMatch lambdaUserMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: lambdaUserId == userWithInitiativeId,
                    user: _usersReadOnlyRepository.Get(id: lambdaUserId).Result,
                    match: _matchesReadOnlyRepository.Get(lambdaMatchId).Result);

                return Operation<UserMatch>.Success(result: lambdaUserMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userWithInitiativeId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(userWithoutInitiativeId, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        List<Category> categories = new List<Category>
        {
            new Category(id: 0, name: ""),
            new Category(id: 1, name: ""),
            new Category(id: 2, name: ""),
            new Category(id: 3, name: ""),
            new Category(id: 4, name: "")
        };

        _roundsRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: categories);

                return Operation<Round>.Success(result: round);
            });

        _roundsRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> rounds = new List<Round>();
                Round activeRound = _roundsRepository.Get(roundId).Result;
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(result: rounds);
            });

        _roundsRepository.Update(Arg.Any<Round>()).Returns(
            (args) =>
            {
                Round lambdaRound = (Round)args[0];

                Round updatedLambdaRound = new Round(
                    id: roundId,
                    roundNumber: lambdaRound.RoundNumber,
                    initialLetter: lambdaRound.InitialLetter,
                    isActive: lambdaRound.IsActive,
                    match: lambdaRound.Match,
                    categories: lambdaRound.Categories);

                return Operation<Round>.Success(result: updatedLambdaRound);
            });

        _turnsRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambaUserId = (int)args[0];

                if (lambaUserId == userWithInitiativeId)
                {
                    DateTime lamdaTurnStartDateTime =
                        DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds) * 2;

                    Turn lambdaTurn = new Turn(
                        user: _usersReadOnlyRepository.Get(id: userWithInitiativeId).Result,
                        round: _roundsRepository.Get(id: roundId).Result,
                        startDateTime: lamdaTurnStartDateTime,
                        endDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds),
                        points: 3);

                    return Operation<Turn>.Success(result: lambdaTurn);
                }
                else
                {
                    DateTime lamdaTurnStartDateTime =
                        DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds);

                    Turn lambdaTurn = new Turn(
                        user: _usersReadOnlyRepository.Get(id: userWithoutInitiativeId).Result,
                        round: _roundsRepository.Get(id: roundId).Result,
                        startDateTime: lamdaTurnStartDateTime);

                    return Operation<Turn>.Success(result: lambdaTurn);
                }
            });

        _wordsRepository.Exists(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<char>()).Returns(
            (args) =>
            {
                string text = (string)args[0];
                char initialLetter = (char)args[2];
                string other = $"{initialLetter} TEST".ToLower();
                bool result = text.ToLower() == other;
                return Operation<bool>.Success(result: result);
            });

        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        List<CategoryDto> categoryDtos = categories
            .Select((category, index) => new CategoryDto(id: categories[index].Id, name: categories[index].Name))
            .ToList();

        for (int i = 0; i < 3; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: categoryDtos[i],
                userInput: $"{_roundsRepository.Get(roundId).Result.InitialLetter} TEST",
                order: i);
        }

        for (int i = 3; i < answerDtos.Length; i++)
        {
            answerDtos[i] = new AnswerDto(categoryDto: categoryDtos[i], userInput: "Something", order: i);
        }

        _answerDtoMapper.ToDTO(Arg.Any<Answer>()).Returns(
            (args) =>
            {
                Answer lambdaAnswer = (Answer)args[0];

                AnswerDto lambdaAnswerDto = new AnswerDto(
                    categoryDto: categoryDtos.Single(categoryDto => categoryDto.Id == lambdaAnswer.Category.Id),
                    userInput: lambdaAnswer.UserInput,
                    order: lambdaAnswer.Order);

                return lambdaAnswerDto;
            });

        _answerDtoMapper.ToDTOs(Arg.Any<List<Answer>>()).Returns(
            (args) =>
            {
                return ((List<Answer>)args[0]).Select(answer => _answerDtoMapper.ToDTO(answer)).ToList();
            });

        _turnsRepository.Update(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                DateTime lamdaTurnStartDateTime =
                        DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds);

                Turn lambdaTurn = new Turn(
                        user: _usersReadOnlyRepository.Get(id: userWithoutInitiativeId).Result,
                        round: _roundsRepository.Get(id: roundId).Result,
                        startDateTime: lamdaTurnStartDateTime,
                        endDateTime: DateTime.UtcNow,
                        points: 3);

                return Operation<Turn>.Success(result: lambdaTurn);
            });

        _roundWithCategoriesDtoMapper.ToDTO(Arg.Any<Round>()).Returns(
            (args) =>
            {
                Round round = (Round)args[0];

                RoundDto roundDto = new RoundDto(
                    id: round.Id,
                    roundNumber: round.RoundNumber,
                    initialLetter: round.InitialLetter,
                    isActive: round.IsActive,
                    matchId: round.Match.Id);

                List<CategoryDto> categoryDtos = round.Categories
                    .Select(category => new CategoryDto(id: category.Id, name: category.Name))
                    .ToList();

                return new RoundWithCategoriesDto(roundDto: roundDto, categoryDtos: categoryDtos);
            });

        _roundWithCategoriesDtoMapper.ToDTOs(Arg.Any<List<Round>>()).Returns(
            (args) =>
            {
                List<Round> rounds = (List<Round>)args[0];
                return rounds.Select(_roundWithCategoriesDtoMapper.ToDTO).ToList();
            });

        _turnDtoMapper.ToDTO(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                Turn turn = (Turn)args[0];

                TurnDto turnDto = new TurnDto(
                    userId: turn.User.Id,
                    roundId: turn.Round.Id,
                    points: turn.Points,
                    startDateTime: turn.StartDateTime,
                    endDateTime: turn.EndDateTime);

                return turnDto;
            });

        _matchDtoMapper.ToDTO(Arg.Any<Match>()).Returns(
            (args) =>
            {
                Match lambaMatch = (Match)args[0];
                MatchDto lambaMatchDto = new MatchDto(
                    id: lambaMatch.Id,
                    startDateTime: lambaMatch.StartDateTime,
                    endDateTime: lambaMatch.EndDateTime);
                return lambaMatchDto;
            });

        _userMatchDtoMapper.ToDTO(Arg.Any<UserMatch>()).Returns(
            (args) =>
            {
                UserMatch lambaUserMatch = (UserMatch)args[0];

                UserMatchDto lambaUserMatchDto = new UserMatchDto(
                    score: lambaUserMatch.Score,
                    isWinner: lambaUserMatch.IsWinner,
                    hasInitiative: lambaUserMatch.HasInitiative,
                    userId: lambaUserMatch.User.Id,
                    matchId: lambaUserMatch.Match.Id);

                return lambaUserMatchDto;
            });

        _answersRepository.GetMany(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];

                if(userWithInitiativeId == lambdaUserId)
                {
                    List<Answer> answers = new List<Answer>();

                    foreach (AnswerDto answerDto in answerDtos)
                    {
                        Answer answer = new Answer(
                            userInput: answerDto.UserInput,
                            order: answerDto.Order,
                            category: categories.Single(category => category.Id == answerDto.CategoryDto.Id),
                            turn: _turnsRepository.Get(userId: userWithInitiativeId, roundId: roundId).Result);

                        answers.Add(answer);
                    }

                    return Operation<List<Answer>>.Success(result: answers);
                }
                
                throw new NotImplementedException();
            });

        _answersRepository.Insert(Arg.Any<Answer>()).Returns(
            (args) =>
            {
                Answer answer = (Answer)args[0];
                return Operation<Answer>.Success(result: answer);
            });

        _userRoundsRepository.Insert(Arg.Any<UserRound>()).Returns(
            (args) =>
            {
                UserRound lambdaUserRound = (UserRound)args[0];
                return Operation<UserRound>.Success(result: lambdaUserRound);
            });

        _userRoundsRepository.GetMany(Arg.Any<int>(), Arg.Any<List<int>>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                List<UserRound> lambdaUserRounds = new List<UserRound>();

                if (lambdaUserId == userWithInitiativeId)
                {
                    UserRound lambdaUserRound = new UserRound(
                        user: _usersReadOnlyRepository.Get(id: userWithInitiativeId).Result,
                        round: _roundsRepository.Get(id: roundId).Result,
                        isWinner: true,
                        points: 3);

                    lambdaUserRounds.Add(lambdaUserRound);
                }
                else if(lambdaUserId == userWithoutInitiativeId)
                {
                    UserRound lambdaUserRound = new UserRound(
                        user: _usersReadOnlyRepository.Get(id: userWithoutInitiativeId).Result,
                        round: _roundsRepository.Get(id: roundId).Result,
                        isWinner: true,
                        points: 3);

                    lambdaUserRounds.Add(lambdaUserRound);
                }
                return Operation<List<UserRound>>.Success(result: lambdaUserRounds);
            });

        _userRoundDtoMapper.ToDTOs(Arg.Any<List<UserRound>>()).Returns(
            (args) =>
            {
                List<UserRound> userRounds = (List<UserRound>)args[0];
                List<UserRoundDto> userRoundDtos = new List<UserRoundDto>();

                foreach(UserRound userRound in userRounds)
                {
                    UserRoundDto userRoundDto = new UserRoundDto(
                        userId: userRound.User.Id,
                        roundId: userRound.Round.Id,
                        isWinner: userRound.IsWinner,
                        points: userRound.Points);

                    userRoundDtos.Add(userRoundDto);
                }

                return userRoundDtos;
            });

        match = _matchesReadOnlyRepository.Get(matchId).Result;

        MatchDto matchDto = new MatchDto(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime);

        round = _roundsRepository.Get(matchId).Result;

        RoundDto roundDto = new RoundDto(
            id: round.Id,
            roundNumber: round.RoundNumber,
            initialLetter: round.InitialLetter,
            isActive: false,
            matchId: round.Match.Id);

        UserMatch userWithInitiativeMatch = _userMatchesRepository.Get(
            userId: userWithInitiativeId, matchId: match.Id).Result;

        UserMatchDto userWithInitiativeMatchDto = new UserMatchDto(
            score: userWithInitiativeMatch.Score,
            isWinner: userWithInitiativeMatch.IsWinner,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            userId: userWithInitiativeMatch.User.Id,
            matchId: userWithInitiativeMatch.Match.Id);

        UserMatch userWithoutInitiativeMatch = _userMatchesRepository.Get(
            userId: userWithoutInitiativeId, matchId: match.Id).Result;

        UserMatchDto userWithoutIniciativeMatchDto = new UserMatchDto(
            score: userWithoutInitiativeMatch.Score,
            isWinner: userWithoutInitiativeMatch.IsWinner,
            hasInitiative: userWithoutInitiativeMatch.HasInitiative,
            userId: userWithoutInitiativeMatch.User.Id,
            matchId: userWithoutInitiativeMatch.Match.Id);

        RoundWithCategoriesDto roundWithCategoriesDto = new RoundWithCategoriesDto(
            roundDto: roundDto, categoryDtos: categoryDtos);

        Turn turn = _turnsRepository.Get(userWithInitiativeId, roundId).Result;

        TurnDto userWithInitiativeTurnDto = new TurnDto(
            userId: turn.User.Id,
            roundId: turn.Round.Id,
            points: turn.Points,
            startDateTime: turn.StartDateTime,
            endDateTime: turn.EndDateTime);

        UserRoundDto userWithInitiativeRoundDto = new UserRoundDto(
            userId: userWithInitiativeId,
            roundId: roundId,
            isWinner: true,
            points: 3);

        turn = _turnsRepository.Get(userWithoutInitiativeId, roundId).Result;

        TurnDto userWithoutInitiativeTurnDto = new TurnDto(
            userId: turn.User.Id,
            roundId: turn.Round.Id,
            points: turn.Points,
            startDateTime: turn.StartDateTime,
            endDateTime: turn.EndDateTime);

        UserRoundDto userWithoutInitiativeRoundDto = new UserRoundDto(
            userId: userWithoutInitiativeId,
            roundId: roundId,
            isWinner: true,
            points: 3);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(
                userId: userWithoutInitiativeId,
                matchId: matchId,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        EndOfTurnDto expectedDto = new EndOfTurnDto(
            matchDto: matchDto,
            roundWithCategoriesDto: roundWithCategoriesDto,
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutIniciativeMatchDto,
            userWithInitiativeRoundDtos: new List<UserRoundDto>() { userWithInitiativeRoundDto },
            userWithoutInitiativeRoundDtos: new List<UserRoundDto>() { userWithoutInitiativeRoundDto },
            answerDtosOfUserWithInitiative: answerDtos.ToList(),
            answerDtosOfUserWithoutInitiative: answerDtos.ToList());

        Assert.IsTrue(useCaseOperation.WasOk);
        Assert.AreEqual(expected: expectedDto, actual: useCaseOperation.Result);
        #endregion
    }

    [Test]
    public void Test_ok_for_user_without_initiative_end_of_final_round_and_match_win()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_initiative_end_of_final_round_and_match_lose()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_initiative_end_of_final_round_and_match_draw()
    {
        #region -- Arrange --
        int userWithInitiativeId = Configuration.TestUserId;
        int userWithoutInitiativeId = Configuration.TestBotId;
        int matchId = 0;
        List<int> roundIds = new List<int>() { 0, 1, 2 };
        int activeRoundId = roundIds.Last();
        Match match;
        Round round;
        char initialLetter = 'f';

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                return Operation<User>.Success(result: new User(id: lambdaUserId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                List<Round> lambdaRounds = new List<Round>();

                for(int i = 0; i < roundIds.Count; i++)
                {
                    Round lambdaRound = new Round(
                        id: roundIds[i],
                        roundNumber: i,
                        initialLetter: initialLetter,
                        isActive: i == roundIds.Count - 1,
                        match: match,
                        categories: new List<Category>());

                    lambdaRounds.Add(lambdaRound);
                }

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: lambdaRounds);

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                int lambdaMatchId = (int)args[1];

                UserMatch lambdaUserMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: lambdaUserId == userWithInitiativeId,
                    user: _usersReadOnlyRepository.Get(id: lambdaUserId).Result,
                    match: _matchesReadOnlyRepository.Get(lambdaMatchId).Result);

                return Operation<UserMatch>.Success(result: lambdaUserMatch);
            });

        _userMatchesRepository.Update(Arg.Any<UserMatch>()).Returns(
            (args) =>
            {
                UserMatch inputLambdaUserMatch = (UserMatch)args[0];

                UserMatch outputLambdaUserMatch = new UserMatch(
                    score: inputLambdaUserMatch.Score,
                    isWinner: true,
                    hasInitiative: inputLambdaUserMatch.HasInitiative,
                    user: inputLambdaUserMatch.User,
                    match: inputLambdaUserMatch.Match);

                return Operation<UserMatch>.Success(result: outputLambdaUserMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userWithInitiativeId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(userWithoutInitiativeId, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        List<Category> categories = new List<Category>
        {
            new Category(id: 0, name: ""),
            new Category(id: 1, name: ""),
            new Category(id: 2, name: ""),
            new Category(id: 3, name: ""),
            new Category(id: 4, name: "")
        };

        _roundsRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaRoundId = (int)args[0];

                Round round = new Round(
                    id: lambdaRoundId,
                    roundNumber: lambdaRoundId,
                    initialLetter: initialLetter,
                    isActive: lambdaRoundId == activeRoundId,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: categories);

                return Operation<Round>.Success(result: round);
            });

        _roundsRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> lambdaRounds = new List<Round>();
                foreach (int roundId in roundIds)
                {
                    Round lambdaRound = _roundsRepository.Get(roundId).Result;
                    lambdaRounds.Add(lambdaRound);
                }
                return Operation<List<Round>>.Success(result: lambdaRounds);
            });

        _roundsRepository.Update(Arg.Any<Round>()).Returns(
            (args) =>
            {
                Round lambdaRound = (Round)args[0];

                Round updatedLambdaRound = new Round(
                    id: lambdaRound.Id,
                    roundNumber: lambdaRound.RoundNumber,
                    initialLetter: lambdaRound.InitialLetter,
                    isActive: false,
                    match: lambdaRound.Match,
                    categories: lambdaRound.Categories);

                return Operation<Round>.Success(result: updatedLambdaRound);
            });

        _turnsRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambaUserId = (int)args[0];

                if (lambaUserId == userWithInitiativeId)
                {
                    DateTime lamdaTurnStartDateTime =
                        DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds) * 2;

                    Turn lambdaTurn = new Turn(
                        user: _usersReadOnlyRepository.Get(id: userWithInitiativeId).Result,
                        round: _roundsRepository.Get(id: activeRoundId).Result,
                        startDateTime: lamdaTurnStartDateTime,
                        endDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds),
                        points: 3);

                    return Operation<Turn>.Success(result: lambdaTurn);
                }
                else
                {
                    DateTime lamdaTurnStartDateTime =
                        DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds);

                    Turn lambdaTurn = new Turn(
                        user: _usersReadOnlyRepository.Get(id: userWithoutInitiativeId).Result,
                        round: _roundsRepository.Get(id: activeRoundId).Result,
                        startDateTime: lamdaTurnStartDateTime);

                    return Operation<Turn>.Success(result: lambdaTurn);
                }
            });

        _wordsRepository.Exists(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<char>()).Returns(
            (args) =>
            {
                string text = (string)args[0];
                char initialLetter = (char)args[2];
                string other = $"{initialLetter} TEST".ToLower();
                bool result = text.ToLower() == other;
                return Operation<bool>.Success(result: result);
            });

        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        List<CategoryDto> categoryDtos = categories
            .Select((category, index) => new CategoryDto(id: categories[index].Id, name: categories[index].Name))
            .ToList();

        for (int i = 0; i < 3; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: categoryDtos[i],
                userInput: $"{_roundsRepository.Get(activeRoundId).Result.InitialLetter} TEST",
                order: i);
        }

        for (int i = 3; i < answerDtos.Length; i++)
        {
            answerDtos[i] = new AnswerDto(categoryDto: categoryDtos[i], userInput: "WRONG ANSWER", order: i);
        }

        _answerDtoMapper.ToDTO(Arg.Any<Answer>()).Returns(
            (args) =>
            {
                Answer lambdaAnswer = (Answer)args[0];

                AnswerDto lambdaAnswerDto = new AnswerDto(
                    categoryDto: categoryDtos.Single(categoryDto => categoryDto.Id == lambdaAnswer.Category.Id),
                    userInput: lambdaAnswer.UserInput,
                    order: lambdaAnswer.Order);

                return lambdaAnswerDto;
            });

        _answerDtoMapper.ToDTOs(Arg.Any<List<Answer>>()).Returns(
            (args) =>
            {
                return ((List<Answer>)args[0]).Select(answer => _answerDtoMapper.ToDTO(answer)).ToList();
            });

        _turnsRepository.Update(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                DateTime lamdaTurnStartDateTime =
                        DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds);

                Turn lambdaTurn = new Turn(
                        user: _usersReadOnlyRepository.Get(id: userWithoutInitiativeId).Result,
                        round: _roundsRepository.Get(id: activeRoundId).Result,
                        startDateTime: lamdaTurnStartDateTime,
                        endDateTime: DateTime.UtcNow,
                        points: 3);

                return Operation<Turn>.Success(result: lambdaTurn);
            });

        _roundWithCategoriesDtoMapper.ToDTO(Arg.Any<Round>()).Returns(
            (args) =>
            {
                Round round = (Round)args[0];

                RoundDto roundDto = new RoundDto(
                    id: round.Id,
                    roundNumber: round.RoundNumber,
                    initialLetter: round.InitialLetter,
                    isActive: round.IsActive,
                    matchId: round.Match.Id);

                List<CategoryDto> categoryDtos = round.Categories
                    .Select(category => new CategoryDto(id: category.Id, name: category.Name))
                    .ToList();

                return new RoundWithCategoriesDto(roundDto: roundDto, categoryDtos: categoryDtos);
            });

        _roundWithCategoriesDtoMapper.ToDTOs(Arg.Any<List<Round>>()).Returns(
            (args) =>
            {
                List<Round> rounds = (List<Round>)args[0];
                return rounds.Select(_roundWithCategoriesDtoMapper.ToDTO).ToList();
            });

        _turnDtoMapper.ToDTO(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                Turn turn = (Turn)args[0];

                TurnDto turnDto = new TurnDto(
                    userId: turn.User.Id,
                    roundId: turn.Round.Id,
                    points: turn.Points,
                    startDateTime: turn.StartDateTime,
                    endDateTime: turn.EndDateTime);

                return turnDto;
            });

        _matchDtoMapper.ToDTO(Arg.Any<Match>()).Returns(
            (args) =>
            {
                Match lambaMatch = (Match)args[0];
                MatchDto lambaMatchDto = new MatchDto(
                    id: lambaMatch.Id,
                    startDateTime: lambaMatch.StartDateTime,
                    endDateTime: lambaMatch.EndDateTime);
                return lambaMatchDto;
            });

        _userMatchDtoMapper.ToDTO(Arg.Any<UserMatch>()).Returns(
            (args) =>
            {
                UserMatch lambaUserMatch = (UserMatch)args[0];

                UserMatchDto lambaUserMatchDto = new UserMatchDto(
                    score: lambaUserMatch.Score,
                    isWinner: lambaUserMatch.IsWinner,
                    hasInitiative: lambaUserMatch.HasInitiative,
                    userId: lambaUserMatch.User.Id,
                    matchId: lambaUserMatch.Match.Id);

                return lambaUserMatchDto;
            });

        _userMatchDtoMapper.FromDTO(Arg.Any<UserMatchDto>()).Returns(
            (args) =>
            {
                UserMatchDto lambdaUserMatchDto = (UserMatchDto)args[0];

                UserMatch lambdaUserMatch = new UserMatch(
                    score: lambdaUserMatchDto.Score,
                    isWinner: lambdaUserMatchDto.IsWinner,
                    hasInitiative: lambdaUserMatchDto.HasInitiative,
                    user: _usersReadOnlyRepository.Get(id: lambdaUserMatchDto.UserId).Result,
                    match: _matchesReadOnlyRepository.Get(id: lambdaUserMatchDto.MatchId).Result);

                return lambdaUserMatch;
            });

        _answersRepository.GetMany(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                int lambdaRound = (int)args[1];

                List<Answer> answers = new List<Answer>();

                foreach (AnswerDto answerDto in answerDtos)
                {
                    Answer answer = new Answer(
                        userInput: answerDto.UserInput,
                        order: answerDto.Order,
                        category: categories.Single(category => category.Id == answerDto.CategoryDto.Id),
                        turn: _turnsRepository.Get(userId: userWithInitiativeId, roundId: lambdaRound).Result);

                    answers.Add(answer);
                }

                return Operation<List<Answer>>.Success(result: answers);
            });

        _answersRepository.Insert(Arg.Any<Answer>()).Returns(
            (args) =>
            {
                Answer answer = (Answer)args[0];
                return Operation<Answer>.Success(result: answer);
            });

        _userRoundsRepository.Insert(Arg.Any<UserRound>()).Returns(
            (args) =>
            {
                UserRound lambdaUserRound = (UserRound)args[0];
                return Operation<UserRound>.Success(result: lambdaUserRound);
            });

        _userRoundsRepository.GetMany(Arg.Any<int>(), Arg.Any<List<int>>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                List<UserRound> lambdaUserRounds = new List<UserRound>();

                foreach(int roundId in roundIds)
                {
                    UserRound lambdaUserRound = new UserRound(
                        user: _usersReadOnlyRepository.Get(id: lambdaUserId).Result,
                        round: _roundsRepository.Get(id: roundId).Result,
                        isWinner: true,
                        points: 3);

                    lambdaUserRounds.Add(lambdaUserRound);
                }

                return Operation<List<UserRound>>.Success(result: lambdaUserRounds);
            });

        _userRoundDtoMapper.ToDTOs(Arg.Any<List<UserRound>>()).Returns(
            (args) =>
            {
                List<UserRound> userRounds = (List<UserRound>)args[0];
                List<UserRoundDto> userRoundDtos = new List<UserRoundDto>();

                foreach (UserRound userRound in userRounds)
                {
                    UserRoundDto userRoundDto = new UserRoundDto(
                        userId: userRound.User.Id,
                        roundId: userRound.Round.Id,
                        isWinner: userRound.IsWinner,
                        points: userRound.Points);

                    userRoundDtos.Add(userRoundDto);
                }

                return userRoundDtos;
            });

        match = _matchesReadOnlyRepository.Get(matchId).Result;

        MatchDto matchDto = new MatchDto(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime);

        round = _roundsRepository.Get(activeRoundId).Result;

        RoundDto roundDto = new RoundDto(
            id: round.Id,
            roundNumber: round.RoundNumber,
            initialLetter: round.InitialLetter,
            isActive: false,
            matchId: round.Match.Id);

        UserMatch userWithInitiativeMatch = _userMatchesRepository.Get(
            userId: userWithInitiativeId, matchId: match.Id).Result;

        UserMatchDto userWithInitiativeMatchDto = new UserMatchDto(
            score: Configuration.RoundsPerMatch,
            isWinner: true,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            userId: userWithInitiativeMatch.User.Id,
            matchId: userWithInitiativeMatch.Match.Id);

        UserMatch userWithoutInitiativeMatch = _userMatchesRepository.Get(
            userId: userWithoutInitiativeId, matchId: match.Id).Result;

        UserMatchDto userWithoutInitiativeMatchDto = new UserMatchDto(
            score: Configuration.RoundsPerMatch,
            isWinner: true,
            hasInitiative: userWithoutInitiativeMatch.HasInitiative,
            userId: userWithoutInitiativeMatch.User.Id,
            matchId: userWithoutInitiativeMatch.Match.Id);

        RoundWithCategoriesDto roundWithCategoriesDto = new RoundWithCategoriesDto(
            roundDto: roundDto, categoryDtos: categoryDtos);

        Turn turn = _turnsRepository.Get(userWithInitiativeId, activeRoundId).Result;

        TurnDto userWithInitiativeTurnDto = new TurnDto(
            userId: turn.User.Id,
            roundId: turn.Round.Id,
            points: turn.Points,
            startDateTime: turn.StartDateTime,
            endDateTime: turn.EndDateTime);

        List<UserRoundDto> userWithInitiativeRoundDtos = new List<UserRoundDto>();

        foreach(int roundId in roundIds)
        {
            UserRoundDto userWithInitiativeRoundDto = new UserRoundDto(
            userId: userWithInitiativeId,
            roundId: roundId,
            isWinner: true,
            points: 3);

            userWithInitiativeRoundDtos.Add(userWithInitiativeRoundDto);
        }

        turn = _turnsRepository.Get(userWithoutInitiativeId, activeRoundId).Result;

        TurnDto userWithoutInitiativeTurnDto = new TurnDto(
            userId: turn.User.Id,
            roundId: turn.Round.Id,
            points: turn.Points,
            startDateTime: turn.StartDateTime,
            endDateTime: turn.EndDateTime);

        List<UserRoundDto> userWithoutInitiativeRoundDtos = new List<UserRoundDto>();

        foreach (int roundId in roundIds)
        {
            UserRoundDto userWithoutInitiativeRoundDto = new UserRoundDto(
            userId: userWithoutInitiativeId,
            roundId: roundId,
            isWinner: true,
            points: 3);

            userWithoutInitiativeRoundDtos.Add(userWithoutInitiativeRoundDto);
        }
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(
                userId: userWithoutInitiativeId,
                matchId: matchId,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        EndOfTurnDto expectedDto = new EndOfTurnDto(
            matchDto: matchDto,
            roundWithCategoriesDto: roundWithCategoriesDto,
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutInitiativeMatchDto,
            userWithInitiativeRoundDtos: userWithInitiativeRoundDtos,
            userWithoutInitiativeRoundDtos: userWithoutInitiativeRoundDtos,
            answerDtosOfUserWithInitiative: answerDtos.ToList(),
            answerDtosOfUserWithoutInitiative: answerDtos.ToList());

        Assert.IsTrue(useCaseOperation.WasOk);
        Assert.AreEqual(expected: expectedDto, actual: useCaseOperation.Result);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_user()
    {
        #region -- Arrange --
        int userId = -1;
        int matchId = -1;

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Failure(errorMessage: $"User not found with id: {userId}");
            });
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"User not found with id: {userId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_match()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        int matchId = -1;

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                return Operation<Match>.Failure(errorMessage: $"Match not found with id: {matchId}");
            });
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Match not found with id: {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_user_not_in_match()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        int matchId = 0;

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int lambdaUserId = (int)args[0];
                return Operation<User>.Success(result: new User(id: lambdaUserId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                return Operation<Match>.Success(result: new Match(id: matchId, startDateTime: DateTime.UtcNow));
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                userId = userId > 0 ? userId * -1 : userId;

                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: false,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[Configuration.PlayersPerMatch]
                {
                    _userMatchesRepository.Get(userId: -1, matchId: matchId).Result,
                    _userMatchesRepository.Get(userId: -2, matchId: matchId).Result
                };
                return Operation<UserMatch[]>.Success(result: userMatches);
            });
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"User with id {userId} is not involved in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_turn()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        int matchId = 0;
        int roundId = 0;
        Match match;
        Round round;

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'j',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: new List<Round>() { round } );

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(userId, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: true,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.Get(-1, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: false,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(-1, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        _roundsRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: new List<Category>());

                return Operation<Round>.Success(result: round);
            });

        _roundsRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> rounds = new List<Round>();
                Round activeRound = _roundsRepository.Get(roundId).Result;
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsRepository.Get(userId, roundId).Returns(
            (args) =>
            {
                string errorMessage =
                    $"Turn not found for user with id {userId} " +
                    $"in round with id {roundId} in match with id {matchId}";

                return Operation<Turn>.Failure(errorMessage: errorMessage);
            });
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn not found for user with id {userId} in round with id {roundId} in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_already_ended_turn()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        int matchId = 0;
        int roundId = 0;
        Match match;
        Round round;

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'j',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: new List<Round>() { round });

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(userId, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: true,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.Get(-1, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: false,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(-1, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        _roundsRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: new List<Category>());

                return Operation<Round>.Success(result: round);
            });

        _roundsRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> rounds = new List<Round>();
                Round activeRound = _roundsRepository.Get(roundId).Result;
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsRepository.Get(userId, roundId).Returns(
            (args) =>
            {
                Turn turn = new Turn(
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    round: _roundsRepository.Get(id: roundId).Result,
                    startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds),
                    endDateTime: DateTime.UtcNow);

                return Operation<Turn>.Success(result: turn);
            });
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn already ended for user with id {userId} " +
                    $"in round with id {roundId} in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_too_many_or_too_few_answers()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        int matchId = 0;
        int roundId = 0;
        Match match;
        Round round;

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'j',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: new List<Round>() { round });

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(userId, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: true,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.Get(-1, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: false,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(-1, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        List<Category> categories = new List<Category>();
        categories.Add(new Category(id: 0, name: ""));
        categories.Add(new Category(id: 1, name: ""));
        categories.Add(new Category(id: 2, name: ""));
        categories.Add(new Category(id: 3, name: ""));
        categories.Add(new Category(id: 4, name: ""));
        categories.Add(new Category(id: 5, name: ""));
        
        _roundsRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: categories.Take(Configuration.CategoriesPerRound).ToList());

                return Operation<Round>.Success(result: round);
            });

        _roundsRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> rounds = new List<Round>();
                Round activeRound = _roundsRepository.Get(roundId).Result;
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsRepository.Get(userId, roundId).Returns(
            (args) =>
            {
                Turn turn = new Turn(
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    round: _roundsRepository.Get(id: roundId).Result,
                    startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds));

                return Operation<Turn>.Success(result: turn);
            });
        
        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound + 1];
        CategoryDto categoryDto;
        for (int i = 0; i < answerDtos.Length; i++)
        {
            categoryDto = new CategoryDto(id: categories[i].Id, name: categories[i].Name);
            answerDtos[i] = new AnswerDto(categoryDto: categoryDto, userInput: "", order: i);
        }
        #endregion

        #region -- Act & Assert --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(
                userId: userId,
                matchId: matchId,
                answerDtos: answerDtos.Take(Configuration.CategoriesPerRound - 1).ToArray());

        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Too few answers for turn of user with id {userId} " +
                $"for round with id {roundId} for match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);

        useCaseOperation = _useCase
            .Execute(
                userId: userId,
                matchId: matchId,
                answerDtos: answerDtos.Take(Configuration.CategoriesPerRound + 1).ToArray());

        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Too many answers for turn of user with id {userId} " +
                $"for round with id {roundId} for match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_category_mismatch()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        int matchId = 0;
        int roundId = 0;
        Match match;
        Round round;

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'j',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: new List<Round>() { round });

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(userId, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: true,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.Get(-1, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: false,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(-1, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        List<Category> categories = new List<Category>
        {
            new Category(id: 0, name: ""),
            new Category(id: 1, name: ""),
            new Category(id: 2, name: ""),
            new Category(id: 3, name: ""),
            new Category(id: 4, name: "")
        };

        _roundsRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: categories);

                return Operation<Round>.Success(result: round);
            });

        _roundsRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> rounds = new List<Round>();
                Round activeRound = _roundsRepository.Get(roundId).Result;
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsRepository.Get(userId, roundId).Returns(
            (args) =>
            {
                Turn turn = new Turn(
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    round: _roundsRepository.Get(id: roundId).Result,
                    startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds));

                return Operation<Turn>.Success(result: turn);
            });

        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];
        CategoryDto categoryDto;
        for (int i = 0; i < answerDtos.Length-1; i++)
        {
            categoryDto = new CategoryDto(id: categories[i].Id, name: categories[i].Name);
            answerDtos[i] = new AnswerDto(categoryDto: categoryDto, userInput: "", order: i);
        }
        
        categoryDto = new CategoryDto(id: -1, name: "");
        answerDtos[^1] = new AnswerDto(categoryDto: categoryDto, userInput: "", order: answerDtos.Length-1);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _useCase
            .Execute(
                userId: userId,
                matchId: matchId,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Some of your answers don't match the categories for round with id {roundId} in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }
}

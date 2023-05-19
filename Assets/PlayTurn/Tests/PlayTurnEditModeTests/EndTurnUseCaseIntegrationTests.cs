using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.UseCases;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.PlayTurn.UseCases;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.TestUtils;
using TopicTwister.Shared.UseCases;
using TopicTwister.Shared.Utils;


public class EndTurnUseCaseIntegrationTests
{
    private IEndTurnUseCase _testTargetUseCase;

    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IdaoMapper<Match, MatchDaoJson> _matchDaoJsonMapper;
    private IUserMatchesRepository _userMatchesRepository;
    private IdaoMapper<UserMatch, UserMatchDaoJson> _userMatchDaoMapper;
    private IMatchesRepository _matchesRepository;
    private IUniqueIdGenerator _matchesIdGenerator;
    private ITurnsRepository _turnsRepository;
    private IdaoMapper<Turn, TurnDaoJson> _turnDaoMapper;
    private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
    private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
    private IdaoMapper<Category, CategoryDaoJson> _categoryDaoJsonMapper;
    private IRoundsRepository _roundsRepository;
    private IUniqueIdGenerator _roundsIdGenerator;
    private IdtoMapper<Category, CategoryDto> _categoryDtoMapper;
    private IdtoMapper<Match, MatchDto> _matchDtoMapper;
    private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
    private IdtoMapper<Round, RoundDto> _roundDtoMapper;
    private IdtoMapper<UserMatch, UserMatchDto> _userMatchDtoMapper;
    private IdtoMapper<Answer, AnswerDto> _answerDtoMapper;
    private IAnswersRepository _answersRepository;
    private IdaoMapper<Answer, AnswerDaoJson> _answerDaoJsonMapper;
    private ITurnsReadOnlyRepository _turnsReadOnlyRepository;
    private IWordsRepository _wordsRepository;
    private IUserRoundsRepository _userRoundsRepository;
    private IdtoMapper<UserRound,  UserRoundDto> _userRoundDtoMapper;
    private IStartTurnUseCase _startTurnUseCase;
    private ICreateRoundSubUseCase _createRoundSubUseCase;
    private ICreateMatchSubUseCase _createMatchSubUseCase;
    private IdtoMapper<Turn, TurnDto> _turnDtoMapper;
    private ILetterReadOnlyRepository _letterReadOnlyRepository;
    private IdaoMapper<UserRound, UserRoundDaoJson> _userRoundDaoJsonMapper;
    private IResumeMatchUseCase _resumeMatchUseCase;

    [SetUp]
    public void SetUp()
    {
        _usersReadOnlyRepository = new UsersReadOnlyRepositoryInMemory();

        _matchDaoJsonMapper = new MatchDaoJsonMapper();

        _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
            resourceName: "TestData/Matches",
            matchDaoMapper: _matchDaoJsonMapper);

        _userMatchDaoMapper = new UserMatchDaoJsonMapper(
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userReadOnlyRepository: _usersReadOnlyRepository);

        _userMatchesRepository = new UserMatchesRepositoryJson(
            resourceName: "TestData/UserMatches",
            userMatchDaoMapper: _userMatchDaoMapper);

        _categoryDaoJsonMapper = new CategoryDaoJsonMapper();

        _categoriesReadOnlyRepository = new CategoriesReadOnlyRepositoryJson(
            resourceName: "TestData/Categories",
            categoryDaoJsonMapper: _categoryDaoJsonMapper);

        _roundsReadOnlyRepository = new RoundsReadOnlyRepositoryJson(
            resourceName: "TestData/Rounds",
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

        _wordsRepository = new WordsRepositoryJson(
                resourceName: "TestData/Words");

        _turnDaoMapper = new TurnDaoJsonMapper(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository);

        _matchDtoMapper = new MatchDtoMapper();

        _categoryDtoMapper = new CategoryDtoMapper();

        _roundDtoMapper = new RoundDtoMapper();

        _roundWithCategoriesDtoMapper = new RoundWithCategoriesDtoMapper(
            categoryDtoMapper: _categoryDtoMapper,
            roundDtoMapper: _roundDtoMapper,
            roundReadOnlyRepository: _roundsReadOnlyRepository);

        _matchesIdGenerator = new MatchesIdGenerator(
            matchesReadOnlyRepository: _matchesReadOnlyRepository);

        _matchesRepository = new MatchesRepositoryJson(
            resourceName: "TestData/Matches",
            matchesIdGenerator: _matchesIdGenerator,
            matchDaoMapper: _matchDaoJsonMapper);

        _userMatchDtoMapper = new UserMatchDtoMapper(
            matchesRepository: _matchesRepository,
            usersReadOnlyRepository: _usersReadOnlyRepository);

        _answerDtoMapper = new AnswerDtoMapper(categoryDtoMapper: _categoryDtoMapper);

        _turnsReadOnlyRepository = new TurnsReadOnlyRepositoryJson(
            resourceName: "TestData/Turns",
            turnDaoMapper: _turnDaoMapper);

        _answerDaoJsonMapper = new AnswerDaoJsonMapper(
            categoriesReadOnlyRepository: _categoriesReadOnlyRepository,
            turnsReadOnlyRepository: _turnsReadOnlyRepository);

        _answersRepository = new AnswersRepositoryJson(
            resourceName: "TestData/Answers",
            daoMapper: _answerDaoJsonMapper);

        _wordsRepository = new WordsRepositoryJson(
            resourceName: "TestData/Words");

        _turnsRepository = new TurnsRepositoryJson(
            resourceName: "TestData/Turns",
            turnDaoMapper: _turnDaoMapper);

        _roundsIdGenerator = new RoundsIdGenerator(
            roundsReadOnlyRepository: _roundsReadOnlyRepository);

        _roundsRepository = new RoundsRepositoryJson(
            resourceName: "TestData/Rounds",
            roundsIdGenerator: _roundsIdGenerator,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

        _userRoundDaoJsonMapper = new UserRoundDaoJsonMapper(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository);

        _userRoundsRepository = new UserRoundsRepository(
            resourceName: "TestData/UserRounds",
            userRoundDaoJsonMapper: _userRoundDaoJsonMapper);

        _userRoundDtoMapper = new UserRoundDtoMapper();

        _testTargetUseCase = new EndTurnUseCase(
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

        _turnDtoMapper = new TurnDtoMapper();

        _startTurnUseCase = new StartTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userMatchesRepository: _userMatchesRepository,
            turnsRepository: _turnsRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository,
            turnDtoMapper: _turnDtoMapper);

        _letterReadOnlyRepository = new LetterReadOnlyRepositoryInMemory();

        _createRoundSubUseCase = new CreateRoundSubUseCase(
            roundsRepository: _roundsRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            categoryReadOnlyRepository: _categoriesReadOnlyRepository,
            letterReadOnlyRepository: _letterReadOnlyRepository,
            roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);

        _createMatchSubUseCase = new CreateMatchSubUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            usersReadOnlyRespository: _usersReadOnlyRepository,
            matchDtoMapper: _matchDtoMapper);

        _resumeMatchUseCase = new ResumeMatchUseCase(
            createRoundSubUseCase: _createRoundSubUseCase,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository,
            roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
}

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
        new UserMatchesDeleteJson().Delete();
        new RoundsDeleteJson().Delete();
        new TurnsDeleteJson().Delete();
        new AnswersDeleteJson().Delete();
        new UserRoundsDeleteJson().Delete();
    }

    [Test]
    public void Test_ok_for_user_with_initiative_inside_time_limit()
    {
        #region -- Arrange --
        int userWithInitiativeId = Configuration.TestUserId;
        int userWithoutInitiativeId = Configuration.TestBotId;
        DateTime startDateTime = DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds);
        Match match = _matchesRepository.Insert(new Match(startDateTime: startDateTime)).Result;
        List<Category> categories = _categoriesReadOnlyRepository.GetRandomCategories(Configuration.CategoriesPerRound).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'B',
            isActive: true,
            match: match,
            categories: categories);

        round = _roundsRepository.Insert(round).Result;
        User userWithInitiative = _usersReadOnlyRepository.Get(id: userWithInitiativeId).Result;

        UserMatch userWithInitiativeMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: userWithInitiative,
            match: match);

        userWithInitiativeMatch = _userMatchesRepository.Insert(userWithInitiativeMatch).Result;

        UserMatch userWithoutInitiativeMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: false,
            user: _usersReadOnlyRepository.Get(id: userWithoutInitiativeId).Result,
            match: match);

        userWithoutInitiativeMatch = _userMatchesRepository.Insert(userWithoutInitiativeMatch).Result;

        Turn turn = new Turn(
            user: userWithInitiative,
            round: round,
            startDateTime: startDateTime);

        turn = _turnsRepository.Insert(turn).Result;
        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        List<CategoryDto> categoryDtos = categories
            .Select((category, index) => new CategoryDto(id: categories[index].Id, name: categories[index].Name))
            .ToList();

        for (int i = 0; i < 3; i++)
        {
            answerDtos[i] = new AnswerDto(categoryDto: categoryDtos[i], userInput: "Something", order: i);
        }

        for (int i = 3; i < answerDtos.Length; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: categoryDtos[i],
                userInput: $"{round.InitialLetter} TEST", order: i);
        }

        MatchDto matchDto = new MatchDto(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime);

        RoundDto roundDto = new RoundDto(
            id: round.Id,
            roundNumber: round.RoundNumber,
            initialLetter: round.InitialLetter,
            isActive: round.IsActive,
            matchId: round.Match.Id);

        UserMatchDto userWithInitiativeMatchDto = new UserMatchDto(
            score: userWithInitiativeMatch.Score,
            isWinner: userWithInitiativeMatch.IsWinner,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            userId: userWithInitiativeMatch.User.Id,
            matchId: userWithInitiativeMatch.Match.Id);

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
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(userId: userWithInitiativeId, matchId: match.Id, answerDtos: answerDtos);
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
        DateTime startDateTime = DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSecondsPlusTolerance + 10); 
        Match match = _matchesRepository.Insert(new Match(startDateTime: startDateTime)).Result;
        List<Category> categories = _categoriesReadOnlyRepository.GetRandomCategories(Configuration.CategoriesPerRound).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'B',
            isActive: true,
            match: match,
            categories: categories);

        round = _roundsRepository.Insert(round).Result;
        User userWithInitiative = _usersReadOnlyRepository.Get(id: userWithInitiativeId).Result;

        UserMatch userWithInitiativeMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: userWithInitiative,
            match: match);

        userWithInitiativeMatch = _userMatchesRepository.Insert(userWithInitiativeMatch).Result;

        UserMatch userWithoutInitiativeMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: false,
            user: _usersReadOnlyRepository.Get(id: userWithoutInitiativeId).Result,
            match: match);

        userWithoutInitiativeMatch = _userMatchesRepository.Insert(userWithoutInitiativeMatch).Result;

        Turn turn = new Turn(
            user: userWithInitiative,
            round: round,
            startDateTime: startDateTime);

        turn = _turnsRepository.Insert(turn).Result;
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

        MatchDto matchDto = new MatchDto(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime);

        RoundDto roundDto = new RoundDto(
            id: round.Id,
            roundNumber: round.RoundNumber,
            initialLetter: round.InitialLetter,
            isActive: round.IsActive,
            matchId: round.Match.Id);

        UserMatchDto userWithInitiativeMatchDto = new UserMatchDto(
            score: userWithInitiativeMatch.Score,
            isWinner: userWithInitiativeMatch.IsWinner,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            userId: userWithInitiativeMatch.User.Id,
            matchId: userWithInitiativeMatch.Match.Id);

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
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(userId: userWithInitiativeId, matchId: match.Id, answerDtos: answerDtos);
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

        MatchDto matchDto = _createMatchSubUseCase.Create(
            userWithInitiativeId: userWithInitiativeId,
            userWithoutInitiativeId: userWithoutInitiativeId).Result;

        RoundWithCategoriesDto roundWithCategoriesDto = _createRoundSubUseCase.Execute(matchDto: matchDto).Result;

        _startTurnUseCase.Execute(userId: userWithInitiativeId, matchId: matchDto.Id);
        _startTurnUseCase.Execute(userId: userWithoutInitiativeId, matchId: matchDto.Id);

        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        for (int i = 0; i < 3; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: roundWithCategoriesDto.CategoryDtos[i],
                userInput: $"{roundWithCategoriesDto.RoundDto.InitialLetter} TEST",
                order: i);
        }

        for (int i = 3; i < answerDtos.Length; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: roundWithCategoriesDto.CategoryDtos[i],
                userInput: "Something",
                order: i);
        }

        Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithInitiativeId, matchDto.Id, answerDtos: answerDtos).WasOk);

        UserMatch userWithInitiativeMatch = _userMatchesRepository
            .Get(userId: userWithInitiativeId, matchId: matchDto.Id).Result;

        UserMatchDto userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(userWithInitiativeMatch);

        UserMatch userWithoutInitiativeMatch = _userMatchesRepository
            .Get(userId: userWithoutInitiativeId, matchId: matchDto.Id).Result;

        UserMatchDto userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(userWithoutInitiativeMatch);

        UserRoundDto userWithInitiativeRoundDto = new UserRoundDto(
            userId: userWithInitiativeId,
            roundId: roundWithCategoriesDto.RoundDto.Id,
            isWinner: true,
            points: 3);

        UserRoundDto userWithoutInitiativeRoundDto = new UserRoundDto(
            userId: userWithoutInitiativeId,
            roundId: roundWithCategoriesDto.RoundDto.Id,
            isWinner: true,
            points: 3);

        RoundDto roundDto = new RoundDto(
            id: roundWithCategoriesDto.RoundDto.Id,
            roundNumber: roundWithCategoriesDto.RoundDto.RoundNumber,
            initialLetter: roundWithCategoriesDto.RoundDto.InitialLetter,
            isActive: false,
            matchId: roundWithCategoriesDto.RoundDto.MatchId);

        RoundWithCategoriesDto updatedRoundWithCategoriesDto = new RoundWithCategoriesDto(
            roundDto: roundDto,
            categoryDtos: roundWithCategoriesDto.CategoryDtos);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
                userId: userWithoutInitiativeId,
                matchId: matchDto.Id,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        EndOfTurnDto expectedDto = new EndOfTurnDto(
            matchDto: matchDto,
            roundWithCategoriesDto: updatedRoundWithCategoriesDto,
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutInitiativeMatchDto,
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
        #region -- Arrange --
        int userWithInitiativeId = Configuration.TestUserId;
        int userWithoutInitiativeId = Configuration.TestBotId;

        MatchDto matchDto = _createMatchSubUseCase.Create(
            userWithInitiativeId: userWithInitiativeId,
            userWithoutInitiativeId: userWithoutInitiativeId).Result;

        AnswerDto[] commonAnswerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        RoundWithCategoriesDto activeRoundWithCategoriesDto;
        List<RoundWithCategoriesDto> roundWithCategoriesDtos = new List<RoundWithCategoriesDto>();

        for (int i = 0; i < Configuration.RoundsPerMatch - 1; i++)
        {
            activeRoundWithCategoriesDto = _resumeMatchUseCase.Execute(matchDto).Result;
            roundWithCategoriesDtos.Add(activeRoundWithCategoriesDto);

            for (int j = 0; j < 2; j++)
            {
                commonAnswerDtos[j] = new AnswerDto(
                    categoryDto: activeRoundWithCategoriesDto.CategoryDtos[j],
                    userInput: $"{activeRoundWithCategoriesDto.RoundDto.InitialLetter} TEST",
                    order: j);
            }

            for (int j = 2; j < commonAnswerDtos.Length; j++)
            {
                commonAnswerDtos[j] = new AnswerDto(
                    categoryDto: activeRoundWithCategoriesDto.CategoryDtos[j],
                    userInput: "Something",
                    order: j);
            }

            _startTurnUseCase.Execute(userId: userWithInitiativeId, matchId: matchDto.Id);
            Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithInitiativeId, matchDto.Id, answerDtos: commonAnswerDtos).WasOk);

            _startTurnUseCase.Execute(userId: userWithoutInitiativeId, matchId: matchDto.Id);
            Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithoutInitiativeId, matchDto.Id, answerDtos: commonAnswerDtos).WasOk);
        }

        activeRoundWithCategoriesDto = _resumeMatchUseCase.Execute(matchDto).Result;
        roundWithCategoriesDtos.Add(activeRoundWithCategoriesDto);

        for (int i = 0; i < 2; i++)
        {
            commonAnswerDtos[i] = new AnswerDto(
                categoryDto: activeRoundWithCategoriesDto.CategoryDtos[i],
                userInput: $"{activeRoundWithCategoriesDto.RoundDto.InitialLetter} TEST",
                order: i);
        }

        for (int i = 2; i < commonAnswerDtos.Length; i++)
        {
            commonAnswerDtos[i] = new AnswerDto(
                categoryDto: activeRoundWithCategoriesDto.CategoryDtos[i],
                userInput: "Something",
                order: i);
        }

        _startTurnUseCase.Execute(userId: userWithInitiativeId, matchId: matchDto.Id);
        Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithInitiativeId, matchDto.Id, answerDtos: commonAnswerDtos).WasOk);

        List<AnswerDto> userWithInitiativeAnswersDtos = commonAnswerDtos.ToList();
        List<AnswerDto> userWithoutInitiativeAnswersDtos = commonAnswerDtos.ToList();

        userWithoutInitiativeAnswersDtos[^1] = new AnswerDto(
                categoryDto: activeRoundWithCategoriesDto.CategoryDtos[^1],
                userInput: $"{activeRoundWithCategoriesDto.RoundDto.InitialLetter} TEST",
                order: userWithoutInitiativeAnswersDtos.Count - 1);

        _startTurnUseCase.Execute(userId: userWithoutInitiativeId, matchId: matchDto.Id);

        UserMatch userWithInitiativeMatch = _userMatchesRepository
            .Get(userId: userWithInitiativeId, matchId: matchDto.Id).Result;

        userWithInitiativeMatch = new UserMatch(
            score: Configuration.RoundsPerMatch - 1,
            isWinner: false,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            user: userWithInitiativeMatch.User,
            match: userWithInitiativeMatch.Match);

        UserMatchDto userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(userWithInitiativeMatch);

        UserMatch userWithoutInitiativeMatch = _userMatchesRepository
            .Get(userId: userWithoutInitiativeId, matchId: matchDto.Id).Result;

        userWithoutInitiativeMatch = new UserMatch(
            score: Configuration.RoundsPerMatch,
            isWinner: true,
            hasInitiative: userWithoutInitiativeMatch.HasInitiative,
            user: userWithoutInitiativeMatch.User,
            match: userWithoutInitiativeMatch.Match);

        UserMatchDto userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(userWithoutInitiativeMatch);

        List<UserRoundDto> userWithInitiativeRoundDtos = new List<UserRoundDto>();
        List<UserRoundDto> userWithoutInitiativeRoundDtos = new List<UserRoundDto>();

        foreach (RoundWithCategoriesDto roundWithCategoriesDto in roundWithCategoriesDtos)
        {
            bool isLastRound = roundWithCategoriesDto.RoundDto.RoundNumber == Configuration.RoundsPerMatch - 1;

            UserRoundDto userWithInitiativeRoundDto = new UserRoundDto(
                userId: userWithInitiativeId,
                roundId: roundWithCategoriesDto.RoundDto.Id,
                isWinner: isLastRound == false,
                points: 2);

            userWithInitiativeRoundDtos.Add(userWithInitiativeRoundDto);

            UserRoundDto userWithoutInitiativeRoundDto = new UserRoundDto(
                userId: userWithoutInitiativeId,
                roundId: roundWithCategoriesDto.RoundDto.Id,
                isWinner: true,
                points: isLastRound ? 3 : 2);

            userWithoutInitiativeRoundDtos.Add(userWithoutInitiativeRoundDto);
        }

        RoundDto roundDto = new RoundDto(
            id: activeRoundWithCategoriesDto.RoundDto.Id,
            roundNumber: Configuration.RoundsPerMatch - 1,
            initialLetter: activeRoundWithCategoriesDto.RoundDto.InitialLetter,
            isActive: false,
            matchId: activeRoundWithCategoriesDto.RoundDto.MatchId);

        RoundWithCategoriesDto updatedRoundWithCategoriesDto = new RoundWithCategoriesDto(
            roundDto: roundDto,
            categoryDtos: activeRoundWithCategoriesDto.CategoryDtos);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
                userId: userWithoutInitiativeId,
                matchId: matchDto.Id,
                answerDtos: userWithoutInitiativeAnswersDtos.ToArray());
        #endregion

        #region -- Assert --
        EndOfTurnDto expectedDto = new EndOfTurnDto(
            matchDto: matchDto,
            roundWithCategoriesDto: updatedRoundWithCategoriesDto,
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutInitiativeMatchDto,
            userWithInitiativeRoundDtos: userWithInitiativeRoundDtos,
            userWithoutInitiativeRoundDtos: userWithoutInitiativeRoundDtos,
            answerDtosOfUserWithInitiative: userWithInitiativeAnswersDtos,
            answerDtosOfUserWithoutInitiative: userWithoutInitiativeAnswersDtos);

        Assert.IsTrue(useCaseOperation.WasOk);
        Assert.AreEqual(expected: expectedDto, actual: useCaseOperation.Result);
        #endregion
    }

    [Test]
    public void Test_ok_for_user_without_initiative_end_of_final_round_and_match_lose()
    {
        #region -- Arrange --
        int userWithInitiativeId = Configuration.TestUserId;
        int userWithoutInitiativeId = Configuration.TestBotId;

        MatchDto matchDto = _createMatchSubUseCase.Create(
            userWithInitiativeId: userWithInitiativeId,
            userWithoutInitiativeId: userWithoutInitiativeId).Result;

        AnswerDto[] commonAnswerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        RoundWithCategoriesDto activeRoundWithCategoriesDto;
        List<RoundWithCategoriesDto> roundWithCategoriesDtos = new List<RoundWithCategoriesDto>();

        for (int i = 0; i < Configuration.RoundsPerMatch - 1; i++)
        {
            activeRoundWithCategoriesDto = _resumeMatchUseCase.Execute(matchDto).Result;
            roundWithCategoriesDtos.Add(activeRoundWithCategoriesDto);

            for (int j = 0; j < 2; j++)
            {
                commonAnswerDtos[j] = new AnswerDto(
                    categoryDto: activeRoundWithCategoriesDto.CategoryDtos[j],
                    userInput: $"{activeRoundWithCategoriesDto.RoundDto.InitialLetter} TEST",
                    order: j);
            }

            for (int j = 2; j < commonAnswerDtos.Length; j++)
            {
                commonAnswerDtos[j] = new AnswerDto(
                    categoryDto: activeRoundWithCategoriesDto.CategoryDtos[j],
                    userInput: "Something",
                    order: j);
            }

            _startTurnUseCase.Execute(userId: userWithInitiativeId, matchId: matchDto.Id);
            Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithInitiativeId, matchDto.Id, answerDtos: commonAnswerDtos).WasOk);

            _startTurnUseCase.Execute(userId: userWithoutInitiativeId, matchId: matchDto.Id);
            Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithoutInitiativeId, matchDto.Id, answerDtos: commonAnswerDtos).WasOk);
        }

        activeRoundWithCategoriesDto = _resumeMatchUseCase.Execute(matchDto).Result;
        roundWithCategoriesDtos.Add(activeRoundWithCategoriesDto);

        for (int i = 0; i < 2; i++)
        {
            commonAnswerDtos[i] = new AnswerDto(
                categoryDto: activeRoundWithCategoriesDto.CategoryDtos[i],
                userInput: $"{activeRoundWithCategoriesDto.RoundDto.InitialLetter} TEST",
                order: i);
        }

        for (int i = 2; i < commonAnswerDtos.Length; i++)
        {
            commonAnswerDtos[i] = new AnswerDto(
                categoryDto: activeRoundWithCategoriesDto.CategoryDtos[i],
                userInput: "Something",
                order: i);
        }

        _startTurnUseCase.Execute(userId: userWithInitiativeId, matchId: matchDto.Id);
        Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithInitiativeId, matchDto.Id, answerDtos: commonAnswerDtos).WasOk);

        List<AnswerDto> userWithInitiativeAnswersDtos = commonAnswerDtos.ToList();
        List<AnswerDto> userWithoutInitiativeAnswersDtos = commonAnswerDtos.ToList();

        userWithoutInitiativeAnswersDtos[0] = new AnswerDto(
                categoryDto: activeRoundWithCategoriesDto.CategoryDtos[0],
                userInput: "Something",
                order: 0);

        _startTurnUseCase.Execute(userId: userWithoutInitiativeId, matchId: matchDto.Id);

        UserMatch userWithInitiativeMatch = _userMatchesRepository
            .Get(userId: userWithInitiativeId, matchId: matchDto.Id).Result;

        userWithInitiativeMatch = new UserMatch(
            score: Configuration.RoundsPerMatch,
            isWinner: true,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            user: userWithInitiativeMatch.User,
            match: userWithInitiativeMatch.Match);

        UserMatchDto userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(userWithInitiativeMatch);

        UserMatch userWithoutInitiativeMatch = _userMatchesRepository
            .Get(userId: userWithoutInitiativeId, matchId: matchDto.Id).Result;

        userWithoutInitiativeMatch = new UserMatch(
            score: Configuration.RoundsPerMatch -1,
            isWinner: false,
            hasInitiative: userWithoutInitiativeMatch.HasInitiative,
            user: userWithoutInitiativeMatch.User,
            match: userWithoutInitiativeMatch.Match);

        UserMatchDto userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(userWithoutInitiativeMatch);

        List<UserRoundDto> userWithInitiativeRoundDtos = new List<UserRoundDto>();
        List<UserRoundDto> userWithoutInitiativeRoundDtos = new List<UserRoundDto>();

        foreach (RoundWithCategoriesDto roundWithCategoriesDto in roundWithCategoriesDtos)
        {
            UserRoundDto userWithInitiativeRoundDto = new UserRoundDto(
                userId: userWithInitiativeId,
                roundId: roundWithCategoriesDto.RoundDto.Id,
                isWinner: true,
                points: 2);

            userWithInitiativeRoundDtos.Add(userWithInitiativeRoundDto);

            bool isLastRound = roundWithCategoriesDto.RoundDto.RoundNumber == Configuration.RoundsPerMatch - 1;

            UserRoundDto userWithoutInitiativeRoundDto = new UserRoundDto(
                userId: userWithoutInitiativeId,
                roundId: roundWithCategoriesDto.RoundDto.Id,
                isWinner: isLastRound == false,
                points: isLastRound ? 1 : 2);

            userWithoutInitiativeRoundDtos.Add(userWithoutInitiativeRoundDto);
        }

        RoundDto roundDto = new RoundDto(
            id: activeRoundWithCategoriesDto.RoundDto.Id,
            roundNumber: Configuration.RoundsPerMatch - 1,
            initialLetter: activeRoundWithCategoriesDto.RoundDto.InitialLetter,
            isActive: false,
            matchId: activeRoundWithCategoriesDto.RoundDto.MatchId);

        RoundWithCategoriesDto updatedRoundWithCategoriesDto = new RoundWithCategoriesDto(
            roundDto: roundDto,
            categoryDtos: activeRoundWithCategoriesDto.CategoryDtos);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
                userId: userWithoutInitiativeId,
                matchId: matchDto.Id,
                answerDtos: userWithoutInitiativeAnswersDtos.ToArray());
        #endregion

        #region -- Assert --
        EndOfTurnDto expectedDto = new EndOfTurnDto(
            matchDto: matchDto,
            roundWithCategoriesDto: updatedRoundWithCategoriesDto,
            userWithInitiativeMatchDto: userWithInitiativeMatchDto,
            userWithoutInitiativeMatchDto: userWithoutInitiativeMatchDto,
            userWithInitiativeRoundDtos: userWithInitiativeRoundDtos,
            userWithoutInitiativeRoundDtos: userWithoutInitiativeRoundDtos,
            answerDtosOfUserWithInitiative: userWithInitiativeAnswersDtos,
            answerDtosOfUserWithoutInitiative: userWithoutInitiativeAnswersDtos);

        Assert.IsTrue(useCaseOperation.WasOk);
        Assert.AreEqual(expected: expectedDto, actual: useCaseOperation.Result);
        #endregion
    }

    [Test]
    public void Test_ok_for_user_without_initiative_end_of_final_round_and_match_draw()
    {
        #region -- Arrange --
        int userWithInitiativeId = Configuration.TestUserId;
        int userWithoutInitiativeId = Configuration.TestBotId;

        MatchDto matchDto = _createMatchSubUseCase.Create(
            userWithInitiativeId: userWithInitiativeId,
            userWithoutInitiativeId: userWithoutInitiativeId).Result;

        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        RoundWithCategoriesDto activeRoundWithCategoriesDto;
        List<RoundWithCategoriesDto> roundWithCategoriesDtos = new List<RoundWithCategoriesDto>();

        for (int i = 0; i < Configuration.RoundsPerMatch - 1; i++)
        {
            activeRoundWithCategoriesDto = _resumeMatchUseCase.Execute(matchDto).Result;
            roundWithCategoriesDtos.Add(activeRoundWithCategoriesDto);

            for (int j = 0; j < 2; j++)
            {
                answerDtos[j] = new AnswerDto(
                    categoryDto: activeRoundWithCategoriesDto.CategoryDtos[j],
                    userInput: $"{activeRoundWithCategoriesDto.RoundDto.InitialLetter} TEST",
                    order: j);
            }

            for (int j = 2; j < answerDtos.Length; j++)
            {
                answerDtos[j] = new AnswerDto(
                    categoryDto: activeRoundWithCategoriesDto.CategoryDtos[j],
                    userInput: "Something",
                    order: j);
            }

            _startTurnUseCase.Execute(userId: userWithInitiativeId, matchId: matchDto.Id);
            Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithInitiativeId, matchDto.Id, answerDtos: answerDtos).WasOk);

            _startTurnUseCase.Execute(userId: userWithoutInitiativeId, matchId: matchDto.Id);
            Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithoutInitiativeId, matchDto.Id, answerDtos: answerDtos).WasOk);
        }

        activeRoundWithCategoriesDto = _resumeMatchUseCase.Execute(matchDto).Result;
        roundWithCategoriesDtos.Add(activeRoundWithCategoriesDto);

        for (int i = 0; i < 2; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: activeRoundWithCategoriesDto.CategoryDtos[i],
                userInput: $"{activeRoundWithCategoriesDto.RoundDto.InitialLetter} TEST",
                order: i);
        }

        for (int i = 2; i < answerDtos.Length; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: activeRoundWithCategoriesDto.CategoryDtos[i],
                userInput: "Something",
                order: i);
        }

        _startTurnUseCase.Execute(userId: userWithInitiativeId, matchId: matchDto.Id);
        Assert.IsTrue(_testTargetUseCase.Execute(userId: userWithInitiativeId, matchDto.Id, answerDtos: answerDtos).WasOk);

        _startTurnUseCase.Execute(userId: userWithoutInitiativeId, matchId: matchDto.Id);

        UserMatch userWithInitiativeMatch = _userMatchesRepository
            .Get(userId: userWithInitiativeId, matchId: matchDto.Id).Result;

        userWithInitiativeMatch = new UserMatch(
            score: Configuration.RoundsPerMatch,
            isWinner: true,
            hasInitiative: userWithInitiativeMatch.HasInitiative,
            user: userWithInitiativeMatch.User,
            match: userWithInitiativeMatch.Match);

        UserMatchDto userWithInitiativeMatchDto = _userMatchDtoMapper.ToDTO(userWithInitiativeMatch);

        UserMatch userWithoutInitiativeMatch = _userMatchesRepository
            .Get(userId: userWithoutInitiativeId, matchId: matchDto.Id).Result;

        userWithoutInitiativeMatch = new UserMatch(
            score: Configuration.RoundsPerMatch,
            isWinner: true,
            hasInitiative: userWithoutInitiativeMatch.HasInitiative,
            user: userWithoutInitiativeMatch.User,
            match: userWithoutInitiativeMatch.Match);

        UserMatchDto userWithoutInitiativeMatchDto = _userMatchDtoMapper.ToDTO(userWithoutInitiativeMatch);

        List<UserRoundDto> userWithInitiativeRoundDtos = new List<UserRoundDto>();
        List<UserRoundDto> userWithoutInitiativeRoundDtos = new List<UserRoundDto>();

        foreach(RoundWithCategoriesDto roundWithCategoriesDto in roundWithCategoriesDtos)
        {
            UserRoundDto userWithInitiativeRoundDto = new UserRoundDto(
                userId: userWithInitiativeId,
                roundId: roundWithCategoriesDto.RoundDto.Id,
                isWinner: true,
                points: 2);

            userWithInitiativeRoundDtos.Add(userWithInitiativeRoundDto);

            UserRoundDto userWithoutInitiativeRoundDto = new UserRoundDto(
                userId: userWithoutInitiativeId,
                roundId: roundWithCategoriesDto.RoundDto.Id,
                isWinner: true,
                points: 2);

            userWithoutInitiativeRoundDtos.Add(userWithoutInitiativeRoundDto);
        }

        RoundDto roundDto = new RoundDto(
            id: activeRoundWithCategoriesDto.RoundDto.Id,
            roundNumber: Configuration.RoundsPerMatch - 1,
            initialLetter: activeRoundWithCategoriesDto.RoundDto.InitialLetter,
            isActive: false,
            matchId: activeRoundWithCategoriesDto.RoundDto.MatchId);

        RoundWithCategoriesDto updatedRoundWithCategoriesDto = new RoundWithCategoriesDto(
            roundDto: roundDto,
            categoryDtos: activeRoundWithCategoriesDto.CategoryDtos);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
                userId: userWithoutInitiativeId,
                matchId: matchDto.Id,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        EndOfTurnDto expectedDto = new EndOfTurnDto(
            matchDto: matchDto,
            roundWithCategoriesDto: updatedRoundWithCategoriesDto,
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
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
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
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
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
        User user = _usersReadOnlyRepository.Get(userId).Result;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        UserMatch userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: _usersReadOnlyRepository.Get(Configuration.ExtraUserOneId).Result,
            match: match);

        _userMatchesRepository.Insert(userMatch);

        userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: _usersReadOnlyRepository.Get(Configuration.ExtraUserTwoId).Result,
            match: match);

        _userMatchesRepository.Insert(userMatch);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
                userId: user.Id,
                matchId: match.Id,
                answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"User with id {user.Id} is not involved in match with id {match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_turn()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        User user = _usersReadOnlyRepository.Get(userId).Result;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'P',
            isActive: true,
            match: match,
            categories: new List<Category>());

        _roundsRepository.Insert(round);

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: _roundsReadOnlyRepository.GetMany(match.Id).Result);

        UserMatch userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: user,
            match: match);

        userMatch = _userMatchesRepository.Insert(userMatch).Result;

        UserMatch botUserMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: _usersReadOnlyRepository.Get(Configuration.TestBotId).Result,
            match: match);

        _userMatchesRepository.Insert(botUserMatch);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
            userId: userMatch.User.Id,
            matchId: userMatch.Match.Id,
            answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);

        Assert.AreEqual(
            expected: $"Turn not found for user with id {user.Id} " +
                $"in round with id {match.ActiveRound.Id} in match with id {match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_already_ended_turn()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        User user = _usersReadOnlyRepository.Get(userId).Result;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'P',
            isActive: true,
            match: match,
            categories: new List<Category>());

        _roundsRepository.Insert(round);

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: _roundsReadOnlyRepository.GetMany(match.Id).Result);

        UserMatch userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: user,
            match: match);

        userMatch = _userMatchesRepository.Insert(userMatch).Result;

        _userMatchesRepository.Insert(
            new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: true,
                user: _usersReadOnlyRepository.Get(Configuration.ExtraUserOneId).Result,
                match: match));

        Turn turn = new Turn(
            user: user,
            round: match.ActiveRound,
            startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds),
            endDateTime: DateTime.UtcNow);

        turn = _turnsRepository.Insert(turn).Result;
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
                userId: userMatch.User.Id,
                matchId: userMatch.Match.Id,
                answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn already ended for user with id {turn.User.Id} " +
                    $"in round with id {turn.Round.Id} in match with id {turn.Round.Match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_too_many_or_too_few_answers()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        User user = _usersReadOnlyRepository.Get(userId).Result;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        List<Category> categories = _categoriesReadOnlyRepository
            .GetRandomCategories(Configuration.CategoriesPerRound + 1).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'P',
            isActive: true,
            match: match,
            categories: categories);

        _roundsRepository.Insert(round);

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: _roundsReadOnlyRepository.GetMany(match.Id).Result);

        UserMatch userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: user,
            match: match);
        userMatch = _userMatchesRepository.Insert(userMatch).Result;

        UserMatch botUserMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: _usersReadOnlyRepository.Get(Configuration.TestBotId).Result,
            match: match);

        _userMatchesRepository.Insert(botUserMatch);

        Turn turn = new Turn(
            user: userMatch.User,
            round: match.ActiveRound,
            startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds));

        turn = _turnsRepository.Insert(turn).Result;
        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound + 1];
        for(int i = 0; i < answerDtos.Length; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: _categoryDtoMapper.ToDTO(categories[i]),
                userInput: "", order: i);
        }
        #endregion

        #region -- Act & Assert --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
                userId: user.Id,
                matchId: match.Id,
                answerDtos: answerDtos.Take(Configuration.CategoriesPerRound - 1).ToArray());

        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Too few answers for turn of user with id {turn.User.Id} " +
                $"for round with id {turn.Round.Id} for match with id {turn.Round.Match.Id}",
            actual: useCaseOperation.ErrorMessage);

        useCaseOperation = _testTargetUseCase
            .Execute(
                userId: user.Id,
                matchId: match.Id,
                answerDtos: answerDtos.Take(Configuration.CategoriesPerRound + 1).ToArray());

        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Too many answers for turn of user with id {turn.User.Id} " +
                $"for round with id {turn.Round.Id} for match with id {turn.Round.Match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_category_mismatch()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        User user = _usersReadOnlyRepository.Get(userId).Result;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        List<Category> categories = _categoriesReadOnlyRepository
            .GetRandomCategories(Configuration.CategoriesPerRound + 1).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'P',
            isActive: true,
            match: match,
            categories: categories);

        _roundsRepository.Insert(round);

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: _roundsReadOnlyRepository.GetMany(match.Id).Result);

        UserMatch userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: user,
            match: match);
        userMatch = _userMatchesRepository.Insert(userMatch).Result;

        _userMatchesRepository.Insert(
            new UserMatch(
                score: 0,
                isWinner: false,
                hasInitiative: true,
                user: _usersReadOnlyRepository.Get(Configuration.ExtraUserOneId).Result,
                match: match));

        Turn turn = new Turn(
            user: userMatch.User,
            round: match.ActiveRound,
            startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds));

        turn = _turnsRepository.Insert(turn).Result;
        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];

        for (int i = 0; i < answerDtos.Length - 1; i++)
        {
            answerDtos[i] = new AnswerDto(
                categoryDto: _categoryDtoMapper.ToDTO(categories[i]),
                userInput: "", order: i);
        }

        answerDtos[^1] = new AnswerDto(
            categoryDto: new CategoryDto(id: -1, name: ""),
            userInput: "", order: answerDtos.Length - 1);
        #endregion

        #region -- Act --
        Operation<EndOfTurnDto> useCaseOperation = _testTargetUseCase
            .Execute(
                userId: turn.User.Id,
                matchId: turn.Round.Match.Id,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);

        Assert.AreEqual(
            expected: $"Some of your answers don't match the categories for round with id " +
                $"{turn.Round.Id} in match with id {turn.Round.Match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }
}

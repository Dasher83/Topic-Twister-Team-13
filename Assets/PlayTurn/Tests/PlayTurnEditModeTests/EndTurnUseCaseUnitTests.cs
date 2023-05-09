using System;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


public class EndTurnUseCaseUnitTests
{
    private IEndTurnUseCase _useCase;
    private IUsersReadOnlyRepository _usersReadOnlyRepository;

    [SetUp]
    public void SetUp()
    {
        _usersReadOnlyRepository = Substitute.For<IUsersReadOnlyRepository>();

        _useCase = new EndTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository);
    }

    [Test]
    public void Test_ok_for_user_with_iniciative_inside_time_limit()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_with_iniciative_outside_time_limit()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_iniciative_inside_time_limit()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_iniciative_outside_time_limit()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_unknown_user()
    {
        #region -- Arrange --
        int userId = -1;
        int matchId = -1;

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Failure(errorMessage: $"User not found with id: {userId}");
            });
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
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
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_user_not_in_match()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_unknown_turn()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_already_ended_turn()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_too_many_or_too_few_answers()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_category_mismatch()
    {
        throw new NotImplementedException();
    }
}

using System;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;
using UnityEngine;
using UnityEngine.TestTools;


public class StartTurnUseCaseUnitTests
{
    IStartTurnUseCase _useCase;
    IUsersReadOnlyRepository _userReadOnlyRepository;

    [SetUp]
    public void SetUp()
    {
        _userReadOnlyRepository = Substitute.For<IUsersReadOnlyRepository>();
    }

    [Test]
    public void Test_ok_scenario()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_unknown_user()
    {
        #region -- Arrange --
        int userId = -1;

        _userReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Failure(errorMessage: $"User not found with id: {userId}");
            });

        _useCase = new StartTurnUseCase(userReadOnlyRepository: _userReadOnlyRepository);
        #endregion

        #region -- Act --
        Operation<bool> useCaseOperation = _useCase.Execute(userId: userId, matchDto: null);
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
    public void Test_fail_due_to_already_started_turn()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_user_turn_order()
    {
        throw new NotImplementedException();
    }
}

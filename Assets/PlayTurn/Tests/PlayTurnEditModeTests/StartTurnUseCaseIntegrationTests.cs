using System;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Utils;
using UnityEngine;
using UnityEngine.TestTools;


public class StartTurnUseCaseIntegrationTests
{
    IStartTurnUseCase _useCase;
    IUsersReadOnlyRepository _userReadOnlyRepository;

    [SetUp]
    public void SetUp()
    {
        _userReadOnlyRepository = new UsersReadOnlyRepositoryInMemory();
        _useCase = new StartTurnUseCase(userReadOnlyRepository: _userReadOnlyRepository);
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

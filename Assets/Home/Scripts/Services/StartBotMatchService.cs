using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.Services
{
    public class StartBotMatchService : IStartBotMatchService
    {
        private IStartBotMatchUseCase _useCase;

        public StartBotMatchService(IStartBotMatchUseCase useCase)
        {
            _useCase = useCase;
        }

        public MatchDto StartMatch()
        {
            Operation<MatchDto> useCaseOperation = _useCase.Execute(userId: Configuration.TestUserId);
            return useCaseOperation.WasOk ? useCaseOperation.Result : null;
        }
    }
}

using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.Services
{
    public class StartBotMatchService : IStartBotMatchService
    {
        private IStartBotMatchUseCase _useCase;
        private const int UserTestId = 1;

        public StartBotMatchService(IStartBotMatchUseCase useCase)
        {
            _useCase = useCase;
        }

        public MatchDto Start()
        {
            Operation<MatchDto> useCaseOperation = _useCase.Start(userId: UserTestId);
            return useCaseOperation.WasOk ? useCaseOperation.Outcome : null;
        }
    }
}

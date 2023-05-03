using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.Services
{
    public class CreateBotMatchService : ICreateBotMatchService
    {
        private ICreateBotMatchUseCase _useCase;
        private const int UserTestId = 1;

        public CreateBotMatchService(ICreateBotMatchUseCase useCase)
        {
            _useCase = useCase;
        }

        public MatchDto Create()
        {
            Operation<MatchDto> useCaseOperation = _useCase.Create(userId: UserTestId);
            return useCaseOperation.WasOk ? useCaseOperation.Outcome : null;
        }
    }
}

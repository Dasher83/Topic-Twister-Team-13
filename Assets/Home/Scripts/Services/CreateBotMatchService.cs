using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.UseCases.Utils;

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

        public MatchDTO Create()
        {
            UseCaseResult<MatchDTO> useCaseResult = _useCase.Create(userId: UserTestId);
            return useCaseResult.WasOk ? useCaseResult.Outcome : null;
        }
    }
}

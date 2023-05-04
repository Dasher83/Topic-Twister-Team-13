using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Services
{
    public class ResumeMatchGatewayService : IResumeMatchGatewayService
    {
        private IResumeMatchUseCase _useCase;

        public ResumeMatchGatewayService(IResumeMatchUseCase useCase)
        {
            _useCase = useCase;
        }

        public RoundWithCategoriesDto Create(MatchDto matchDto)
        {
            Operation<RoundWithCategoriesDto> useCaseResult = _useCase.Execute(matchDto);
            return useCaseResult.WasOk ? useCaseResult.Outcome : null;
        }
    }
}

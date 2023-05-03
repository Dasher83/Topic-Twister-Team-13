using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.UseCases
{
    public class ResumeMatchUseCase : IResumeMatchUseCase
    {
        private ICreateRoundSubUseCase _createRoundSubUseCase;

        public ResumeMatchUseCase(ICreateRoundSubUseCase createRoundSubUseCase)
        {
            _createRoundSubUseCase = createRoundSubUseCase;
        }

        public Operation<RoundWithCategoriesDto> Execute(MatchDto matchDto)
        {
            Operation<RoundWithCategoriesDto> createRoundSubUseCaseOperation = _createRoundSubUseCase.Execute(matchDto);

            if(createRoundSubUseCaseOperation.WasOk == false)
            {
                Operation<RoundWithCategoriesDto>.Failure(errorMessage: createRoundSubUseCaseOperation.ErrorMessage);
            }

            return Operation<RoundWithCategoriesDto>.Success(outcome: createRoundSubUseCaseOperation.Outcome);
        }
    }
}

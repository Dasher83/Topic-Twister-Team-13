using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.UseCases
{
    public class CreateBotMatchUseCase : ICreateBotMatchUseCase
    {
        private ICreateMatchSubUseCase _createMatchSubUseCase;
        private ICreateRoundSubUseCase _createRoundSubUseCase;
        private const int BotId = 2;

        public CreateBotMatchUseCase(
            ICreateMatchSubUseCase createMatchSubUseCase,
            ICreateRoundSubUseCase createRoundSubUseCase)
        {
            _createMatchSubUseCase = createMatchSubUseCase;
            _createRoundSubUseCase = createRoundSubUseCase;
        }

        public Operation<MatchDto> Create(int userId)
        {
            Operation<MatchDto> createMatchSubUseCaseOperation = _createMatchSubUseCase.Create(
                userIdPlayerOne: userId, userIdPlayerTwo: BotId);

            if(createMatchSubUseCaseOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: createMatchSubUseCaseOperation.ErrorMessage);
            }

            MatchDto matchDto = createMatchSubUseCaseOperation.Outcome;

            Operation<RoundWithCategoriesDto> createRoundSubUseCaseOberation = _createRoundSubUseCase.Create(matchDto);

            if(createRoundSubUseCaseOberation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: createRoundSubUseCaseOberation.ErrorMessage);
            }

            return createMatchSubUseCaseOperation;
        }
    }
}

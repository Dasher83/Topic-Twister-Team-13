using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.UseCases
{
    public class StartBotMatchUseCase : IStartBotMatchUseCase
    {
        private ICreateMatchSubUseCase _createMatchSubUseCase;
        private ICreateRoundSubUseCase _createRoundSubUseCase;
        private const int BotId = 2;

        public StartBotMatchUseCase(
            ICreateMatchSubUseCase createMatchSubUseCase,
            ICreateRoundSubUseCase createRoundSubUseCase)
        {
            _createMatchSubUseCase = createMatchSubUseCase;
            _createRoundSubUseCase = createRoundSubUseCase;
        }

        public Operation<MatchDto> Execute(int userId)
        {
            Operation<MatchDto> createMatchSubUseCaseOperation = _createMatchSubUseCase.Create(
                userWithInitiativeId: userId, userWithoutInitiativeId: BotId);

            if(createMatchSubUseCaseOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: createMatchSubUseCaseOperation.ErrorMessage);
            }

            MatchDto matchDto = createMatchSubUseCaseOperation.Result;

            Operation<RoundWithCategoriesDto> createRoundSubUseCaseOperation = _createRoundSubUseCase.Execute(matchDto);

            if(createRoundSubUseCaseOperation.WasOk == false)
            {
                return Operation<MatchDto>.Failure(errorMessage: createRoundSubUseCaseOperation.ErrorMessage);
            }

            return createMatchSubUseCaseOperation;
        }
    }
}

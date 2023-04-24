using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;


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
            return _useCase.Create(userId: UserTestId);
        }
    }
}

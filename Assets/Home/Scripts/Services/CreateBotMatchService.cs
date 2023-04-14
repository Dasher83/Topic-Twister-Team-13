using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.Home.Services
{
    public class CreateBotMatchService : ICreateBotMatchService
    {
        private ICreateBotMatchUseCase _useCase;

        public CreateBotMatchService(ICreateBotMatchUseCase useCase)
        {
            _useCase = useCase;
        }

        public MatchDTO Create()
        {
            throw new System.NotImplementedException();
        }
    }
}

using TopicTwister.NewRound.Models;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.NewRound.UseCases
{
    public class CreateRoundUseCase : ICreateRoundUseCase
    {
        private IRoundsRepository _roundsRepository;
        private IdtoMapper<Match, MatchDTO> _mapper;

        public CreateRoundUseCase(IRoundsRepository roundsRepository, IdtoMapper<Match, MatchDTO> mapper)
        {
            _roundsRepository = roundsRepository;
            _mapper = mapper;
        }

        public RoundWithCategoriesDto Create(MatchDTO matchDto)
        {
            Round round = new Round();
            round = _roundsRepository.Save(round);
        }
    }
}

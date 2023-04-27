using TopicTwister.NewRound.Models;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Mappers;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;
using TopicTwister.Shared.UseCases.Utils;


namespace TopicTwister.NewRound.UseCases
{
    public class CreateRoundUseCase : ICreateRoundUseCase
    {
        private IRoundsRepository _roundsRepository;
        private IMatchesRepository _matchesRepository;
        private IdtoMapper<Match, MatchDTO> _matchDtoMapper;
        private IdtoMapper<Round, RoundWithCategoriesDtoMapper> _roundWithCategoriesDtoMapper;

        public CreateRoundUseCase(
            IRoundsRepository roundsRepository,
            IMatchesRepository matchesRepository,
            IdtoMapper<Match, MatchDTO> matchDtoMapper,
            IdtoMapper<Round, RoundWithCategoriesDtoMapper> roundWithCategoriesDtoMapper)
        {
            _roundsRepository = roundsRepository;
            _matchesRepository = matchesRepository;
            _matchDtoMapper = matchDtoMapper;
            _roundWithCategoriesDtoMapper = roundWithCategoriesDtoMapper;
        }

        public UseCaseResult<RoundWithCategoriesDto> Create(MatchDTO matchDto)
        {
            Match match;
            try
            {
                match = _matchesRepository.Get(id: matchDto.Id);
            }
            catch (MatchNotFoundByRespositoryException exception)
            {
                throw new RoundNotCreatedInUseCaseException(inner: exception);
            }

            if (match.IsActive == false)
            {
                throw new NewRoundForInactiveMatchUseCaseException(message: $"matchDto.Id: {matchDto.Id}");
            }

            return null;
        }
    }
}

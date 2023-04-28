using TopicTwister.NewRound.Models;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Mappers;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


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

        public Result<RoundWithCategoriesDto> Create(MatchDTO matchDto)
        {
            Result<Match> getMatchOperationResult = _matchesRepository.Get(id: matchDto.Id);
            if(getMatchOperationResult.WasOk == false)
            {
                return Result<RoundWithCategoriesDto>.Failure(errorMessage: getMatchOperationResult.ErrorMessage);
            }

            Match match = getMatchOperationResult.Outcome;
            if (match.IsActive == false)
            {
                return Result<RoundWithCategoriesDto>.Failure(
                    errorMessage: $"Cannot create new round for inactive match with id: {matchDto.Id}");
            }

            return Result<RoundWithCategoriesDto>.Success(outcome: null);
        }
    }
}

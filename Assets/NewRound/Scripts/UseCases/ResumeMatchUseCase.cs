using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.UseCases
{
    public class ResumeMatchUseCase : IResumeMatchUseCase
    {
        private ICreateRoundSubUseCase _createRoundSubUseCase;
        private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
        private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;

        public ResumeMatchUseCase(
            ICreateRoundSubUseCase createRoundSubUseCase,
            IMatchesReadOnlyRepository matchesReadOnlyRepository,
            IRoundsReadOnlyRepository roundsReadOnlyRepository,
            IdtoMapper<Round, RoundWithCategoriesDto> roundWithCategoriesDtoMapper)
        {
            _createRoundSubUseCase = createRoundSubUseCase;
            _matchesReadOnlyRepository = matchesReadOnlyRepository;
            _roundsReadOnlyRepository = roundsReadOnlyRepository;
            _roundWithCategoriesDtoMapper = roundWithCategoriesDtoMapper;
        }

        public Operation<RoundWithCategoriesDto> Execute(MatchDto matchDto) //TODO: Falta Test
        {
            Operation<RoundWithCategoriesDto> createRoundSubUseCaseOperation = _createRoundSubUseCase.Execute(matchDto);

            if(createRoundSubUseCaseOperation.WasOk == true)
            {
                return Operation<RoundWithCategoriesDto>.Success(result: createRoundSubUseCaseOperation.Result);
            }

            Match match = _matchesReadOnlyRepository.Get(matchDto.Id).Result;
            match = new Match(
                id: match.Id,
                startDateTime: match.StartDateTime,
                endDateTime: match.EndDateTime,
                rounds: _roundsReadOnlyRepository.GetMany(matchDto.Id).Result);

            RoundWithCategoriesDto roundWithCategoriesDto = _roundWithCategoriesDtoMapper.ToDTO(match.ActiveRound);

            return Operation<RoundWithCategoriesDto>.Success(result: roundWithCategoriesDto);
        }
    }
}

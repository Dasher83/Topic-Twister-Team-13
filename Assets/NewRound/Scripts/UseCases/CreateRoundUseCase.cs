using System.Collections.Generic;
using TopicTwister.NewRound.Models;
using TopicTwister.NewRound.Shared.Interfaces;
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
        private ICategoriesReadOnlyRepository _categoryRepository;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;

        public CreateRoundUseCase(
            IRoundsRepository roundsRepository,
            IMatchesRepository matchesRepository,
            ICategoriesReadOnlyRepository categoryRepository,
            IdtoMapper<Round, RoundWithCategoriesDto> roundWithCategoriesDtoMapper)
        {
            _roundsRepository = roundsRepository;
            _matchesRepository = matchesRepository;
            _categoryRepository = categoryRepository;
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

            if (match.IsValid == false)
            {
                return Result<RoundWithCategoriesDto>.Failure(
                    errorMessage: $"Cannot create new round for invalid match with id: {matchDto.Id}");
            }

            if (match.IsActive == false)
            {
                return Result<RoundWithCategoriesDto>.Failure(
                    errorMessage: $"Cannot create new round for inactive match with id: {matchDto.Id}");
            }

            Result<List<Category>> getRandomCategoriesOperationResult = _categoryRepository
                .GetRandomCategories(numberOfCategories: 5);

            if(getRandomCategoriesOperationResult.WasOk == false)
            {
                return Result<RoundWithCategoriesDto>.Failure(
                    errorMessage: getRandomCategoriesOperationResult.ErrorMessage);
            }

            Round round = new Round(
                roundNumber: 0,
                initialLetter: 'A',
                isActive: true,
                categories: getRandomCategoriesOperationResult.Outcome);

            Result<Round> saveRoundOperationResult = _roundsRepository.Save(round);

            if(saveRoundOperationResult.WasOk == false)
            {
                return Result<RoundWithCategoriesDto>.Failure(errorMessage: saveRoundOperationResult.ErrorMessage);
            }

            RoundWithCategoriesDto roundWithCategoriesDto = _roundWithCategoriesDtoMapper.ToDTO(model: round);

            return Result<RoundWithCategoriesDto>.Success(outcome: roundWithCategoriesDto);
        }
    }
}

using System.Collections.Generic;
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

            Round round = new Round(
                roundNumber: 0,
                initialLetter: 'A',
                isActive: true,
                categories: _categoryRepository.GetRandomCategories(5));



            return Result<RoundWithCategoriesDto>.Success(outcome: null);
        }
    }
}

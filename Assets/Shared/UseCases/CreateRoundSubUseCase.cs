using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.UseCases
{
    public class CreateRoundSubUseCase : ICreateRoundSubUseCase
    {
        private IRoundsRepository _roundsRepository;
        private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
        private ICategoriesReadOnlyRepository _categoryReadOnlyRepository;
        private ILetterReadOnlyRepository _letterReadOnlyRepository;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;

        public CreateRoundSubUseCase(
            IRoundsRepository roundsRepository,
            IMatchesReadOnlyRepository matchesReadOnlyRepository,
            ICategoriesReadOnlyRepository categoryReadOnlyRepository,
            ILetterReadOnlyRepository letterReadOnlyRepository,
            IdtoMapper<Round, RoundWithCategoriesDto> roundWithCategoriesDtoMapper)
        {
            _roundsRepository = roundsRepository;
            _matchesReadOnlyRepository = matchesReadOnlyRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _letterReadOnlyRepository = letterReadOnlyRepository;
            _roundWithCategoriesDtoMapper = roundWithCategoriesDtoMapper;
        }

        public Operation<RoundWithCategoriesDto> Create(MatchDto matchDto)
        {
            Operation<Match> getMatchOperationResult = _matchesReadOnlyRepository.Get(id: matchDto.Id);

            if(getMatchOperationResult.WasOk == false)
            {
                return Operation<RoundWithCategoriesDto>.Failure(errorMessage: getMatchOperationResult.ErrorMessage);
            }

            Match match = getMatchOperationResult.Outcome;

            Operation<List<Round>> getRoundsOperationResult = _roundsRepository.GetMany(matchId: match.Id);

            if (getRoundsOperationResult.WasOk == false)
            {
                return Operation<RoundWithCategoriesDto>.Failure(errorMessage: getRoundsOperationResult.ErrorMessage);
            }

            match = new Match(
                id: match.Id,
                startDateTime: match.StartDateTime,
                endDateTime: match.EndDateTime,
                rounds: getRoundsOperationResult.Outcome);

            if (match.IsValid == false)
            {
                return Operation<RoundWithCategoriesDto>.Failure(
                    errorMessage: $"Cannot create new round for invalid match with id: {matchDto.Id}");
            }

            if (match.IsActive == false)
            {
                return Operation<RoundWithCategoriesDto>.Failure(
                    errorMessage: $"Cannot create new round for inactive match with id: {matchDto.Id}");
            }

            if (match.AreAllRoundsCreated)
            {
                return Operation<RoundWithCategoriesDto>.Failure(
                    errorMessage: $"All rounds are already created for match with id: {matchDto.Id}");
            }

            Operation<List<Category>> getRandomCategoriesOperationResult = _categoryReadOnlyRepository
                .GetRandomCategories(numberOfCategories: 5);

            if(getRandomCategoriesOperationResult.WasOk == false)
            {
                return Operation<RoundWithCategoriesDto>.Failure(
                    errorMessage: getRandomCategoriesOperationResult.ErrorMessage);
            }

            Round round = new Round(
                roundNumber: match.Rounds.Count,
                initialLetter: _letterReadOnlyRepository.GetRandomLetter().Outcome,
                isActive: true,
                match: match,
                categories: getRandomCategoriesOperationResult.Outcome);

            Operation<Round> saveRoundOperationResult = _roundsRepository.Save(round);

            if(saveRoundOperationResult.WasOk == false)
            {
                return Operation<RoundWithCategoriesDto>.Failure(errorMessage: saveRoundOperationResult.ErrorMessage);
            }

            RoundWithCategoriesDto roundWithCategoriesDto = _roundWithCategoriesDtoMapper.ToDTO(
                model: saveRoundOperationResult.Outcome);

            return Operation<RoundWithCategoriesDto>.Success(outcome: roundWithCategoriesDto);
        }
    }
}

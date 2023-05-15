using System;
using System.Collections.Generic;
using System.Linq;


namespace TopicTwister.Shared.DTOs
{
    public class EndOfTurnDto
    {
        private MatchDto _matchDto;
        private RoundWithCategoriesDto _roundWithCategoriesDto;
        private UserMatchDto _userWithInitiativeMatchDto;
        private UserMatchDto _userWithoutInitiativeMatchDto;
        private List<UserRoundDto> _userWithInitiativeRoundDtos;
        private List<UserRoundDto> _userWithoutInitiativeRoundDtos;
        private List<AnswerDto> _answerDtosOfUserWithInitiative;
        private List<AnswerDto> _answerDtosOfUserWithoutInitiative;

        private EndOfTurnDto() {}

        public EndOfTurnDto(
            MatchDto matchDto,
            RoundWithCategoriesDto roundWithCategoriesDto,
            UserMatchDto userWithInitiativeMatchDto,
            UserMatchDto userWithoutInitiativeMatchDto,
            List<UserRoundDto> userWithInitiativeRoundDtos,
            List<UserRoundDto> userWithoutInitiativeRoundDtos,
            List<AnswerDto> answerDtosOfUserWithInitiative,
            List<AnswerDto> answerDtosOfUserWithoutInitiative)
        {
            _matchDto = matchDto;
            _roundWithCategoriesDto = roundWithCategoriesDto;
            _userWithInitiativeMatchDto = userWithInitiativeMatchDto;
            _userWithoutInitiativeMatchDto = userWithoutInitiativeMatchDto;
            _userWithInitiativeRoundDtos = userWithInitiativeRoundDtos;
            _userWithoutInitiativeRoundDtos = userWithoutInitiativeRoundDtos;
            _answerDtosOfUserWithInitiative = answerDtosOfUserWithInitiative;
            _answerDtosOfUserWithoutInitiative = answerDtosOfUserWithoutInitiative;
        }

        public MatchDto MatchDto => _matchDto;
        public RoundWithCategoriesDto RoundWithCategoriesDtos => _roundWithCategoriesDto;
        public UserMatchDto UserWithIniciativeMatchDto => _userWithInitiativeMatchDto;
        public UserMatchDto UserWithoutIniciativeMatchDto => _userWithoutInitiativeMatchDto;
        public List<UserRoundDto> UserWithIniciativeUserRoundDtos => _userWithInitiativeRoundDtos;
        public List<UserRoundDto> UserWithoutIniciativeUserRoundDtos => _userWithoutInitiativeRoundDtos;
        public List<AnswerDto> AnswerDtosOfUserWithIniciative => _answerDtosOfUserWithInitiative.ToList();
        public List<AnswerDto> AnswerDtosOfUserWithoutIniciative => _answerDtosOfUserWithoutInitiative.ToList();

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            EndOfTurnDto other = (EndOfTurnDto)obj;

            bool matchDtoEquals = _matchDto.Equals(other._matchDto);

            bool roundWithCategoriesDtoEquals = _roundWithCategoriesDto.Equals(other._roundWithCategoriesDto);

            bool userWithInitiativeMatchDtoEquals = _userWithInitiativeMatchDto
                .Equals(other._userWithInitiativeMatchDto);

            bool userWithoutInitiativeMatchDtoEquals = _userWithoutInitiativeMatchDto
                .Equals(other._userWithoutInitiativeMatchDto);

            bool userWithInitiativeRoundDtoEquals = Enumerable
                .SequenceEqual(_userWithInitiativeRoundDtos, other._userWithInitiativeRoundDtos);

            bool userWithoutInitiativeRoundDtoEquals = Enumerable
                .SequenceEqual(_userWithoutInitiativeRoundDtos, other._userWithoutInitiativeRoundDtos);

            bool answerDtosOfUserWithInitiativeEquals = Enumerable
                .SequenceEqual(_answerDtosOfUserWithInitiative, other._answerDtosOfUserWithInitiative);

            bool answerDtosOfUserWithoutInitiativeEquals = Enumerable
                .SequenceEqual(_answerDtosOfUserWithoutInitiative, other._answerDtosOfUserWithoutInitiative);

            return matchDtoEquals &&
                roundWithCategoriesDtoEquals &&
                userWithInitiativeMatchDtoEquals &&
                userWithoutInitiativeMatchDtoEquals &&
                userWithInitiativeRoundDtoEquals &&
                userWithoutInitiativeRoundDtoEquals &&
                answerDtosOfUserWithInitiativeEquals &&
                answerDtosOfUserWithoutInitiativeEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_matchDto, _roundWithCategoriesDto, _userWithInitiativeMatchDto,
                _userWithoutInitiativeMatchDto, _userWithInitiativeRoundDtos, _userWithoutInitiativeRoundDtos,
                _answerDtosOfUserWithInitiative, _answerDtosOfUserWithoutInitiative);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;


namespace TopicTwister.Shared.DTOs
{
    public class MatchFullStateDto
    {
        private MatchDto _matchDto;
        private List<RoundWithCategoriesDto> _roundWithCategoriesDtos;
        private UserMatchDto _userWithInitiativeMatchDto;
        private UserMatchDto _userWithoutInitiativeMatchDto;
        private List<UserRoundDto> _userWithInitiativeRoundDtos;
        private List<UserRoundDto> _userWithoutInitiativeRoundDtos;
        private List<AnswerDto> _answerDtosOfUserWithInitiative;
        private List<AnswerDto> _answerDtosOfUserWithoutInitiative;
        private List<TurnDto> _turnDtosOfUserWithInitiative;
        private List<TurnDto> _turnDtosOfUserWithoutInitiative;

        private MatchFullStateDto() {}

        public MatchFullStateDto(
            MatchDto matchDto,
            List<RoundWithCategoriesDto> roundWithCategoriesDtos,
            UserMatchDto userWithInitiativeMatchDto,
            UserMatchDto userWithoutInitiativeMatchDto,
            List<UserRoundDto> userWithInitiativeRoundDtos,
            List<UserRoundDto> userWithoutInitiativeRoundDtos,
            List<AnswerDto> answerDtosOfUserWithInitiative,
            List<AnswerDto> answerDtosOfUserWithoutInitiative,
            List<TurnDto> turnDtosOfUserWithInitiative,
            List<TurnDto> turnDtosOfUserWithoutInitiative)
        {
            _matchDto = matchDto;
            _roundWithCategoriesDtos = roundWithCategoriesDtos;
            _userWithInitiativeMatchDto = userWithInitiativeMatchDto;
            _userWithoutInitiativeMatchDto = userWithoutInitiativeMatchDto;
            _userWithInitiativeRoundDtos = userWithInitiativeRoundDtos;
            _userWithoutInitiativeRoundDtos = userWithoutInitiativeRoundDtos;
            _answerDtosOfUserWithInitiative = answerDtosOfUserWithInitiative;
            _answerDtosOfUserWithoutInitiative = answerDtosOfUserWithoutInitiative;
            _turnDtosOfUserWithInitiative = turnDtosOfUserWithInitiative;
            _turnDtosOfUserWithoutInitiative = turnDtosOfUserWithoutInitiative;
        }

        public MatchDto MatchDto => _matchDto;
        public List<RoundWithCategoriesDto> RoundWithCategoriesDtos => _roundWithCategoriesDtos.ToList();
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

            MatchFullStateDto other = (MatchFullStateDto)obj;

            bool matchDtoEquals = _matchDto.Equals(other._matchDto);

            bool roundWithCategoriesDtosEquals = Enumerable
                .SequenceEqual(_roundWithCategoriesDtos, other._roundWithCategoriesDtos);

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

            bool turnDtosOfUserWithInitiativeEquals = Enumerable
                .SequenceEqual(_turnDtosOfUserWithInitiative, other._turnDtosOfUserWithInitiative);

            bool turnDtosOfUserWithoutInitiative = Enumerable
                .SequenceEqual(_turnDtosOfUserWithoutInitiative, other._turnDtosOfUserWithoutInitiative);

            return matchDtoEquals &&
                roundWithCategoriesDtosEquals &&
                userWithInitiativeMatchDtoEquals &&
                userWithoutInitiativeMatchDtoEquals &&
                userWithInitiativeRoundDtoEquals &&
                userWithoutInitiativeRoundDtoEquals &&
                answerDtosOfUserWithInitiativeEquals &&
                answerDtosOfUserWithoutInitiativeEquals &&
                turnDtosOfUserWithInitiativeEquals &&
                turnDtosOfUserWithoutInitiative;
        }

        public override int GetHashCode()
        {
            int firstPart = HashCode.Combine(_matchDto, _roundWithCategoriesDtos, _userWithInitiativeMatchDto,
                _userWithoutInitiativeMatchDto, _userWithInitiativeRoundDtos, _userWithoutInitiativeRoundDtos,
                _answerDtosOfUserWithInitiative, _answerDtosOfUserWithoutInitiative);

            int secondPart = HashCode.Combine(_turnDtosOfUserWithInitiative, _turnDtosOfUserWithoutInitiative);

            return HashCode.Combine(firstPart, secondPart);
        }
    }
}

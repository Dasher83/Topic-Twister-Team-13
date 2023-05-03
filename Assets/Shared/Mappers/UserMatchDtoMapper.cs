using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class UserMatchDtoMapper: IdtoMapper<UserMatch, UserMatchDto>
    {
        private IMatchesRepository _matchesRepository;
        private IUserReadOnlyRepository _userRepository;

        public UserMatchDtoMapper(IMatchesRepository matchesRepository, IUserReadOnlyRepository userRepository)
        {
            _matchesRepository = matchesRepository;
            _userRepository = userRepository;
        }

        public UserMatchDto ToDTO(UserMatch userMatch )
        {
            return new UserMatchDto(
                score: userMatch.Score,
                isWinner: userMatch.IsWinner,
                hasInitiative: userMatch.HasInitiative,
                userId: userMatch.User.Id,
                matchId: userMatch.Match.Id
                );
        }

        public UserMatch FromDTO(UserMatchDto userMatchDTO)
        {
            return new UserMatch(
                score: userMatchDTO.Score,
                isWinner: userMatchDTO.IsWinner,
                hasInitiative: userMatchDTO.HasInitiative,
                user: _userRepository.Get(userMatchDTO.UserId).Outcome,
                match: _matchesRepository.Get(id: userMatchDTO.MatchId).Outcome
                );
        }

        public List<UserMatchDto> ToDTOs(List<UserMatch> userMatches)
        {
            return userMatches.Select(ToDTO).ToList();
        }

        public List<UserMatch> FromDTOs(List<UserMatchDto> userMatchesDTOs)
        {
            return userMatchesDTOs.Select(FromDTO).ToList();
        }
    }
}

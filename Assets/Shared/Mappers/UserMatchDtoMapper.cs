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
        private IUsersReadOnlyRepository _usersReadOnlyRepository;

        public UserMatchDtoMapper(
            IMatchesRepository matchesRepository,
            IUsersReadOnlyRepository usersReadOnlyRepository)
        {
            _matchesRepository = matchesRepository;
            _usersReadOnlyRepository = usersReadOnlyRepository;
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
                user: _usersReadOnlyRepository.Get(userMatchDTO.UserId).Result,
                match: _matchesRepository.Get(id: userMatchDTO.MatchId).Result
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

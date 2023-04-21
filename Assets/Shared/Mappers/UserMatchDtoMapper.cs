using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class UserMatchDtoMapper: IdtoMapper<UserMatch, UserMatchDTO>
    {
        private IMatchesRepository _matchesRepository;

        public UserMatchDtoMapper(IMatchesRepository matchesRepository)
        {
            _matchesRepository = matchesRepository;
        }

        public UserMatchDTO ToDTO(UserMatch userMatch)
        {
            return new UserMatchDTO(
                score: userMatch.Score,
                isWinner: userMatch.IsWinner,
                hasInitiative: userMatch.HasInitiative,
                userId: userMatch.UserId,
                matchId: userMatch.Match.Id
                );
        }

        public UserMatch FromDTO(UserMatchDTO userMatchDTO)
        {
            return new UserMatch(
                score: userMatchDTO.Score,
                isWinner: userMatchDTO.IsWinner,
                hasInitiative: userMatchDTO.HasInitiative,
                userId: userMatchDTO.UserId,
                match: _matchesRepository.Get(id: userMatchDTO.MatchId)
                );
        }

        public List<UserMatchDTO> ToDTOs(List<UserMatch> userMatches)
        {
            return userMatches.Select(ToDTO).ToList();
        }

        public List<UserMatch> FromDTOs(List<UserMatchDTO> userMatchesDTOs)
        {
            return userMatchesDTOs.Select(FromDTO).ToList();
        }
    }
}

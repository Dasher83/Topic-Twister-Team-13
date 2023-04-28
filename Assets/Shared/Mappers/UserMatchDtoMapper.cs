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
        private IUserRepository _userRepository;

        public UserMatchDtoMapper(IMatchesRepository matchesRepository, IUserRepository userRepository)
        {
            _matchesRepository = matchesRepository;
            _userRepository = userRepository;
        }

        public UserMatchDTO ToDTO(UserMatch userMatch )
        {
            return new UserMatchDTO(
                score: userMatch.Score,
                isWinner: userMatch.IsWinner,
                hasInitiative: userMatch.HasInitiative,
                userId: userMatch.User.Id,
                matchId: userMatch.Match.Id
                );
        }

        public UserMatch FromDTO(UserMatchDTO userMatchDTO)
        {
            return new UserMatch(
                score: userMatchDTO.Score,
                isWinner: userMatchDTO.IsWinner,
                hasInitiative: userMatchDTO.HasInitiative,
                user: _userRepository.Get(userMatchDTO.UserId).Outcome,
                match: _matchesRepository.Get(id: userMatchDTO.MatchId).Outcome
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

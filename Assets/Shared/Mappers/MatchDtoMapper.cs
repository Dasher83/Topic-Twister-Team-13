using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class MatchDtoMapper
    {
        public MatchDTO ToDTO(Match match)
        {
            return new MatchDTO(
                id: match.Id,
                startDateTime: match.StartDateTime,
                endDateTime: match.EndDateTime);
        }

        public Match FromDTO(MatchDTO matchDTO)
        {
            return new Match(
                id: matchDTO.Id,
                startDateTime: matchDTO.StartDateTime,
                endDateTime: matchDTO.EndDateTime);
        }

        public List<MatchDTO> ToDTOs(List<Match> matches)
        {
            return matches.Select(ToDTO).ToList();
        }

        public List<Match> FromDTOs(List<MatchDTO> matchesDTOs)
        {
            return matchesDTOs.Select(FromDTO).ToList();
        }
    }
}
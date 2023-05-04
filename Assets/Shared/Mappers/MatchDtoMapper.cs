using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class MatchDtoMapper: IdtoMapper<Match, MatchDto>
    {
        public MatchDto ToDTO(Match match)
        {
            return new MatchDto(
                id: match.Id,
                startDateTime: match.StartDateTime,
                endDateTime: match.EndDateTime);
        }

        public Match FromDTO(MatchDto matchDTO)
        {
            return new Match(
                id: matchDTO.Id,
                startDateTime: matchDTO.StartDateTime,
                endDateTime: matchDTO.EndDateTime);
        }

        public List<MatchDto> ToDTOs(List<Match> matches)
        {
            return matches.Select(ToDTO).ToList();
        }

        public List<Match> FromDTOs(List<MatchDto> matchesDTOs)
        {
            return matchesDTOs.Select(FromDTO).ToList();
        }
    }
}
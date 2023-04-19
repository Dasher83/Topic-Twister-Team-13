using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class MatchMapper
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
    }
}
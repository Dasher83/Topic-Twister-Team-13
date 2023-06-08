using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class TurnDtoMapper : IdtoMapper<Turn, TurnDto>
    {
        public Turn FromDTO(TurnDto DTO)
        {
            throw new System.NotImplementedException();
        }

        public List<Turn> FromDTOs(List<TurnDto> DTOs)
        {
            throw new System.NotImplementedException();
        }

        public TurnDto ToDTO(Turn turn)
        {
            return new TurnDto(
                userId: turn.User.Id,
                roundId: turn.Round.Id,
                points: turn.Points,
                startDateTime: turn.StartDateTime,
                endDateTime: turn.EndDateTime);
        }

        public List<TurnDto> ToDTOs(List<Turn> models)
        {
            throw new System.NotImplementedException();
        }
    }
}

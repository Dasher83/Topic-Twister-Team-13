using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class RoundDtoMapper : IdtoMapper<Round, RoundDto>
    {
        public Round FromDTO(RoundDto DTO)
        {
            throw new System.NotImplementedException();
        }

        public List<Round> FromDTOs(List<RoundDto> DTOs)
        {
            throw new System.NotImplementedException();
        }

        public RoundDto ToDTO(Round round)
        {
            return new RoundDto(
                id: round.Id,
                roundNumber: round.RoundNumber,
                initialLetter: round.InitialLetter,
                isActive: round.IsActive);
        }

        public List<RoundDto> ToDTOs(List<Round> models)
        {
            throw new System.NotImplementedException();
        }
    }
}
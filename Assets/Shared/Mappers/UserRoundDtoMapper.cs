using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class UserRoundDtoMapper : IdtoMapper<UserRound, UserRoundDto>
    {
        public UserRound FromDTO(UserRoundDto DTO)
        {
            throw new System.NotImplementedException();
        }

        public List<UserRound> FromDTOs(List<UserRoundDto> DTOs)
        {
            throw new System.NotImplementedException();
        }

        public UserRoundDto ToDTO(UserRound model)
        {
            UserRoundDto dto = new UserRoundDto(
                userId: model.User.Id,
                roundId: model.Round.Id,
                isWinner: model.IsWinner,
                points: model.Points);

            return dto;
        }

        public List<UserRoundDto> ToDTOs(List<UserRound> models)
        {
            return models.Select(ToDTO).ToList();
        }
    }
}

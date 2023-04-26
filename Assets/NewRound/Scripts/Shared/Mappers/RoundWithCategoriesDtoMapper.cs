using System.Collections.Generic;
using TopicTwister.NewRound.Models;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Shared.Mappers
{
    public class RoundWithCategoriesDtoMapper : IdtoMapper<Round, RoundWithCategoriesDtoMapper>
    {
        public Round FromDTO(RoundWithCategoriesDtoMapper DTO)
        {
            throw new System.NotImplementedException();
        }

        public List<Round> FromDTOs(List<RoundWithCategoriesDtoMapper> DTOs)
        {
            throw new System.NotImplementedException();
        }

        public RoundWithCategoriesDtoMapper ToDTO(Round model)
        {
            throw new System.NotImplementedException();
            //return new RoundWithCategoriesDto(roundDto: ) <--------------------- TODO
        }

        public List<RoundWithCategoriesDtoMapper> ToDTOs(List<Round> models)
        {
            throw new System.NotImplementedException();
        }
    }
}

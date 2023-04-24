using System.Collections.Generic;


namespace TopicTwister.Shared.Interfaces
{
    public interface IdtoMapper<Model, DTO>
    {
        public DTO ToDTO(Model model);
        public Model FromDTO(DTO DTO);
        public List<DTO> ToDTOs(List<Model> models);
        public List<Model> FromDTOs(List<DTO> DTOs);
    }
}
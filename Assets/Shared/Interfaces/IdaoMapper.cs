using System.Collections.Generic;


namespace TopicTwister.Shared.Interfaces
{
    public interface IdaoMapper<Model, DAO>
    {
        public DAO ToDAO(Model model);

        public Model FromDAO(DAO dao);

        public List<DAO> ToDAOs(List<Model> models);

        public List<Model> FromDAOs(List<DAO> daos);
    }
}
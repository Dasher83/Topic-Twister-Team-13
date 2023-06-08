using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class UserRoundDaoJsonMapper : IdaoMapper<UserRound, UserRoundDaoJson>
    {
        private IUsersReadOnlyRepository _usersReadOnlyRepository;
        private IRoundsReadOnlyRepository _roundsReadOnlyRepository;

        public UserRoundDaoJsonMapper(
            IUsersReadOnlyRepository usersReadOnlyRepository,
            IRoundsReadOnlyRepository roundsReadOnlyRepository)
        {
            _usersReadOnlyRepository = usersReadOnlyRepository;
            _roundsReadOnlyRepository = roundsReadOnlyRepository;
        }

        public UserRound FromDAO(UserRoundDaoJson dao)
        {
            User user = _usersReadOnlyRepository.Get(id: dao.UserId).Result;
            Round round = _roundsReadOnlyRepository.Get(id: dao.RoundId).Result;

            UserRound userRound = new UserRound(
                user: user,
                round: round,
                isWinner: dao.IsWinner,
                points: dao.Points);

            return userRound;
        }

        public List<UserRound> FromDAOs(List<UserRoundDaoJson> daos)
        {
            return daos.Select(FromDAO).ToList();
        }

        public UserRoundDaoJson ToDAO(UserRound model)
        {
            UserRoundDaoJson dao = new UserRoundDaoJson(
                userId: model.User.Id,
                roundId: model.Round.Id,
                isWinner: model.IsWinner,
                points: model.Points);

            return dao;
        }

        public List<UserRoundDaoJson> ToDAOs(List<UserRound> models)
        {
            return models.Select(ToDAO).ToList();
        }
    }
}

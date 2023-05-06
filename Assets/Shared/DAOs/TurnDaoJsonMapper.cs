using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.DAOs
{
    public class TurnDaoJsonMapper : IdaoMapper<Turn, TurnDaoJson>
    {
        IUsersReadOnlyRepository _usersReadOnlyRepository;
        IRoundsReadOnlyRepository _roundsReadOnlyRepository;

        public TurnDaoJsonMapper(
            IUsersReadOnlyRepository usersReadOnlyRepository,
            IRoundsReadOnlyRepository roundsReadOnlyRepository)
        {
            _usersReadOnlyRepository = usersReadOnlyRepository;
            _roundsReadOnlyRepository = roundsReadOnlyRepository;
        }

        public Turn FromDAO(TurnDaoJson turnDao)
        {
            User user = _usersReadOnlyRepository.Get(id: turnDao.UserId).Outcome;
            Round round = _roundsReadOnlyRepository.Get(id: turnDao.RoundId).Outcome;
            Turn turn = new Turn(user: user, round: round, startDateTime: turnDao.StartDateTime);
            return turn;
        }

        public List<Turn> FromDAOs(List<TurnDaoJson> daos)
        {
            return daos.Select(FromDAO).ToList();
        }

        public TurnDaoJson ToDAO(Turn turn)
        {
            TurnDaoJson turnDaoJson = new TurnDaoJson(
                userId: turn.User.Id,
                roundId: turn.Round.Id,
                startDateTime: turn.StartDateTime.ToString("s")); //ISO 8601

            return turnDaoJson;
        }

        public List<TurnDaoJson> ToDAOs(List<Turn> turns)
        {
            return turns.Select(ToDAO).ToList();
        }
    }
}

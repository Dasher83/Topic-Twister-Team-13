using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class RoundDaoJsonMapper : IdaoMapper<Round, RoundDaoJson>
    {
        IMatchesReadOnlyRepository _matchesRepository;
        ICategoriesReadOnlyRepository _categoriesRepository;

        public RoundDaoJsonMapper(
            IMatchesReadOnlyRepository matchesRepository,
            ICategoriesReadOnlyRepository categoriesRepository)
        {
            _matchesRepository = matchesRepository;
            _categoriesRepository = categoriesRepository;
        }

        public Round FromDAO(RoundDaoJson roundDao)
        {
            Match match = _matchesRepository.Get(roundDao.MatchId).Result;
            List<Category> categories = _categoriesRepository.GetMany(roundDao.CategoryIds).Result;

            return new Round(
                id: roundDao.Id,
                roundNumber: roundDao.RoundNumber,
                initialLetter: roundDao.InitialLetter,
                isActive: roundDao.IsActive,
                match: match,
                categories: categories);
        }

        public RoundDaoJson ToDAO(Round round)
        {
            RoundDaoJson roundDao = new RoundDaoJson(
                id: round.Id,
                roundNumber: round.RoundNumber,
                initialLetter: round.InitialLetter,
                isActive: round.IsActive,
                matchId: round.Match.Id,
                categoryIds: round.Categories.Select(category => category.Id).ToList());

            return roundDao;
        }

        public List<Round> FromDAOs(List<RoundDaoJson> roundDaos)
        {
            return roundDaos.Select(FromDAO).ToList();
        }

        public List<RoundDaoJson> ToDAOs(List<Round> rounds)
        {
            return rounds.Select(ToDAO).ToList();
        }
    }
}

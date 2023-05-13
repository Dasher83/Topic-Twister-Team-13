using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class AnswerDaoJsonMapper : IdaoMapper<Answer, AnswerDaoJson>
    {
        private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
        private ITurnsReadOnlyRepository _turnsReadOnlyRepository;

        private AnswerDaoJsonMapper() { }

        public AnswerDaoJsonMapper(
            ICategoriesReadOnlyRepository categoriesReadOnlyRepository,
            ITurnsReadOnlyRepository turnsReadOnlyRepository)
        {
            _categoriesReadOnlyRepository = categoriesReadOnlyRepository;
            _turnsReadOnlyRepository = turnsReadOnlyRepository;
        }

        public Answer FromDAO(AnswerDaoJson dao)
        {
            return new Answer(
                userInput: dao.UserInput,
                order: dao.Order,
                category: _categoriesReadOnlyRepository.Get(id: dao.CategoryId).Result,
                turn: _turnsReadOnlyRepository.Get(userId: dao.UserId, roundId: dao.RoundId).Result);
        }

        public List<Answer> FromDAOs(List<AnswerDaoJson> daos)
        {
            return daos.Select(FromDAO).ToList();
        }

        public AnswerDaoJson ToDAO(Answer answer)
        {
            return new AnswerDaoJson(
                userInput: answer.UserInput,
                order: answer.Order,
                categoryId: answer.Category.Id,
                userId: answer.Turn.User.Id,
                roundId: answer.Turn.Round.Id);
        }

        public List<AnswerDaoJson> ToDAOs(List<Answer> answers)
        {
            return answers.Select(ToDAO).ToList();
        }
    }
}

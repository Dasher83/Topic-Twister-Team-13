using System;
using System.Collections.Generic;
using TopicTwister.NewRound.Actions;
using TopicTwister.NewRound.Presenters;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Shared.Providers
{
    public class ActionsProvider
    {
        private readonly Dictionary<Type, object> _actions;

        public IAction ProvideAction(Type type)
        {
            _actions.TryGetValue(type, out object action);
            return (IAction)action;
        }

        public ActionsProvider()
        {
            _actions = new Dictionary<Type, object>();

            IGetNewRoundCategoriesUseCase useCase = new GetNewRoundCategoriesUseCase(
                categoryRepository: new CategoryRepositoryStub());

            object action = new GetRandomCategoriesAction(
                categoriesService: new CategoriesService(useCase));

            _actions.Add(typeof(GetRandomCategoriesAction), action);
        }
    }
}

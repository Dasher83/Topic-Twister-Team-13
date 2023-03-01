using System;
using System.Collections.Generic;
using TopicTwister.NewRound.Actions;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Services;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Shared.Providers
{
    public class ActionProvider<T> where T : IAction
    {
        private readonly Dictionary<Type, object> _actions = new()
        {
            {
                typeof(GetRandomCategoriesAction),
                new GetRandomCategoriesAction(
                    categoriesService: new CategoriesService(
                        new GetNewRoundCategoriesUseCase(
                            categoryRepository: new CategoryRepositoryJson())))
            }
        };

        public T Provide()
        {
            _actions.TryGetValue(typeof(T), out object action);
            return (T)action;
        }
    }
}

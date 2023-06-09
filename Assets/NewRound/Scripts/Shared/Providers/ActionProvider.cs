using System;
using System.Collections.Generic;
using TopicTwister.NewRound.Actions;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Services;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Shared.Providers
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
                            categoryRepository: new CategoriesRepositoryJson())))
            },
            {
                typeof(GetShuffledLetterAction),
                new GetShuffledLetterAction(
                    letterService: new LetterService(
                        new ShuffleLetterUseCase()))
            }
        };

        public T Provide()
        {
            _actions.TryGetValue(typeof(T), out object action);
            return (T)action;
        }
    }
}

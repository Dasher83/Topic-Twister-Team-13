using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;
using TopicTwister.NewRound.Shared;
using NUnit.Framework;
using System.Linq;

public class TestGetNewRoundCategoriesUseCase
{
    [Test]
    public void Test_get_new_round_categories_use_case_with_json_and_randomness()
    {
        IGetNewRoundCategoriesUseCase useCase = new GetNewRoundCategoriesUseCase(new CategoriesRepositoryJson());

        CategoryDTO[] actualResult = useCase.GetRandomCategories(Constants.Categories.CategoriesPerRound);
        Assert.IsTrue(new CategoriesRepositoryJson().Exists(actualResult.Select(c => c.Name).ToArray()));
        int cycles = 100;

        for (int i = 0; i < cycles; i++)
        {
            CategoryDTO[] duplicateResult = useCase.GetRandomCategories(Constants.Categories.CategoriesPerRound);
            Assert.IsTrue(new CategoriesRepositoryJson().Exists(actualResult.Select(c => c.Name).ToArray()));

            try
            {
                Assert.IsTrue(!actualResult.SequenceEqual(duplicateResult));
            }
            catch(AssertionException ex)
            {
                if(i == cycles - 1)
                {
                    throw ex;
                }
                continue;
            }

            break;
        }
    }
}


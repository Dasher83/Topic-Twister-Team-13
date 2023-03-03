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
    public void TestGetNewRoundCategoriesUseCaseSimplePassesSTUB()
    {
        IGetNewRoundCategoriesUseCase useCase = new GetNewRoundCategoriesUseCase(new CategoryRepositoryStub());

        CategoryDTO[] actualResult = useCase.GetRandomCategories(Constants.Categories.CategoriesPerRound);
        
        CategoryDTO[] expectedResult = new[]{
                new CategoryDTO("1", "Colores"  ),
                new CategoryDTO("2", "Animales" ),
                new CategoryDTO("3", "Paises"   ),
                new CategoryDTO("4", "Plantas"  ),
                new CategoryDTO("5", "Peliculas")
            };

        Assert.AreEqual(expectedResult, actualResult);
    }
    
    [Test]
    public void TestGetNewRoundCategoriesUseCaseWithJsonReturnsLengthOfFive()
    {
        IGetNewRoundCategoriesUseCase useCase = new GetNewRoundCategoriesUseCase(new CategoryRepositoryJsonStub());

        CategoryDTO[] actualResult = useCase.GetRandomCategories(Constants.Categories.CategoriesPerRound);

        int expectedResultLength = 5;

        Assert.AreEqual(expectedResultLength, actualResult.Length);
    }

    [Test]
    public void TestGetNewRoundCategoriesUseCaseWithJsonReturnsFiveElements()
    {
        IGetNewRoundCategoriesUseCase useCase = new GetNewRoundCategoriesUseCase(new CategoryRepositoryJsonStub());

        CategoryDTO[] actualResult = useCase.GetRandomCategories(Constants.Categories.CategoriesPerRound);

        string[] expectedResult = { "Colores", "Animales", "Países", "Plantas", "Películas" };

        for(int i = 0; i < actualResult.Length; i++)
        {
            Assert.AreEqual(expectedResult[i], actualResult[i].Name);
        }
    }

    [Test]
    public void TestGetNewRoundCategoriesUseCaseWithJsonAndRandomness()
    {
        IGetNewRoundCategoriesUseCase useCase = new GetNewRoundCategoriesUseCase(new CategoryRepositoryJson());

        CategoryDTO[] actualResult = useCase.GetRandomCategories(Constants.Categories.CategoriesPerRound);
        Assert.IsTrue(new CategoryRepositoryJson().Exists(actualResult.Select(c => c.Name).ToArray()));
        int cycles = 100;

        for (int i = 0; i < cycles; i++)
        {
            CategoryDTO[] duplicateResult = useCase.GetRandomCategories(Constants.Categories.CategoriesPerRound);
            Assert.IsTrue(new CategoryRepositoryJson().Exists(actualResult.Select(c => c.Name).ToArray()));

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


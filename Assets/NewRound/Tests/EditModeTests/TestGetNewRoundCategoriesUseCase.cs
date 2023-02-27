using NUnit.Framework;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared;


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
    public void TestGetNewRoundCategoriesUseCaseWithScriptablesReturnsFiveElements()
    {
        IGetNewRoundCategoriesUseCase useCase = new GetNewRoundCategoriesUseCase(new CategoryRepositoryScriptable());

        CategoryDTO[] actualResult = useCase.GetRandomCategories(Constants.Categories.CategoriesPerRound);

        int expectedResultLength = 5;

        Assert.AreEqual(expectedResultLength, actualResult.Length);
    }
}


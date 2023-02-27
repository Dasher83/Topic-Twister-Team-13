using System;
using System.Collections.Generic;
using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using UnityEngine;

public class CategoryRepositoryScriptable : ICategoryRepository
{
    private CategoriesScriptable _categoriesData;

    public CategoryRepositoryScriptable()
    {
        _categoriesData = ScriptableObject.CreateInstance<CategoriesScriptable>();
        _categoriesData.AddCategory(new Category(name: "Colores"));
        _categoriesData.AddCategory(new Category(name: "Animales"));
        _categoriesData.AddCategory(new Category(name: "Paises"));
        _categoriesData.AddCategory(new Category(name: "Plantas"));
        _categoriesData.AddCategory(new Category(name: "Peliculas"));
    }

    public CategoryDTO[] GetRandomCategories(int numberOfCategories)
    {
        List<CategoryDTO> result = new List<CategoryDTO>();
        List<Category> categories = _categoriesData.GetRandomCategories(numberOfCategories: 5);

        foreach (Category category in categories)
        {
            result.Add(category.ToDTO());
        }
        return result.ToArray();
    }
}

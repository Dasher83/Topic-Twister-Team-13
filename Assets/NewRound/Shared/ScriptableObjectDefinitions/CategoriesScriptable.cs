using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "CategoriesData", menuName = "ScriptableObjects/CategoriesScriptable", order = 0)]
public class CategoriesScriptable : ScriptableObject
{
    [SerializeField] private List<Category> _categories = new List<Category>();

    public void AddCategory(Category category)
    {
        _categories.Add(category);
    }

    public List<Category> GetRandomCategories(int numberOfCategories)
    {
        return _categories.ToList();
    }
}



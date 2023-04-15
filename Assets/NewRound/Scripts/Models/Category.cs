using System;
using TopicTwister.Shared.DTOs;
using UnityEngine;


[Serializable]
public class Category
{
    private readonly string _id;
    [SerializeField] private string _name;

    public string Id => _id;
    public string Name => _name;

    private Category() { }

    public Category(string name)
    {
        _id = Guid.NewGuid().ToString();
        _name = name;
    }

    public CategoryDTO ToDTO()
    {
        return new CategoryDTO(id: _id, name: _name);
    }
}

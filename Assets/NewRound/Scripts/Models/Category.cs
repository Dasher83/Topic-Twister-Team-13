public class Category
{
    private readonly int _id;
    private string _name;

    public int Id => _id;
    public string Name => _name;

    private Category() { }

    public Category(string name)
    {
        _name = name;
    }

    public Category(int id, string name) : this(name)
    {
        _id = id;
    }
}

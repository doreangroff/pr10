namespace pr10.Entitites;

public class GoodCategory
{
    public GoodCategory(int categoryId, string categoryName)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
    }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
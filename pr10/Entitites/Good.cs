namespace pr10.Entitites;

public class Good
{
    public string ArticleNumber { get; set; }
    public string GoodName { get; set; }
    public string Measurment { get; set; }
    public int MeasurmentId { get; set; }
    public int Cost { get; set; }
    public int MaxDiscount { get; set; }
    public string Manufacturer { get; set; }
    public int ManufacturerId { get; set; }
    public string Supplier { get; set; }
    public int SupplierId { get; set; }
    public string Category { get; set; }
    public int CategoryId { get; set; }
    public int CurrentDiscount { get; set; }
    public int QuantityInStock { get; set; }
    public string Description { get; set; }
    public byte[]? Image { get; set; }
}
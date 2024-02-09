namespace pr10.Entitites;

public class Supplier
{
    public Supplier(int sipplierId, string supplierName)
    {
        SipplierId = sipplierId;
        SupplierName = supplierName;
    }

    public int SipplierId { get; set; }
    public string SupplierName { get; set; }
}
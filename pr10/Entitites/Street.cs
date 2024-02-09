namespace pr10.Entitites;

public class Street
{
    public Street(int streetId, string streetName)
    {
        StreetId = streetId;
        StreetName = streetName;
    }

    public int StreetId { get; set; }
    public string StreetName { get; set; }
}
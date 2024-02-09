namespace pr10.Entitites;

public class City
{
    public City(int cityId, string cityName)
    {
        CityId = cityId;
        CityName = cityName;
    }

    public int CityId { get; set; }
    public string CityName { get; set; }
}
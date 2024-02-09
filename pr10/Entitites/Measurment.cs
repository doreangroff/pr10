namespace pr10.Entitites;

public class Measurment
{
    public Measurment(int measurmentId, string measurmentName)
    {
        MeasurmentId = measurmentId;
        MeasurmentName = measurmentName;
    }

    public int MeasurmentId { get; set; }
    public string MeasurmentName { get; set; }
}
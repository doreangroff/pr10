namespace pr10.Entitites;

public class OrderStatus
{
    public OrderStatus(int statusId, string statusName)
    {
        StatusId = statusId;
        StatusName = statusName;
    }

    public int StatusId { get; set; }
    public string StatusName { get; set; }
}
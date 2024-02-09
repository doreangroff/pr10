using System;

namespace pr10.Entitites;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public int PickUpPoint { get; set; }
    public string Client { get; set; }
    public int ClientId { get; set; }
    public int Code { get; set; }
    public int OrderStatusId { get; set; }
    public string OrderStatus { get; set; }
}
namespace KolokwiumAPBDCodeFirst.Entities;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
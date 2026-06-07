using KolokwiumAPBDCodeFirst.Data;

using KolokwiumAPBDCodeFirst.DTOs;
using KolokwiumAPBDCodeFirst.Services;
using Microsoft.EntityFrameworkCore;
namespace KolokwiumAPBDCodeFirst;
public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    public OrderService(AppDbContext context)
    {
        _context = context;

    }
    public async Task<object?> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Payments)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null)
            return null;
        return new
        {
            order.OrderId,
            order.OrderDate,
            order.Status,
            order.TotalAmount,
            user = order.User.Username,
            payments = order.Payments.Select(p => new
            {
                p.PaymentId,
                p.PaymentMethod,
                p.Amount,
                p.PaymentStatus
            }),
            orderItems = order.OrderItems.Select(i => new
            {
                product = new
                {
                    i.Product.ProductId,
                    i.Product.Name,
                    i.Product.Description,
                    i.Product.Price,
                    i.Product.StockQuantity
                },
                i.Quantity,
                i.Price
            })
        };
    }

    public async Task<bool> ProcessOrder(UpdateOrderDto dto)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();
        var order = await _context.Orders
            .Include(o => o.Payments)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);
        if (order == null)
            return false;
        if (!order.Payments.Any())
            return false;
        order.Status = "Processed";
        decimal total = 0;
        foreach (var item in order.OrderItems)
        {
            item.Product.Price *= 0.9m;
            total += item.Product.Price * item.Quantity;
        }
        order.TotalAmount = total;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return true;
    }
}
 
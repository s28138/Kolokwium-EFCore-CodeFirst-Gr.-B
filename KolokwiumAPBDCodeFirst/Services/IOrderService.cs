using KolokwiumAPBDCodeFirst.DTOs;

namespace KolokwiumAPBDCodeFirst.Services;

public interface IOrderService
{
    Task<object?> GetOrder(int orderId);
    Task<bool> ProcessOrder(UpdateOrderDto dto);
}
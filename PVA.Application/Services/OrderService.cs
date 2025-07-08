using Microsoft.Extensions.Logging;
using PVA.Core.Entities;
using PVA.Core.Enums;
using PVA.Core.Interfaces;

namespace PVA.Application.Services;

/// <summary>
/// Order service implementation for order processing operations
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        try
        {
            _logger.LogInformation("Getting order by ID: {OrderId}", orderId);
            return await _orderRepository.GetByIdAsync(orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order by ID: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
    {
        try
        {
            _logger.LogInformation("Getting order by number: {OrderNumber}", orderNumber);
            return await _orderRepository.GetByOrderNumberAsync(orderNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order by number: {OrderNumber}", orderNumber);
            throw;
        }
    }

    public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
    {
        try
        {
            _logger.LogInformation("Getting orders for customer: {CustomerId}", customerId);
            return await _orderRepository.GetOrdersByCustomerAsync(customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders for customer: {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        try
        {
            _logger.LogInformation("Getting orders by status: {Status}", status);
            return await _orderRepository.GetOrdersByStatusAsync(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders by status: {Status}", status);
            throw;
        }
    }

    public async Task<Order> CreateOrderFromCartAsync(int customerId, Address shippingAddress, Address? billingAddress = null)
    {
        try
        {
            _logger.LogInformation("Creating order from cart for customer: {CustomerId}", customerId);

            // Get customer with cart items
            var customer = await _customerRepository.GetCustomerWithCartAsync(customerId);
            if (customer == null)
            {
                throw new InvalidOperationException($"Customer with ID {customerId} not found");
            }

            if (customer.CartItems == null || !customer.CartItems.Any())
            {
                throw new InvalidOperationException("Customer cart is empty");
            }

            // Calculate order total
            decimal orderTotal = 0;
            var orderItems = new List<OrderItem>();

            foreach (var cartItem in customer.CartItems)
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {cartItem.ProductId} not found");
                }

                // Check stock availability
                if (product.StockQuantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}. Available: {product.StockQuantity}, Requested: {cartItem.Quantity}");
                }

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Product = product,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * cartItem.Quantity
                };

                orderItems.Add(orderItem);
                orderTotal += orderItem.TotalPrice;
            }

            // Create the order
            var order = new Order
            {
                CustomerId = customerId,
                OrderNumber = await GenerateOrderNumberAsync(),
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = orderTotal,
                Items = orderItems
            };

            // Copy shipping address
            order.ShippingFirstName = shippingAddress.FirstName;
            order.ShippingLastName = shippingAddress.LastName;
            order.ShippingAddressLine1 = shippingAddress.AddressLine1;
            order.ShippingAddressLine2 = shippingAddress.AddressLine2;
            order.ShippingCity = shippingAddress.City;
            order.ShippingState = shippingAddress.State;
            order.ShippingPostalCode = shippingAddress.PostalCode;
            order.ShippingCountry = shippingAddress.Country;
            order.ShippingPhoneNumber = shippingAddress.PhoneNumber;

            // Copy billing address
            var billing = billingAddress ?? shippingAddress;
            order.BillingFirstName = billing.FirstName;
            order.BillingLastName = billing.LastName;
            order.BillingAddressLine1 = billing.AddressLine1;
            order.BillingAddressLine2 = billing.AddressLine2;
            order.BillingCity = billing.City;
            order.BillingState = billing.State;
            order.BillingPostalCode = billing.PostalCode;
            order.BillingCountry = billing.Country;
            order.BillingPhoneNumber = billing.PhoneNumber;

            // Save the order
            await _orderRepository.AddAsync(order);

            // Update product stock quantities
            foreach (var cartItem in customer.CartItems)
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= cartItem.Quantity;
                    _productRepository.Update(product);
                }
            }

            // Clear the customer's cart
            customer.CartItems.Clear();
            _customerRepository.Update(customer);

            // Save all changes
            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation("Order created successfully with number: {OrderNumber}", order.OrderNumber);
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order from cart for customer: {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        try
        {
            _logger.LogInformation("Updating order status for order: {OrderId} to {Status}", orderId, status);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {orderId} not found");
            }

            order.Status = status;
            order.UpdatedDate = DateTime.UtcNow;

            // Handle status-specific business logic
            switch (status)
            {
                case OrderStatus.Shipped:
                    order.ShippedDate = DateTime.UtcNow;
                    break;
                case OrderStatus.Delivered:
                    order.DeliveredDate = DateTime.UtcNow;
                    break;
                case OrderStatus.Cancelled:
                    // Restore stock quantities
                    await RestoreStockQuantitiesAsync(order);
                    break;
            }

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Order status updated successfully for order: {OrderId}", orderId);
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order status for order: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<Order> UpdatePaymentStatusAsync(int orderId, PaymentStatus paymentStatus)
    {
        try
        {
            _logger.LogInformation("Updating payment status for order: {OrderId} to {PaymentStatus}", orderId, paymentStatus);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {orderId} not found");
            }

            order.PaymentStatus = paymentStatus;
            order.UpdatedDate = DateTime.UtcNow;

            if (paymentStatus == PaymentStatus.Captured)
            {
                // If payment is successful and order is still pending, move to processing
                if (order.Status == OrderStatus.Pending)
                {
                    order.Status = OrderStatus.Processing;
                }
            }

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Payment status updated successfully for order: {OrderId}", orderId);
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment status for order: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<bool> CancelOrderAsync(int orderId)
    {
        try
        {
            _logger.LogInformation("Cancelling order: {OrderId}", orderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning("Order not found for cancellation: {OrderId}", orderId);
                return false;
            }

            // Check if order can be cancelled
            if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered)
            {
                throw new InvalidOperationException("Cannot cancel an order that has already been shipped or delivered");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                return true; // Already cancelled
            }

            // Update order status
            order.Status = OrderStatus.Cancelled;
            order.UpdatedDate = DateTime.UtcNow;

            // Restore stock quantities
            await RestoreStockQuantitiesAsync(order);

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Order cancelled successfully: {OrderId}", orderId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling order: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<decimal> CalculateOrderTotalAsync(int customerId)
    {
        try
        {
            _logger.LogInformation("Calculating order total for customer: {CustomerId}", customerId);

            var customer = await _customerRepository.GetCustomerWithCartAsync(customerId);
            if (customer == null || customer.CartItems == null || !customer.CartItems.Any())
            {
                return 0;
            }

            decimal total = 0;
            foreach (var cartItem in customer.CartItems)
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product != null)
                {
                    total += product.Price * cartItem.Quantity;
                }
            }

            return total;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating order total for customer: {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        try
        {
            // Generate order number in format: PVA-YYYYMMDD-XXXXXX
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var random = new Random();
            var sequence = random.Next(100000, 999999);
            
            var orderNumber = $"PVA-{date}-{sequence}";
            
            // Ensure uniqueness by checking if order number already exists
            var existingOrder = await _orderRepository.GetByOrderNumberAsync(orderNumber);
            if (existingOrder != null)
            {
                // If collision occurs, try again with a different sequence
                sequence = random.Next(100000, 999999);
                orderNumber = $"PVA-{date}-{sequence}";
            }

            return orderNumber;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating order number");
            throw;
        }
    }

    private async Task RestoreStockQuantitiesAsync(Order order)
    {
        if (order.Items == null) return;

        foreach (var orderItem in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
            if (product != null)
            {
                product.StockQuantity += orderItem.Quantity;
                _productRepository.Update(product);
            }
        }
        
        await _productRepository.SaveChangesAsync();
    }
}

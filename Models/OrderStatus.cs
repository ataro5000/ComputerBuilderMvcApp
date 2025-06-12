namespace ComputerBuilderMvcApp.Models
{
    public enum OrderStatus
    {
        Pending,        // Order placed, awaiting processing
        Processing,     // Order is being prepared
        Shipped,        // Order has been shipped
        Delivered,      // Order has been delivered
        Cancelled,      // Order was cancelled
        PaymentFailed   // Payment attempt failed
    }
}
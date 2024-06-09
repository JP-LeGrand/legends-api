namespace legend.Entities.Enums
{
    public enum OrderStatus
    {
        NotSet,      // Order has been placed but not yet processed
        Pending,      // Order has been placed but not yet processed
        Processed,   // Order is processed
        Shipped,      // Order has been shipped
        Delivered,    // Order has been delivered to the customer
        Cancelled     // Order has been cancelled
    }

}

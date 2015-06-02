namespace OnlineStore.ServiceContracts.ModelDTOs
{
    public enum OrderStatusDto : int
    {
        Created = 0,
        Paid = 1,
        Picked = 2,
        Dispatched = 3,
        Delivered = 4
    }
}
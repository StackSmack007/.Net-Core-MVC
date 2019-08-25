namespace ChushkaApp.DTOS.Orders
{
    public class orderInfoDTO
    {
        public int OrderId { get; set; }
        public string Customer { get; set; }
        public string Product { get; set; }
        public string OrderedOn { get; set; }
    }
}
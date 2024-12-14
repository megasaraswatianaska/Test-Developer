namespace WebCustomer.ViewModel.DataSales
{
    public class DataItemViewModel
    {
        public long? SoItemId { get; set; }
        public long SoOrderId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DataItemViewModel()
        {
            
        }
        public DataItemViewModel(Models.SoItem x)
        {
            SoItemId = x.SoItemId;
            SoOrderId = x.SoOrderId;    
            ItemName = x.ItemName;
            Quantity = x.Quantity;
            Price = x.Price;
        }
    }
}

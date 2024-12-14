namespace WebCustomer.ViewModel.DataSales
{
    public class DataOrderAddViewModel
    {
        public long? SoOrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int ComCustomerId { get; set; }
        public string CustomerNama { get; set; }
        public string Address { get; set; }
        public List<DataItemViewModel> Item { get; set; } = new List<DataItemViewModel>();
        public DataOrderAddViewModel()
        {

        }
        public DataOrderAddViewModel(Models.SoOrder x)
        {
            SoOrderId = x.SoOrderId;
            OrderNo = x.OrderNo;
            OrderDate = x.OrderDate;
            ComCustomerId = x.ComCustomerId;
            Item = x.SoItems.Select(x=> new DataItemViewModel(x)).ToList();        }
    }
}

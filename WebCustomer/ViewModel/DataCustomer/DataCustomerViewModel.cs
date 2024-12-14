namespace WebCustomer.ViewModel.DataCustomer
{
    public class DataCustomerViewModel
    {
        public int ComCustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DataCustomerViewModel()
        {
            
        }
        public DataCustomerViewModel(Models.ComCustomer x)
        {
            ComCustomerId = x.ComCustomerId;
            CustomerName = x.CustomerName;
        }
    }
}

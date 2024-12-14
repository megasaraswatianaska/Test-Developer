namespace WebCustomer.Services
{
    public interface IDataSalesServices
    {
        Task<List<Models.SoOrder>> ListAll(string? Keywordsearch, DateTime? datesearch);
        Task<List<Models.ComCustomer>> ListCustomer();
        Task<Models.SoOrder> Edit(string id);
        Task<bool> Tambah(Models.SoOrder data);
        Task<bool> Ubah(Models.SoOrder data);
        Task<bool> Delete(Models.SoOrder data);
        Task<string> GenerateCode();
    }
}

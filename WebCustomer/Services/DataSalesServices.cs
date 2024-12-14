using Microsoft.EntityFrameworkCore;
using WebCustomer.Models;

namespace WebCustomer.Services
{
    public class DataSalesServices : IDataSalesServices
    {

        private readonly AppDbContext db;
        public DataSalesServices(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<List<SoOrder>> ListAll(string? Keywordsearch, DateTime? datesearch)
        {
            var data = this.db.SoOrders
                                .AsNoTrackingWithIdentityResolution()
                                .Include(x => x.SoItems)
                                .Include(y => y.ComCustomer)
                                .ToList();

            if (Keywordsearch != null)
            {
                data = data.Where(x => x.OrderDate.Date == datesearch.GetValueOrDefault().Date ||
                                      x.OrderNo.ToLower().Contains(Keywordsearch.ToLower()) ||
                                      x.Address.ToLower().Contains(Keywordsearch.ToLower()) ||
                                      (x.ComCustomer != null ? x.ComCustomer.CustomerName.ToLower().Contains(Keywordsearch.ToLower()) : false)).ToList();
            }
            return data;
        }

        public async Task<List<ComCustomer>> ListCustomer()
        {
            var data = this.db.ComCustomers
                                 .ToList();
            return data;
        }

        public async Task<string> GenerateCode()
        {
            string nextID = "";
            string tgl = "50_";
            int x = 0;
            int maxLength = 4;
            var lastID = db.SoOrders
                .Where(x => EF.Functions.Like(x.OrderNo, $"{tgl}%"))
                .Max(x => x.OrderNo);
            if (lastID is not null)
            {
                x = Int32.Parse(lastID.ToString().Substring(3, 4)) + 1;
                int xLength = x.ToString().Length;
                int max = maxLength - xLength;

                for (int i = 0; i < max; i++)
                {
                    nextID += "0";
                }
                nextID += x.ToString();
            }
            else
            {
                nextID = "0001";
            }
            return $"{tgl}{nextID}";
        }

        public async Task<SoOrder> Edit(string id)
        {
            SoOrder data = db.SoOrders
                                 .Include(x => x.SoItems)
                                 .Where(x => x.SoOrderId == long.Parse(id))
                                 .FirstOrDefault();
            return data;
        }

        public async Task<bool> Delete(SoOrder data)
        {
            try
            {
                var selected = db.SoOrders
                            .AsNoTracking()
                            .Where(x => x.SoOrderId == data.SoOrderId)
                            .FirstOrDefault();
                if (selected is not null)
                {
                    db.SoOrders.Remove(data);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {
                string errmsg = err.ToString();
                return false;
            }
        }


        public async Task<bool> Tambah(SoOrder data)
        {

            try
            {
                using (var transaction = this.db.Database.BeginTransaction())
                {


                    foreach (var item in data.SoItems)
                    {

                        item.SoItemId = null;
                    }

                    db.SoOrders.Add(data);
                    db.SaveChanges();

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception e)
            {

                string msgError = e.ToString();
                return false;
            }
        }

        public async Task<bool> Ubah(SoOrder data)
        {
            List<SoItem> listItem = data.SoItems.ToList();
            try
            {
                using (var transaction = this.db.Database.BeginTransaction())
                {
                    var dataedit = db.SoOrders
                                     .Include(x => x.SoItems)
                                    .Where(x => x.SoOrderId == data.SoOrderId)
                                    .FirstOrDefault();


                    dataedit.OrderNo = data.OrderNo;
                    dataedit.OrderDate = data.OrderDate;
                    dataedit.ComCustomerId = data.ComCustomerId;
                    dataedit.Address = data.Address;


                    dataedit.SoItems.Clear();
                    foreach (var item in listItem)
                    {
                        item.SoItemId = null;
                        dataedit.SoItems.Add(item);
                    }

                    db.SaveChanges();

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception e)
            {

                string msgError = e.ToString();
                return false;
            }
        }

    }
}

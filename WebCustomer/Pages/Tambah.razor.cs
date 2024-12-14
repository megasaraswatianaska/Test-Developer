using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using WebCustomer.Models;
using WebCustomer.Services;
using WebCustomer.ViewModel.DataSales;

namespace WebCustomer.Pages
{
    public partial class Tambah : ComponentBase, IAsyncDisposable
    {

        #region Parameter
        [CascadingParameter] protected App app { get; set; }
        [Parameter] public string? id { get; set; }
        #endregion
        #region Dependency Injection
        [Inject] protected IConfiguration config { get; set; }
        [Inject] protected ILogger<App> logger { get; set; }
        [Inject] protected IJSRuntime js { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        [Inject] protected IDataSalesServices mainSvc { get; set; }
        #endregion
        #region Service
        #endregion
        #region Component
        protected SfGrid<SoItem> mainGrid { get; set; }
        #endregion
        #region ViewModel
        protected LoadingStatus isLoading { get; set; } = new LoadingStatus();
        protected SoOrder tambahViewModel { get; set; } = new SoOrder ();
        #endregion
        #region List

        protected List<ItemModel> Toolbaritems = new List<ItemModel>();
        protected List<ComCustomer> listCustomer { get; set; } = new();
        protected List<SoItem> listItem { get; set; } = new();
        #endregion
        #region Properties
        bool debug = false;
        readonly Lazy<Task<IJSObjectReference>> mainJS;
        #endregion


        protected override async Task OnInitializedAsync()
        {
            
           
            tambahViewModel.OrderDate = DateTime.Today;
            await GetKode();
            await GetDaftarCustomer();
            await GetData();
            Boolean.TryParse(config["Debug"], out this.debug);
        }
        protected async Task GetKode()
        {
            isLoading.Data = true;
            StateHasChanged();

            tambahViewModel.OrderNo = await mainSvc.GenerateCode();
            isLoading.Data = false;
            StateHasChanged();
        }
        protected async Task GetDaftarCustomer()
        {
            isLoading.Data = true;
            StateHasChanged();

            listCustomer = await mainSvc.ListCustomer();
            isLoading.Data = false;
            StateHasChanged();
        }
        protected async Task GetData()
        {
            if (id != null)
            {
                tambahViewModel = await mainSvc.Edit(id);

                listItem = tambahViewModel.SoItems
                                .ToList();
            }
        }
        #region Event Listener
        public async void mainGridTable_OnCommandClicked(CommandClickEventArgs<SoItem> args)
        {
            switch (args.CommandColumn.ID)
            {
                case "BtnDelete":
                    var selected = args.RowData;
                    this.tambahViewModel.SoItems.Remove(args.RowData);
                    this.mainGrid.Refresh();
                    break;
            }
        }
        public async void mainGridTable_ActionBegin(ActionEventArgs<SoItem> args)
        {

            string action = args.Action;
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save && args.Action == "Edit")
            {

              
                var data = this.tambahViewModel.SoItems
                               .Where(x=>x.SoItemId==args.Data.SoItemId)
                               .FirstOrDefault();
                data = args.Data;
                this.mainGrid.Refresh();
            }
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save && args.Action == "Add")
            {
                this.tambahViewModel.SoItems.Add(new SoItem
                {
                     SoItemId = (long)(args.RowIndex+1),
                     ItemName = args.Data.ItemName,
                     Quantity = args.Data.Quantity,
                     Price = args.Data.Price
                });
                this.mainGrid.Refresh();
              
               
            }
           
        }
        public async Task BtnAddItem_Click()
        {
          await  this.mainGrid.AddRecordAsync();
        }
        public async Task BtnSimpan_Click()
        {
                bool response = (id is not null ? await this.mainSvc.Ubah(tambahViewModel) : await this.mainSvc.Tambah(tambahViewModel));
            if (response)
            {
                navigationManager.NavigateTo("/");
                await js.InvokeVoidAsync("ShowNotification", "success", $"Data Berhasil Diperbaharui");
            }
            else
            {
                await js.InvokeVoidAsync("ShowNotification", "error", $"Gagal Memperbaharui Data, jika Anda merasa ada kesalahan silahkan hubungi Administrator");
            }
        }
        #endregion
      

    
        public async ValueTask DisposeAsync()
        {
            if (this.mainJS != null)
            {
                this.mainJS.Value.Dispose();
            }
        }
        protected class LoadingStatus
        {
            public bool Data { get; set; } = false;
        }
    }
}

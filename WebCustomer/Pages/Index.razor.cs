using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using WebCustomer.Models;
using WebCustomer.Services;

namespace WebCustomer.Pages
{
    public partial class Index : ComponentBase, IAsyncDisposable
    {

        #region Parameter
        [CascadingParameter] protected App app { get; set; }
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
        protected SfGrid<SoOrder> mainGrid { get; set; }
        #endregion
        #region ViewModel
        protected LoadingStatus isLoading { get; set; } = new LoadingStatus();
        #endregion
        #region List
        protected long selectedId { get; set; } = new();
        protected List<SoOrder> listViewModel { get; set; } = new List<SoOrder>();
        #endregion
        #region Properties
        bool debug = false;
        readonly Lazy<Task<IJSObjectReference>> mainJS;
        protected string Keywordsearch { get; set; }
        protected DateTime datesearch { get; set; } = DateTime.Now;
        protected int index = 1;

        private TimeZoneInfo timeZone;
        private DateTime dateTimeZone;
        #endregion

        public Index()
        {

        }
        protected override async Task OnInitializedAsync()
        {
            await GetDaftar(null, null);
            Boolean.TryParse(config["Debug"], out this.debug);
        }


        protected async Task GetDaftar(string? qry, DateTime? date)                                                     
        {
            isLoading.Data = true;
            StateHasChanged();

            listViewModel = await mainSvc.ListAll(qry,date);
           
            isLoading.Data = false;
            StateHasChanged();
        }
        private string GetRowNumber(object data, string field)
        {
            var index = listViewModel.IndexOf((SoOrder)data) + 1;
            return index.ToString();
        }
        #region Event Listener

        protected async Task BtnDelete_Click(long id)
        {
            selectedId = id;
            await js.InvokeVoidAsync("ModalShow", $"modal-delete");
        }
        protected async Task BtnDeleteClose_Click()
        {
            selectedId = new();
            await js.InvokeVoidAsync("ModalClose", $"modal-delete");
        }
        protected async Task BtnDeleteConfrim_Click()
        {
            var data = listViewModel.Where(x => x.SoOrderId == selectedId).FirstOrDefault();
            var respone = await mainSvc.Delete(data);
            if (respone)
            {
                
                

                await GetDaftar(null, null);
                await js.InvokeVoidAsync("ShowNotification", "success", $"Data Berhasil Dihapus");
            }
            else
            {
                await js.InvokeVoidAsync("ShowNotification", "error", $"Data Tidak Tersedia, jika Anda merasa ada kesalahan silahkan hubungi Administrator");

            }


            await js.InvokeVoidAsync("ModalClose", $"modal-delete");
        }
        protected async Task BtnSearch_Click()
        {
            if (Keywordsearch is not null || datesearch != null)
            {
                await GetDaftar(Keywordsearch, datesearch);
            }

        }
        protected void BtnAdd_Click()
        {

            this.mainGrid.PreventRender();
            navigationManager.NavigateTo("./sales/tambah");
        }
        protected async Task BtnExport_Click()
        {
            ExcelExportProperties ExportProperties = new ExcelExportProperties();
            ExcelTheme Theme = new ExcelTheme();
            ExcelStyle ThemeStyle = new ExcelStyle()
            {
                FontColor = "#fefefe",
                BackColor = "#000000",
            };
            Theme.Header = ThemeStyle;
            ExportProperties.Theme = Theme;
            ExportProperties.FileName = $"DataSales_{DateTime.Now.ToString("yyyyMMddHHss")}.xlsx";
            ExportProperties.IncludeTemplateColumn = false;



            await this.mainGrid.ExcelExport(ExportProperties);

        }
        #endregion

        public async ValueTask DisposeAsync()
        {
            if (this.mainJS != null && this.mainJS.IsValueCreated)
            {
                this.mainJS.Value.Dispose();
            }
        }
        protected class LoadingStatus
        {
            public bool Data { get; set; } = false;
            public bool Delete { get; set; } = false;
        }
    }
}

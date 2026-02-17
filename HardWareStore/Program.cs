using HardWareStore.BL;
using HardWareStore.DL;
using HardWareStore.Interfaces;
using HardWareStore.UI;
//using MedicineShop;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HardWareStore
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            configureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            
            var mainForm = ServiceProvider.GetRequiredService<Customersale>();
            Application.Run(mainForm);
        }
        private static void configureServices(ServiceCollection services)
        {
            services.AddScoped<IPurchaseBatchDL, PurchaseBatchDL>();
            services.AddScoped<IProductsDL, ProductsDL>();
            services.AddScoped<IVariantsDL, VariantsDL>();
            services.AddScoped<IInventoryDL, InventoryDL>();
            services.AddScoped<Icustomerbilldl, Custbilldl>();
            services.AddScoped<Icustomerbillbl, custbillbl>();



            services.AddTransient<Dashboard>();
            services.AddTransient<InventoryMain>();
            services.AddTransient<ProductsMain>();
            services.AddTransient<VariantsMain>();
            services.AddTransient<Customersale>();
            services.AddTransient<Addcustomer>();
            services.AddTransient<Quotaion>();
            services.AddTransient<customerbillui>();
            services.AddTransient<customerbillspecui>();
            services.AddTransient<Customermain>();
            services.AddTransient<AddCompany>();
            services.AddTransient<CompanyMain>();



            services.AddTransient<AddPurchaseBatchForm>();
            services.AddTransient<SupplierBillDetailsForm>();
            services.AddTransient<SupplierBillsForm>();
            services.AddTransient<Dashboard>();
        }
    }
}

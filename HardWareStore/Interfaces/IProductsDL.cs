using HardWareStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardWareStore.Interfaces
{
    public interface IProductsDL
    {
        Task<bool> AddProduct(Products product);
        Task<bool> DeleteProduct(int productId);
        Task<List<Products>> GetActiveProducts();
        Task<List<Products>> GetAllProducts();
        Task<int> GetCategoryIdByName(string categoryName);
        Task<List<string>> GetCategoryNames(string keyword = "");
        Task<Products> GetProductById(int productId);
        Task<int> GetProductIdByName(string productName);
        Task<List<string>> GetProductNames(string keyword = "");
        Task<List<Products>> GetProductsByCategory(int categoryId);
        Task<List<Products>> GetProductsBySupplier(int supplierId);
        Task<int> GetSupplierIdByName(string supplierName);
        Task<List<string>> GetSupplierNames(string keyword = "");
        Task<bool> HardDeleteProduct(int productId);
        Task<List<Products>> SearchProducts(string keyword);
        Task<bool> UpdateProduct(Products product);
    }
}
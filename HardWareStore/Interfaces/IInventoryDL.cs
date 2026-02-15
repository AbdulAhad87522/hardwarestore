using HardWareStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardWareStore.Interfaces
{
    public interface IInventoryDL
    {
        Task<List<InventoryResponse>> GetActiveInventory();
        Task<List<InventoryResponse>> GetAllInventory();
        Task<List<InventoryResponse>> GetInventoryByCategory(int categoryId);
        Task<List<InventoryResponse>> GetInventoryBySupplier(int supplierId);
        Task<List<InventoryResponse>> GetLowStockInventory();
        Task<List<InventoryResponse>> GetOutOfStockInventory();
        Task<List<InventoryResponse>> SearchInventory(string keyword);
    }
}
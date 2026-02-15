using HardWareStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardWareStore.Interfaces
{
    public interface IPurchaseBatchDL
    {
        Task<bool> AddBatch(PurchaseBatch batch);
        Task<bool> AddBatchItem(PurchaseBatchItem item);
        Task<bool> DeleteBatch(int batchId);
        Task<bool> DeleteBatchItem(int itemId, int variantId, decimal quantity);
        Task<List<PurchaseBatch>> GetAllBatches();
        Task<PurchaseBatch> GetBatchById(int batchId);
        Task<List<PurchaseBatchItem>> GetBatchItems(int batchId);
        Task<int> GetNextBatchId();
        Task<int> GetNextItemId();
        Task<List<PurchaseBatchItem>> GetVariantsForSelection();
        Task<List<PurchaseBatch>> SearchBatches(string keyword);
        Task<List<PurchaseBatchItem>> SearchVariantsForSelection(string keyword);
        Task<bool> UpdateBatch(PurchaseBatch batch);
    }
}
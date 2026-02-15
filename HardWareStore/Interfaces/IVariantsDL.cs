using HardWareStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardWareStore.Interfaces
{
    public interface IVariantsDL
    {
        Task<bool> AddVariant(Variants variant);
        Task<bool> AdjustStock(int variantId, decimal quantityChange);
        Task<bool> DeleteVariant(int variantId);
        Task<List<Variants>> GetActiveVariants();
        Task<List<Variants>> GetAllVariants();
        Task<List<Variants>> GetLowStockVariants();
        Task<List<Variants>> GetOutOfStockVariants();
        Task<List<string>> GetSizesByProductId(int productId, string keyword = "");
        List<string> GetUnitOfMeasures();
        Task<Variants> GetVariantById(int variantId);
        Task<Variants> GetVariantByProductAndSize(int productId, string size, string classType = null);
        Task<List<string>> GetVariantDisplayStrings(string keyword = "", int? productId = null);
        Task<int> GetVariantIdByDisplayString(string displayString);
        Task<List<Variants>> GetVariantsByProductId(int productId);
        Task<bool> HardDeleteVariant(int variantId);
        Task<List<Variants>> SearchVariants(string keyword);
        Task<bool> UpdateStock(int variantId, decimal newQuantity);
        Task<bool> UpdateVariant(Variants variant);
    }
}
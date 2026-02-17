using HardWareStore.Models;
using System.Collections.Generic;

namespace HardWareStore.Interfaces
{
    /// <summary>
    /// Data-layer operations for customer returns.
    /// Uses DatabaseHelper (synchronous) — consistent with the rest of the project.
    /// </summary>
    public interface IReturnsDL
    {
        /// <summary>
        /// Runs a full atomic transaction:
        ///   1. Inserts return header into `returns`
        ///   2. Inserts each line into `return_items`
        ///   3. If RestoreStock = true, increments product_variants.quantity_in_stock
        ///   4. Reduces customers.current_balance by refund amount (credit customers)
        ///   5. Sets bill payment_status to 'Refunded'
        /// Returns true on success, throws on failure.
        /// </summary>
        bool ProcessReturn(ReturnRequest request);
    }

    /// <summary>
    /// Data-layer operations for bill lookup needed by the Returns form.
    /// </summary>
    public interface IBillsDL
    {
        /// <summary>Returns bill header + customer name, or null if not found.</summary>
        BillSummary GetBillByNumber(string billNumber);

        /// <summary>Returns all line items for a bill with product name and variant size.</summary>
        List<BillItemRow> GetBillItems(int billId);
    }
}
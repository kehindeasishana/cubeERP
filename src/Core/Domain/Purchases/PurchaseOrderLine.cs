using Core.Domain.Financials;
using Core.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Domain.Purchases
{

    public partial class PurchaseOrderLine : BaseEntity
    {
        public PurchaseOrderLine()
        {
            PurchaseReceiptLines = new HashSet<PurchaseReceiptLine>();
        }

        public int PurchaseOrderHeaderId { get; set; }
        public string Item { get; set; }
        //public int MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int GLAccountId { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public virtual Account GLAccount { get; set; }
        public virtual PurchaseOrderHeader PurhcaseOrderHeader { get; set; }
        //public virtual InventoryCatalog Item { get; set; }
       // public virtual Measurement Measurement { get; set; }

        public virtual ICollection<PurchaseReceiptLine> PurchaseReceiptLines { get; set; }

        public decimal? GetReceivedQuantity()
        {
            decimal? qty = 0;
            foreach (var stock in PurchaseReceiptLines)
            {
                qty += stock.InventoryControlJournal.INQty;
            }
            return qty;
        }

        public bool IsCompleted()
        {
            bool completed = false;
            decimal totalReceiptAmount = PurchaseReceiptLines.Sum(d => d.Amount);
            if (totalReceiptAmount == Amount)
                completed = true;
            return completed;
        }
    }
}

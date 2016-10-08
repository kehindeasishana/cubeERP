using Core.Domain.Items;
//using Core.Domain.TaxSystem;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Purchases
{
    public partial class PurchaseReceiptLine : BaseEntity
    {
        public int PurchaseReceiptHeaderId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public int? TaxId { get; set; }
        public int? InventoryControlJournalId { get; set; }
        public int? PurchaseOrderLineId { get; set; }
        //public int MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public virtual PurchaseReceiptHeader PurchaseReceiptHeader { get; set; }
        //public virtual Item Item { get; set; }
        //public virtual Measurement Measurement { get; set; }
       // public virtual Tax Tax { get; set; }
        public virtual InventoryControlJournal InventoryControlJournal { get; set; }
        public virtual PurchaseOrderLine PurchaseOrderLine { get; set; }

        //[NotMapped]
        //public decimal LineTaxAmount { get { return Tax != null ? (Tax.Rate * Amount) : 0; } }
    }

}

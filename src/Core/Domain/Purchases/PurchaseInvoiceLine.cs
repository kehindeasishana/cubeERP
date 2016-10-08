using Core.Domain.Items;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Purchases
{
    public partial class PurchaseInvoiceLine : BaseEntity
    {
        public int PurchaseInvoiceHeaderId { get; set; }
        public string Item { get; set; }
        //public int MeasurementId { get; set; }
        public int? InventoryControlJournalId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? ReceivedQuantity { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Discount { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public virtual PurchaseInvoiceHeader PurchaseInvoiceHeader { get; set; }
        //public virtual Item Item { get; set; }
        //public virtual Measurement Measurement { get; set; }
        public virtual InventoryControlJournal InventoryControlJournal { get; set; }

        [NotMapped]
        public decimal LineTaxAmount { get { return ComputeLineTaxAmount(); } }

        private decimal ComputeLineTaxAmount()
        {
            decimal? lineTaxAmount = 0;
            //foreach(var tax in Item.ItemTaxGroup.ItemTaxGroupTax)
            //{
            //    lineTaxAmount += ((tax.Tax.Rate / 100)  * (Quantity * Cost));
            //}
            return lineTaxAmount.Value;
        }
    }
}

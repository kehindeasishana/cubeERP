using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Financials;

namespace Core.Domain.Purchases
{

    public partial class PurchaseOrderHeader : BaseEntity
    {
        public PurchaseOrderHeader()
        {
            PurchaseOrderLines = new HashSet<PurchaseOrderLine>();
            PurchaseReceipts = new HashSet<PurchaseReceiptHeader>();
        }
        public int? VendorId { get; set; }
        public int? PurchaseInvoiceHeaderId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime Date { get; set; }
        public string No { get; set; }
        //public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }
        public string CustSalesOrder { get; set; }
        public ShipVia ShipVia { get; set; }
        public string AccountPayableCategory { get; set; }
        public string CustInvoiceNo { get; set; }
        public string QuantityReceived { get; set; }
        public string Item { get; set; }
        //public string Description { get; set; }
        public int? GLAccountId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string Job { get; set; }
        public int? PayableAccountId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Account GlAccount { get; set; }
        public virtual Account PayableAccount { get; set; }
        public virtual PurchaseInvoiceHeader PurchaseInvoiceHeader { get; set; }
        public string Terms { get; set; }
        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public virtual ICollection<PurchaseReceiptHeader> PurchaseReceipts { get; set; }

        public bool IsCompleted()
        {
            foreach (var line in PurchaseOrderLines)
            {
                foreach (var receipt in PurchaseReceipts)
                {
                    var totalReceivedQuatity = receipt.PurchaseReceiptLines.Where(l => l.PurchaseOrderLineId == line.Id).Sum(q => q.ReceivedQuantity);

                    if (totalReceivedQuatity >= line.Quantity)
                        return true;
                }
            }

            return false;
        }

        public bool IsPaid()
        {
            bool paid = false;
            //decimal totalPaidAmount = Payments.Where(p => p.PurchaseOrderId == Id).Sum(a => a.Amount);
            //decimal totalPurchaseAmount = PurchaseOrderLines.Sum(d => d.Amount);
            //if (totalPaidAmount == totalPurchaseAmount)
            //    paid = true;
            return paid;
        }
    }
}

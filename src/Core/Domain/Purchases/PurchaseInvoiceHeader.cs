using Core.Domain.Financials;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Purchases
{

    public partial class PurchaseInvoiceHeader : BaseEntity
    {
        public PurchaseInvoiceHeader()
        {
            PurchaseInvoiceLines = new HashSet<PurchaseInvoiceLine>();
            PurchaseOrders = new HashSet<PurchaseOrderHeader>();
            VendorPayments = new HashSet<VendorPayment>();
        }
        
        public int? VendorId { get; set; }
        //public int? GeneralLedgerHeaderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string ShippingAddress { get; set; }
        public string CustSalesOrderNo { get; set; }
        public string CustInvoiceNo { get; set; }
        public int? DiscountTermId { get; set; }
        public ShipVia ShipVia { get; set; }
        public int? AccountId { get; set; }
        public string Quantity { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public virtual Account Account { get; set; }
        public virtual DiscountTerm DiscountTerm { get; set; }
        [Required]
        public string VendorInvoiceNo { get; set; }
        //public string Description { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
        public virtual ICollection<PurchaseOrderHeader> PurchaseOrders { get; set; }
        public virtual ICollection<VendorPayment> VendorPayments { get; set; }

        public decimal GetTotalTax()
        {
            decimal totalTaxAmount = 0;
            foreach (var detail in PurchaseInvoiceLines)
            {
                totalTaxAmount += detail.LineTaxAmount;
            }
            return totalTaxAmount;
        }

        //public bool IsPaid()
        //{
        //    return this.GeneralLedgerHeader.GeneralLedgerLines.Where(dr => dr.DrCr == DrOrCrSide.Dr).Sum(l => l.Amount) == VendorPayments.Sum(a => a.Amount);
        //}
    }
}

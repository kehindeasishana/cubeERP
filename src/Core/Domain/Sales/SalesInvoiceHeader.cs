using Core.Domain.Financials;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Sales
{

    public partial class SalesInvoiceHeader : BaseEntity
    {
        public SalesInvoiceHeader()
        {
            SalesInvoiceLines = new HashSet<SalesInvoiceLine>();
            SalesReceipts = new HashSet<SalesReceiptHeader>();
            CustomerAllocations = new HashSet<CustomerAllocation>();
        }

        public int CustomerId { get; set; }
        public int? GeneralLedgerHeaderId { get; set; }
        public int? SalesDeliveryHeaderId { get; set; }
        public string InvoiceNo { get; set; }
        public string ShippingAddress{ get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public ShipVia ShipVia { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string SalesRep { get; set; }
        public string Quantity { get; set; }
        public string Item { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public string Description { get; set; }
        public int? TaxId { get; set; }
        public decimal Amount { get; set; }
        public virtual SalesTax SalesTax { get; set; }
        public decimal ShippingHandlingCharge{ get; set; }
        public SalesInvoiceStatus Status { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SalesDeliveryHeader SalesDeliveryHeader { get; set; }

        public virtual ICollection<SalesInvoiceLine> SalesInvoiceLines { get; set; }
        [NotMapped]
        public virtual ICollection<SalesReceiptHeader> SalesReceipts { get; set; }
        public virtual ICollection<CustomerAllocation> CustomerAllocations { get; set; }

        public decimal ComputeTotalTax()
        {
            decimal totalTax = 0;
            return totalTax;
        }

        public decimal ComputeTotalDiscount()
        {
            decimal totalDiscount = 0;
            return totalDiscount;
        }

        public bool IsFullPaid()
        {
            decimal totalInvoiceAmount = SalesInvoiceLines.Sum(a => a.Amount);
            decimal totalPaidAmount = 0;
            decimal totalAllocation = CustomerAllocations.Sum(a => a.Amount);
            foreach (var line in SalesInvoiceLines)
            {
                totalPaidAmount += line.GetAmountPaid();
            }
            return (totalPaidAmount + totalAllocation) >= totalInvoiceAmount;
        }

        public decimal ComputeTotalAmount()
        {
            decimal totalInvoiceAmount = 0;
            foreach (var line in SalesInvoiceLines)
            {
                totalInvoiceAmount += line.Quantity * line.Amount;
            }
            return totalInvoiceAmount;
        }
    }
}

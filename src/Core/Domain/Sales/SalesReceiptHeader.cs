using Core.Domain.Financials;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Sales
{
//    [Table("SalesReceiptHeader")]
    public partial class SalesReceiptHeader : BaseEntity
    {
        public SalesReceiptHeader()
        {
            SalesReceiptLines = new HashSet<SalesReceiptLine>();
            CustomerAllocations = new HashSet<CustomerAllocation>();
        }

        public int CustomerId { get; set; }
        public int? GeneralLedgerHeaderId { get; set; }
        public int? AccountToDebitId { get; set; }
        //public int? SalesInvoiceHeaderId { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public virtual Customer Customer { get; set; }
        
        public virtual Account AccountToDebit { get; set; }
        //public virtual SalesInvoiceHeader SalesInvoiceHeader { get; set; }

        public virtual ICollection<SalesReceiptLine> SalesReceiptLines { get; set; }
        public virtual ICollection<CustomerAllocation> CustomerAllocations { get; set; }

        [NotMapped]
        public decimal AvailableAmountToAllocate { get { return GetAvailableAmountToAllocate(); } }
        private decimal GetAvailableAmountToAllocate()
        {
            return SalesReceiptLines.Sum(a => a.AmountPaid) - CustomerAllocations.Sum(a => a.Amount);
        }
    }
}

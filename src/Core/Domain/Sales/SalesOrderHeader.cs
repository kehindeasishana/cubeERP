using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Sales
{
    public partial class SalesOrderHeader : BaseEntity
    {
        public SalesOrderHeader()
        {
            SalesOrderLines = new HashSet<SalesOrderLine>();
        }

        public int? CustomerId { get; set; }
        public int? PaymentTermId { get; set; }
        public string No { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime Date { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        public string SalesOrderNo { get; set; }
        //public DateTime Date { get; set; }
        public DateTime? ShipByDate { get; set; }
        public string ShippingAddress { get; set; }
        public string SalesRep { get; set; }
        public ShipVia ShipVia { get; set; }
        public string CustPurchaseOrderNo { get; set; }
        public string QuantityShipped { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string UnitPrice { get; set; }
        public string Tax { get; set; }
        public decimal Amount { get; set; }
        public string GLAccount { get; set; }
        public virtual ICollection<SalesOrderLine> SalesOrderLines { get; set; }
    }
}

using Core.Domain.Items;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Sales
{

    public partial class SalesOrderLine : BaseEntity
    {
        public int SalesOrderHeaderId { get; set; }
        public string Item { get; set; }
       // public int MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public string ItemDescription { get; set; }
        public virtual SalesOrderHeader SalesOrderHeader { get; set; }
        //public virtual Item Item { get; set; }
        //public virtual Measurement Measurement { get; set; }
    }
}

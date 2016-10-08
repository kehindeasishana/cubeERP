using Core.Domain.Items;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Sales
{
    public partial class SalesDeliveryLine : BaseEntity
    {
        public int SalesDeliveryHeaderId { get; set; }
        public string Item { get; set; }
        //public int? MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string ItemDescription { get; set; }
        public virtual SalesDeliveryHeader SalesDeliveryHeader { get; set; }
        //public virtual Item Item { get; set; }
        //public virtual Measurement Measurement { get; set; }
        public decimal GetPriceAfterTax()
        {
            decimal priceAfterTax = 0;
            return priceAfterTax;
        }
    }
}

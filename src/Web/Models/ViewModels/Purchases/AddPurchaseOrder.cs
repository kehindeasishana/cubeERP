using Core.Domain;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Web.Models.ViewModels.Purchases
{
    public class AddPurchaseOrder
    {
        public AddPurchaseOrder()
        {
            PurchaseOrderLines = new List<AddPurchaseOrderLine>();
            Vendors = new HashSet<SelectListItem>();
            Items = new HashSet<SelectListItem>();
            UnitOfMeasurements = new HashSet<SelectListItem>();
            Date = DateTime.Now;
        }

        public DateTime Date { get; set; }
        public int VendorId { get; set; }
        public string ShippingAddress { get; set; }
        public string CustSalesOrder { get; set; }
        public string CustInvoiceNo { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Terms { get; set; }
        public ShipVia ShipVia { get; set; }
        public int? AccountPayableAcc { get; set; }
        public IList<AddPurchaseOrderLine> PurchaseOrderLines { get; set; }

        public ICollection<SelectListItem> Vendors { get; set; }
        public ICollection<SelectListItem> Items { get; set; }
        public ICollection<SelectListItem> UnitOfMeasurements { get; set; }

        #region Fields Add Line Item
        public string Item { get; set; }
        public string Description { get; set; }
        public int GLAccount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        #endregion
    }

    public partial class AddPurchaseOrderLine
    {
        public string Item { get; set; }
        public string Description { get; set; }
        public int GLAccount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Amount { get; set; }
        public decimal TotalLineAmount { get; set; }
    }
}

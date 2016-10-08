using Core.Domain;
using System;
using System.Collections.Generic;

namespace Web.Models.ViewModels.Purchases
{
    public class PurchaseOrders
    {
        public PurchaseOrders()
        {
            PurchaseOrderListLines = new HashSet<PurchaseOrderListLine>();
            Vendors = new HashSet<Vendor>();
        }

        public ICollection<PurchaseOrderListLine> PurchaseOrderListLines { get; set; }
        public ICollection<Vendor> Vendors { get; set; }
    }

    public class PurchaseOrderListLine
    {
        public int Id { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
        public int GLAccount { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public bool Paid { get; set; }
        public bool HasInvoiced { get; set; }
    }

    //public class Vendors
    //{
    //    public string ContactName { get; set; }
    //    public string AccountNo { get; set; }
    //    public string VendorType { get; set; }
    //    public string TaxType { get; set; }
    //    public string ExpenseAccount { get; set; }
    //    public string PhoneNo { get; set; }
    //    public string Mobile { get; set; }
    //    public string Email { get; set; }
    //    public string Website { get; set; }
    //    public string MailingAddress { get; set; }
    //    public string Country { get; set; }
    //    public string State { get; set; }
    //    public string City { get; set; }
    //    public string Zip { get; set; }
    //    public string PaymentAddress { get; set; }
    //    public string PurchaseOrderAdd { get; set; }
    //    public string ShipmentAddress { get; set; }
    //    public string PurchaseRep { get; set; }
    //    public string TaxIdNum { get; set; }
    //    public ShipVia ShipVia { get; set; }
       
    //    public string VendorUserName { get; set; }
    //    public string VendorName { get; set; }
    //    public bool IsActive { get; set; }
    //}
}

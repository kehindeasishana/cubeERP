using Core.Domain.Financials;
using Core.Domain.Items;
//using Core.Domain.TaxSystem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Domain.Purchases
{

    public partial class Vendor : BaseEntity
    {
        public Vendor()
        {
            PurchaseOrders = new HashSet<PurchaseOrderHeader>();
            PurchaseReceipts = new HashSet<PurchaseReceiptHeader>();
            PurchaseInvoices = new HashSet<PurchaseInvoiceHeader>();
            VendorPayments = new HashSet<VendorPayment>();
        }

        public string No { get; set; }
        public string ContactName { get; set; }
        public string AccountNo { get; set; }
        public string VendorType { get; set; }
        public string TaxType { get; set; }
        public string ExpenseAccount { get; set; }
        public string PhoneNo { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string MailingAddress { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string PaymentAddress { get; set; }
        public string PurchaseOrderAdd { get; set; }
        public string ShipmentAddress { get; set; }
        public string PurchaseRep { get; set; }
        public string TaxIdNum { get; set; }
        public ShipVia ShipVia { get; set; }
        public string BatchDeliveryMethod { get; set; }
        public string VendorUserName { get; set; }
        public string VendorName { get; set; }
        public bool  IsActive { get; set; }
        //public int? PartyId { get; set; }
        public int? AccountsPayableAccountId { get; set; }
        public int? PurchaseAccountId { get; set; }
        public int? PurchaseDiscountAccountId { get; set; }        
        //public int? PrimaryContactId { get; set; }
        public int? PaymentTermId { get; set; }
        public int? TaxGroupId { get; set; }

        //public virtual Party Party { get; set; }
        public virtual Account AccountsPayableAccount { get; set; }
        public virtual Account PurchaseAccount { get; set; }
        public virtual Account PurchaseDiscountAccount { get; set; }
        //public virtual Contact PrimaryContact { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        //public virtual TaxGroup TaxGroup { get; set; }

        public virtual ICollection<PurchaseOrderHeader> PurchaseOrders { get; set; }
        public virtual ICollection<PurchaseReceiptHeader> PurchaseReceipts { get; set; }
        public virtual ICollection<PurchaseInvoiceHeader> PurchaseInvoices { get; set; }
        public virtual ICollection<VendorPayment> VendorPayments { get; set; }
        public virtual ICollection<TaxAgency> TaxAgencies { get; set; }
        public virtual ICollection<InventoryCatalog> InventoryCatalogs { get; set; }
        public decimal GetBalance()
        {
            decimal balance = 0;
            decimal totalInvoiceAmount = 0;
            decimal totalInvoicePayment = 0;

            foreach (var invoice in PurchaseInvoices)
                totalInvoiceAmount += invoice.PurchaseInvoiceLines.Sum(a => a.Amount);

            foreach (var payment in VendorPayments)
                totalInvoicePayment += payment.Amount;

            balance = totalInvoiceAmount - totalInvoicePayment;
            return balance;
        }
    }
}

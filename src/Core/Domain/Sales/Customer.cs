using Core.Domain.Financials;
//using Core.Domain.TaxSystem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace Core.Domain.Sales
{

    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            SalesInvoices = new HashSet<SalesInvoiceHeader>();
            SalesReceipts = new HashSet<SalesReceiptHeader>();
            SalesOrders = new HashSet<SalesOrderHeader>();
            CustomerAllocations = new HashSet<CustomerAllocation>();
        }
        public string Username { get; set; }
        public string CustNo { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public bool IsActive { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string SalesTax { get; set; }
        public string SalesRep { get; set; }
        public string GlSalesAccount { get; set; }
        public string ResaleNo { get; set; }
        public string PriceLevel { get; set; }
        public string BatchDeliveryMethod { get; set; }
        public string Shipping { get; set; }
        public string OpenPONumber { get; set; }
        public string CardHoldersName { get; set; }
        public string Address { get; set; }
       
        public string Zip { get; set; }
        public string CreditCardNo { get; set; }
        public string ExpirationDate { get; set; }
        //public int? PartyId { get; set; }
        //public int? PrimaryContactId { get; set; }
        //public int? TaxGroupId { get; set; }
        public int? AccountsReceivableAccountId { get; set; }
        public int? SalesAccountId { get; set; }
        public int? SalesDiscountAccountId { get; set; }
        public int? PromptPaymentDiscountAccountId { get; set; }
        public int? PaymentTermId { get; set; }
        public int? CustomerAdvancesAccountId { get; set; }
        //public int? ContactId { get; set; }
        //public virtual Party Party { get; set; }
        //public virtual TaxGroup TaxGroup { get; set; }
        public virtual Account AccountsReceivableAccount { get; set; }
        public virtual Account SalesAccount { get; set; }
        public virtual Account SalesDiscountAccount { get; set; }
        public virtual Account PromptPaymentDiscountAccount { get; set; }
        //public virtual Contact PrimaryContact { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        public virtual Account CustomerAdvancesAccount { get; set; }
        public virtual ICollection<SalesInvoiceHeader> SalesInvoices { get; set; }
        public virtual ICollection<SalesReceiptHeader> SalesReceipts { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrders { get; set; }
        public virtual ICollection<CustomerAllocation> CustomerAllocations { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        [NotMapped]
        public decimal Balance { get { return GetBalance(); } }

        private decimal GetBalance()
        {
            decimal balance = 0;
            decimal totalInvoiceAmount = 0;
            decimal totalReceiptAmount = 0;
            decimal totalAllocation = 0;

            foreach (var header in SalesInvoices)
            {
                totalInvoiceAmount += header.ComputeTotalAmount();
                totalAllocation += header.CustomerAllocations.Sum(a => a.Amount);

                foreach (var receipt in header.SalesReceipts)
                    foreach(var receiptLine in receipt.SalesReceiptLines)
                        totalReceiptAmount += receiptLine.AmountPaid;
            }

            balance = (totalInvoiceAmount - totalReceiptAmount) - totalAllocation;

            return balance;
        }
    }
}

using Core.Data;
using Core.Domain;
using Core.Domain.Financials;
using Core.Domain.Items;
using Core.Domain.Sales;
using Services.Financial;
using Services.Inventory;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Services.Sales
{
    public partial class SalesService : BaseService, ISalesService
    {
        private readonly IFinancialService _financialService;
        private readonly IInventoryService _inventoryService;
        private readonly IRepository<SalesTax> _salesTaxRepo;
        private readonly IRepository<SalesOrderHeader> _salesOrderRepo;
        private readonly IRepository<SalesInvoiceHeader> _salesInvoiceRepo;
        private readonly IRepository<SalesReceiptHeader> _salesReceiptRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<CustomerAllocation> _custAllocationRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<InventoryCatalog> _inventoryRepo;
        private readonly IRepository<Measurement> _measurementRepo;
        private readonly IRepository<SequenceNumber> _sequenceNumberRepo;
        private readonly IRepository<PaymentTerm> _paymentTermRepo;
        private readonly IRepository<SalesDeliveryHeader> _salesDeliveryRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<TaxAgency> _taxAgencyRepo;
        private readonly IRepository<Contact> _contactRepo;
        //private readonly IRepository<TaxGroup> _taxGroupRepo;

        public SalesService(IFinancialService financialService,
            IInventoryService inventoryService,
            IRepository<SalesOrderHeader> salesOrderRepo,
            IRepository<SalesInvoiceHeader> salesInvoiceRepo,
            IRepository<SalesReceiptHeader> salesReceiptRepo,
            IRepository<Customer> customerRepo,
            IRepository <CustomerAllocation> custAllocationRepo,
            IRepository<Account> accountRepo,
            IRepository<Item> itemRepo,
            IRepository<InventoryCatalog> inventoryRepo,
            IRepository<Measurement> measurementRepo,
            IRepository<SequenceNumber> sequenceNumberRepo,
            IRepository<PaymentTerm> paymentTermRepo,
            IRepository<SalesDeliveryHeader> salesDeliveryRepo,
            IRepository<Bank> bankRepo,
            IRepository<SalesTax> salesTaxRepo,
            IRepository <TaxAgency> taxAgencyRepo,
            IRepository<Contact> contactRepo
            )
            : base(sequenceNumberRepo,  paymentTermRepo, bankRepo)
        {
            _financialService = financialService;
            _inventoryService = inventoryService;
            _salesOrderRepo = salesOrderRepo;
            _salesInvoiceRepo = salesInvoiceRepo;
            _salesReceiptRepo = salesReceiptRepo;
            _customerRepo = customerRepo;
            _custAllocationRepo = custAllocationRepo;
            _accountRepo = accountRepo;
            _itemRepo = itemRepo;
            _inventoryRepo = inventoryRepo;
            _measurementRepo = measurementRepo;
            _sequenceNumberRepo = sequenceNumberRepo;
            _paymentTermRepo = paymentTermRepo;
            _salesDeliveryRepo = salesDeliveryRepo;
            _bankRepo = bankRepo;
            _salesTaxRepo = salesTaxRepo;
            _taxAgencyRepo = taxAgencyRepo;
            //_genetalLedgerSetting = generalLedgerSetting;
            _contactRepo = contactRepo;
            //_taxGroupRepo = taxGroupRepo;
        }

        public void AddSalesOrder(SalesOrderHeader salesOrder, bool toSave)
        {
            if (string.IsNullOrEmpty(salesOrder.No))
                salesOrder.No = GetNextNumber(SequenceNumberTypes.SalesOrder).ToString();
            if(toSave)
                _salesOrderRepo.Insert(salesOrder);
        }

        public void UpdateSalesOrder(SalesOrderHeader salesOrder)
        {
            var persisted = _salesOrderRepo.GetById(salesOrder.Id);
            foreach (var persistedLine in persisted.SalesOrderLines)
            {
                bool found = false;
                foreach (var currentLine in salesOrder.SalesOrderLines)
                {
                    if (persistedLine.Id == currentLine.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                    continue;
                else
                {
                   
                }
            }
            _salesOrderRepo.Update(salesOrder);
        }

        public void AddSalesInvoice(SalesInvoiceHeader salesInvoice, int? salesDeliveryId)
        {   
            decimal totalAmount = 0, totalDiscount = 0;

            var taxes = new List<KeyValuePair<int, decimal>>();
            //var sales = new List<KeyValuePair<int, decimal>>();
            var sales = new List<KeyValuePair<int, decimal>>();
            //var glHeader = _financialService.CreateGeneralLedgerHeader(DocumentTypes.SalesInvoice, salesInvoice.Date, string.Empty);
            var customer = _customerRepo.GetById(salesInvoice.CustomerId);

            foreach (var lineItem in salesInvoice.SalesInvoiceLines)
            {
                //var item = _itemRepo.GetById(lineItem.ItemId);
                //var item = _inventoryRepo.GetById(lineItem.ItemId);
                var lineAmount = lineItem.Quantity * lineItem.Amount;

                //if (!item.GLAccountsValidated())
                //    throw new Exception("Item is not correctly setup for financial transaction. Please check if GL accounts are all set.");

                var lineDiscountAmount = (lineItem.Discount / 100) * lineAmount;
                totalDiscount += lineDiscountAmount;

                var totalLineAmount = lineAmount - lineDiscountAmount;
                
                totalAmount += totalLineAmount;
                
                var lineTaxes = _financialService.ComputeOutputTax(salesInvoice.CustomerId, lineItem.Item, lineItem.Quantity, lineItem.Amount, lineItem.Discount);

                foreach (var t in lineTaxes)
                    taxes.Add(t);

                var lineTaxAmount = lineTaxes != null && lineTaxes.Count > 0 ? lineTaxes.Sum(t => t.Value) : 0;
                totalLineAmount = totalLineAmount - lineTaxAmount;
                
               // sales.Add(new KeyValuePair<int, decimal>( totalLineAmount));

                //if (item.ItemType == ItemTypes.Purchased)
                //{
                //    lineItem.InventoryControlJournal = _inventoryService.CreateInventoryControlJournal(lineItem.ItemId,
                //        lineItem.MeasurementId,
                //        DocumentTypes.SalesInvoice,
                //        null,
                //        lineItem.Quantity,
                //        lineItem.Quantity * item.UnitCost,
                //        lineItem.Quantity * item.SalePrice);
                //}
            }
            
            totalAmount += salesInvoice.ShippingHandlingCharge;
           

            var groupedSalesAccount = from s in sales
                                      group s by s.Key into grouped
                                      select new
                                      {
                                          Key = grouped.Key,
                                          Value = grouped.Sum(s => s.Value)
                                      };

            foreach (var salesAccount in groupedSalesAccount)
            {
                var salesAmount = salesAccount.Value;
                
            }

            if (taxes != null && taxes.Count > 0)
            {
                var groupedTaxes = from t in taxes
                                   group t by t.Key into grouped
                                   select new
                                   {
                                       Key = grouped.Key,
                                       Value = grouped.Sum(t => t.Value)
                                   };

                //foreach (var tax in groupedTaxes)
                //{
                //    var tx = _financialService.GetTaxes().Where(t => t.Id == tax.Key).FirstOrDefault();
                    
                //}
            }

        }

    
        public IEnumerable<SalesInvoiceHeader> GetSalesInvoices()
        {
            var query = from invoice in _salesInvoiceRepo.Table
                        select invoice;
            return query.ToList();
        }

        public SalesInvoiceHeader GetSalesInvoiceById(int id)
        {
            return _salesInvoiceRepo.GetById(id);
        }

        public SalesInvoiceHeader GetSalesInvoiceByNo(string no)
        {
            var query = from invoice in _salesInvoiceRepo.Table
                        where invoice.InvoiceNo == no
                        select invoice;
            return query.FirstOrDefault();
        }

        public void UpdateSalesInvoice(SalesInvoiceHeader salesInvoice)
        {
            _salesInvoiceRepo.Update(salesInvoice);
        }
        public void AddSalesReceipt(SalesReceiptHeader salesReceipt)
        {
            _salesReceiptRepo.Insert(salesReceipt);
        }
        public IEnumerable<SalesReceiptHeader> GetSalesReceipts()
        {
            var query = from receipt in _salesReceiptRepo.Table
                        select receipt;
            return query.ToList();
        }

        public IEnumerable<SalesTax> ListSalesTax()
        {
            var query = from salesTax in _salesTaxRepo.Table
                        select salesTax;
            return query.ToList();
        }
        public SalesTax GetSalesTax(int id)
        {
            return _salesTaxRepo.GetById(id);

        }
        public void DeleteSalesTax(SalesTax salesTax)
        {
            _salesTaxRepo.Delete(salesTax);
        }
        public void AddSalesTax(SalesTax salesTax)
        {
            _salesTaxRepo.Insert(salesTax);
        }
        public void UpdateSalesTax(SalesTax salesTax)
        {
            _salesTaxRepo.Update(salesTax);
        }

        public IEnumerable<TaxAgency> TaxAgency()
        {
            var query = from Tax in _taxAgencyRepo.Table
                        select Tax;
            return query.ToList();
        }
        public TaxAgency GetTaxAgency(int id)
        {
            return _taxAgencyRepo.GetById(id);

        }
        public void DeleteTaxAgency(TaxAgency taxAgency)
        {
            _taxAgencyRepo.Delete(taxAgency);
        }
        public void AddTaxAgency(TaxAgency taxAgency)
        {
            _taxAgencyRepo.Insert(taxAgency);
        }
        public void UpdateTaxAgency(TaxAgency taxAgency)
        {
            _taxAgencyRepo.Update(taxAgency);
        }
        public SalesReceiptHeader GetSalesReceiptById(int id)
        {
            return _salesReceiptRepo.GetById(id);
        }
        public void DeleteContact(Contact contact)
        {
            _contactRepo.Delete(contact);
        }
        public void AddContact(Contact contact)
        {
            _contactRepo.Insert(contact);
        }
        public void UpdateContact(Contact contact)
        {
            _contactRepo.Update(contact);
        }
        public void UpdateSalesReceipt(SalesReceiptHeader salesReceipt)
        {
            _salesReceiptRepo.Update(salesReceipt);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            System.Linq.Expressions.Expression<Func<Customer, object>>[] includeProperties =
                //{ p => p.Party, c => c.AccountsReceivableAccount };
                { c => c.AccountsReceivableAccount };
            var customers = _customerRepo.GetAllIncluding(includeProperties);

            return customers.AsEnumerable();
        }

        public Customer GetCustomerById(int id)
        {
            System.Linq.Expressions.Expression<Func<Customer, object>>[] includeProperties =
                { c => c.AccountsReceivableAccount };

            var customer = _customerRepo.GetAllIncluding(includeProperties)
                .Where(c => c.Id == id).FirstOrDefault();

            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            _customerRepo.Update(customer);
        }
        public void DeleteCustomer(Customer customer)
        {
            _customerRepo.Delete(customer);
        }
        public ICollection<SalesReceiptHeader> GetCustomerReceiptsForAllocation(int customerId)
        {
            var customerReceipts = _salesReceiptRepo.Table.Where(r => r.CustomerId == customerId);
            var customerReceiptsWithNoInvoice = new HashSet<SalesReceiptHeader>();
            foreach (var receipt in customerReceipts)
            {
                //if (receipt.SalesInvoiceHeaderId == null)
                //    customerReceiptsWithNoInvoice.Add(receipt);
                customerReceiptsWithNoInvoice.Add(receipt);
            }
            return customerReceiptsWithNoInvoice;
        }

        public void AddCustomer(Customer customer)
        {
            //var accountAR = _accountRepo.Table.Where(e => e.AccountCode == "10120").FirstOrDefault();
            //var accountSales = _accountRepo.Table.Where(e => e.AccountCode == "40100").FirstOrDefault();
            //var accountAdvances = _accountRepo.Table.Where(e => e.AccountCode == "20120").FirstOrDefault();
            //var accountSalesDiscount = _accountRepo.Table.Where(e => e.AccountCode == "40400").FirstOrDefault();

            //customer.AccountsReceivableAccountId = accountAR != null ? (int?)accountAR.Id : null;
            //customer.SalesAccountId = accountSales != null ? (int?)accountSales.Id : null;
            //customer.CustomerAdvancesAccountId = accountAdvances != null ? (int?)accountAdvances.Id : null;
            //customer.SalesDiscountAccountId = accountSalesDiscount != null ? (int?)accountSalesDiscount.Id : null;
            //customer.TaxGroupId = _taxGroupRepo.Table.Where(tg => tg.Description == "VAT").FirstOrDefault().Id;

            customer.IsActive = true;

            _customerRepo.Insert(customer);
        }

        public IEnumerable<SalesDeliveryHeader> GetSalesDeliveries()
        {
            var query = from f in _salesDeliveryRepo.Table
                        select f;
            return query;
        }

        
        public IEnumerable<SalesOrderHeader> GetSalesOrders()
        {
            var salesOrders = _salesOrderRepo.GetAllIncluding(c => c.Customer,
                pt => pt.PaymentTerm,
                lines => lines.SalesOrderLines);

            foreach(var salesOrder in salesOrders)
            {
                //salesOrder.Customer.Party = GetCustomerById(salesOrder.CustomerId.Value).Party;
                salesOrder.Customer.Username = GetCustomerById(salesOrder.CustomerId.Value).Username;
            }

            return salesOrders;
        }

        public SalesOrderHeader GetSalesOrderById(int id)
        {
            var salesOrder = _salesOrderRepo.GetAllIncluding(lines => lines.SalesOrderLines,
                c => c.Customer,
                p => p.PaymentTerm)
                .Where(o => o.Id == id).FirstOrDefault()
                ;

            //foreach(var line in salesOrder.SalesOrderLines)
            //{
            //    line.Item = _itemRepo.GetById(line.ItemId);
            //    line.Measurement = _measurementRepo.GetById(line.MeasurementId);
            //}

            return salesOrder;
        }
        public void DeleteSalesOrder(SalesOrderHeader salesOrder)
        {
            _salesOrderRepo.Delete(salesOrder);
        }
        public SalesDeliveryHeader GetSalesDeliveryById(int id)
        {
            return _salesDeliveryRepo.GetById(id);
        }

        public IEnumerable<Contact> GetContacts()
        {
            var query = from f in _contactRepo.Table
                        select f;
            return query;
        }

        public int SaveContact(Contact contact)
        {
            _contactRepo.Insert(contact);
            return contact.Id;
        }
        public Contact GetContactById(int id)
        {
           return _contactRepo.GetById(id);
        }
        public ICollection<SalesInvoiceHeader> GetSalesInvoicesByCustomerId(int customerId, SalesInvoiceStatus status)
        {
            var query = from invoice in _salesInvoiceRepo.Table
                        where invoice.CustomerId == customerId && invoice.Status == status
                        select invoice;
            return query.ToList();
        }
        public void SaveCustomerAllocation(CustomerAllocation allocation)
        {
            _custAllocationRepo.Insert(allocation);
            
           
        }
        public ICollection<CustomerAllocation> GetCustomerAllocations(int customerId)
        {
            var query = from invoice in _custAllocationRepo.Table
                        where invoice.CustomerId == customerId 
                        select invoice;
            return query.ToList();
            //return null;
        }
    }
}

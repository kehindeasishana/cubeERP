using Services.Financial;
using Services.Inventory;
using Services.Sales;
using System.Web.Mvc;
using System.Linq;
using Core.Domain.Sales;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Core.Domain;

namespace Web.Controllers
{
    [Authorize(Roles ="SuperAdmin, Account Manager, Employee")]
    public class SalesController : BaseController
    {
        private Web.Models.ViewModels.Sales.SalesViewModelBuilder _viewModelBuilder;

        private readonly IInventoryService _inventoryService;
        private readonly IFinancialService _financialService;
        private readonly ISalesService _salesService;

        public SalesController(IInventoryService inventoryService,
            IFinancialService financialService,
            ISalesService salesService)
        {
            _inventoryService = inventoryService;
            _financialService = financialService;
            _salesService = salesService;

            _viewModelBuilder = new Web.Models.ViewModels.Sales.SalesViewModelBuilder(inventoryService, financialService, salesService);
        }
        [Audit]
        public ActionResult Index()
        {
            return View();
        }
        [Audit]
        public ActionResult SalesOrders()
        {
            var salesOrders = _salesService.GetSalesOrders();
            var model = _viewModelBuilder.CreateSalesOrdersViewModel(salesOrders.ToList());
            return View(model);
        }
        [Audit]
        public ActionResult SalesDeliveries()
        {
            var salesDeliveries = _salesService.GetSalesDeliveries();
            var model = _viewModelBuilder.CreateSalesDeliveriesViewModel(salesDeliveries.ToList());
            return View(model);
        }

        public ActionResult AddSalesTax()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddSalesTax(SalesTax model)
        {
            _salesService.AddSalesTax(new SalesTax()
            {
                NoofAgency = model.NoofAgency,
                SalesTaxId = model.SalesTaxId,
                SalesTaxName = model.SalesTaxName,
                TotalTaxRate = model.TotalTaxRate,
                TaxOnFreight = model.TaxOnFreight
            });
            return RedirectToAction("ListSalesTax");
        }

        [Audit]
        public ActionResult ListSalesTax(SalesTax model)
        {
            var salesTax = _salesService.ListSalesTax();
            return View(salesTax);

        }

        public ActionResult EditSalesTax(int id)
        {
            return View(this._salesService.GetSalesTax(id));

        }
        [Audit]
        [HttpPost]
        public ActionResult EditSalesTax(SalesTax model)
        {
            var salesTax = _salesService.GetSalesTax(model.Id);
            salesTax.NoofAgency = model.NoofAgency;
            salesTax.SalesTaxId = model.SalesTaxId;
            salesTax.SalesTaxName = model.SalesTaxName;
            salesTax.TotalTaxRate = model.TotalTaxRate;
            salesTax.TaxOnFreight = model.TaxOnFreight;
            _salesService.UpdateSalesTax(salesTax);
            return RedirectToAction("ListSalesTax");
        }

        [Audit]
        public ViewResult GetSalesTax(int id)
        {
            SalesTax salesTax = _salesService.GetSalesTax(id);
            return View(salesTax);

        }
        [Audit]
        public ActionResult DeleteSalesTax(int id)
        {
            SalesTax salesTax = _salesService.GetSalesTax(id);
            return View(salesTax);
        }

        [HttpPost]
        public ActionResult DeleteSalesTax(int id, SalesTax salesTax)
        {
            var Tax = _salesService.GetSalesTax(id);
            _salesService.DeleteSalesTax(Tax);
            return RedirectToAction("ListSalesTax");
        }


        public ActionResult AddTaxAgency()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddTaxAgency(TaxAgency model)
        {
            _salesService.AddTaxAgency(new TaxAgency()
            {
                AccountId = model.AccountId,
                CalculateTax = model.CalculateTax,
                TaxAgencyId = model.TaxAgencyId,
                TaxAgencyName = model.TaxAgencyName,
                TaxRate = model.TaxRate,
                VendorId = model.VendorId
            });
            return RedirectToAction("ListTaxAgency");
        }

        [Audit]
        public ActionResult ListTaxAgency(TaxAgency model)
        {
            var taxAgency = _salesService.TaxAgency();
            return View(taxAgency);
        }

        public ActionResult EditTaxAgency(int id)
        {
            return View(this._salesService.GetTaxAgency(id));

        }
        [Audit]
        public ActionResult EditTaxAgency(TaxAgency model)
        {
            var taxAgency = _salesService.GetTaxAgency(model.Id);
            taxAgency.TaxAgencyId = model.TaxAgencyId;
            taxAgency.TaxAgencyName = model.TaxAgencyName;
            taxAgency.TaxRate = model.TaxRate;
            taxAgency.VendorId = model.VendorId;
            taxAgency.AccountId = model.AccountId;
            taxAgency.CalculateTax = model.CalculateTax;
            _salesService.UpdateTaxAgency(taxAgency);
            return RedirectToAction("ListTaxAgency");
        }

        [Audit]
        public ViewResult GetTaxAgency(int id)
        {
            TaxAgency taxAgency = _salesService.GetTaxAgency(id);
            return View(taxAgency);

        }
        [Audit]
        public ActionResult DeleteTaxAgency(int id)
        {
            TaxAgency taxAgency = _salesService.GetTaxAgency(id);
            return View(taxAgency);
        }

        [HttpPost]
        public ActionResult DeleteTaxAgency(int id, TaxAgency taxAgency)
        {
            var agency = _salesService.GetTaxAgency(id);
            _salesService.DeleteTaxAgency(agency);
            return RedirectToAction("ListTaxAgency");
        }
        public ActionResult AddContact()
        {
            return View();
        }
        [Audit]
        [HttpPost, ActionName("AddContact")]
        public ActionResult AddContact(Contact model)
        {
            _salesService.AddContact(new Contact()
            {
                Address = model.Address,
                CompanyName = model.CompanyName,
                Customer = model.Customer,
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                Mobile = model.Mobile,
                Gender = model.Gender,
                JobTitle = model.JobTitle,
                CustomerId = model.CustomerId,
                LastName = model.LastName,
                PhoneNo = model.PhoneNo,
                Notes = model.Notes
                
            });

            return RedirectToAction("Contacts");
        }

        [Audit]
        public ActionResult Contacts(Contact model)
        {
            var contacts = _salesService.GetContacts();
            
            return View(contacts);

        }

        public ActionResult EditContact(int id)
        {
            return View(this._salesService.GetContactById(id));
           
        }
        [Audit]
        [HttpPost, ActionName("EditContact")]

        public ActionResult EditContact(Contact model)
        {
            var contact = _salesService.GetContactById(model.Id);
            contact.FirstName = model.FirstName;
            contact.Email = model.Email;
            contact.CustomerId = model.CustomerId;
            contact.CompanyName = model.CompanyName;
            contact.Address = model.Address;
            contact.Gender = model.Gender;
            contact.MiddleName = model.MiddleName;
            contact.JobTitle = model.JobTitle;
            contact.Mobile = model.Mobile;
            contact.PhoneNo = model.PhoneNo;
            contact.Notes = model.Notes;
            _salesService.UpdateContact(contact);
           
            return RedirectToAction("Contacts");
        }

        [Audit]
        [Authorize]
        public ViewResult GetContactById(int id)
        {
            Contact contact = _salesService.GetContactById(id);
            return View(contact);

        }
        [Audit]
        [Authorize]
        public ActionResult DeleteContact(int id)
        {
            Contact contact = _salesService.GetContactById(id);
            return View(contact);

        }

        [HttpPost, ActionName("DeleteContact")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteContact(int id, Contact contact)
        {
            var contacts = _salesService.GetContactById(id);
            _salesService.DeleteContact(contacts);
            return RedirectToAction("Contacts");
        }
        public ActionResult AddSalesDelivery(bool direct = false)
        {
            if (direct)
            {
                var model = _viewModelBuilder.CreateSalesDeliveryViewModel();
                return View(model);
            }
            return RedirectToAction("SalesDeliveries");
        }
        [Audit]
        [HttpPost, ActionName("AddSalesDelivery")]
        [FormValueRequiredAttribute("SaveSalesDelivery")]
        public ActionResult SaveSalesDelivery(Models.ViewModels.Sales.SalesDeliveryViewModel model)
        {
            var salesDelivery = new SalesDeliveryHeader()
            {
                CustomerId = model.CustomerId,
                PaymentTermId = model.PaymentTermId,
                Date = model.Date,
            };
            foreach(var line in model.SalesDeliveryLines)
            {
                salesDelivery.SalesDeliveryLines.Add(new SalesDeliveryLine()
                {
                    Item = line.Item,
                    //MeasurementId = line.MeasurementId,
                    Quantity = line.Quantity,
                    Discount = line.Discount,
                    Price = line.Quantity * line.Price,
                });
            }
            //_salesService.AddSalesDelivery(salesDelivery, true);
            return RedirectToAction("SalesDeliveries");
        }
        [Audit]
        [HttpPost, ActionName("AddSalesDelivery")]
        [FormValueRequiredAttribute("AddSalesDeliveryLineItem")]
        public ActionResult AddSalesDeliveryLineItem(Models.ViewModels.Sales.SalesDeliveryViewModel model)
        {
            //var item = _inventoryService.GetItemById(model.ItemId.Value);
            var line = new Models.ViewModels.Sales.SalesDeliveryLineViewModel()
            {
                Item = model.Item,
                //MeasurementId = model.MeasurementId,
                Quantity = model.Quantity,
                Price = model.Price * model.Quantity,
                Discount  = model.Discount,
                //LineTotalTaxAmount = item.ItemTaxAmountOutput * model.Quantity
            };

            model.SalesDeliveryLines.Add(line);
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddSalesDelivery")]
        [FormValueRequiredAttribute("DeleteSaleDeliveryLineItem")]
        public ActionResult DeleteSaleDeliveryLineItem(Models.ViewModels.Sales.SalesDeliveryViewModel model)
        {
            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            model.SalesDeliveryLines.Remove(model.SalesDeliveryLines.Where(i => i.Item == deletedItem.ToString()).FirstOrDefault());
            return View(model);
        }
        [Audit]
        public ActionResult SalesInvoices()
        {
            var invoices = _salesService.GetSalesInvoices();
            var model = new Web.Models.ViewModels.Sales.SalesInvoices();
            foreach(var invoice in invoices)
            {
                model.SalesInvoiceListLines.Add(new Models.ViewModels.Sales.SalesInvoiceListLine()
                {
                    Id = invoice.Id,
                    No = invoice.InvoiceNo,
                    Customer = invoice.Customer.CustNo,
                    CustomerId = invoice.CustomerId,
                    Date = invoice.InvoiceDate,
                    Amount = invoice.ComputeTotalAmount(),
                    IsFullPaid = invoice.IsFullPaid()
                });
            }
            return View(model);
        }

        public ActionResult AddSalesInvoice(bool direct = false)
        {
            var model = new Web.Models.ViewModels.Sales.AddSalesInvoice();
            model.Customers = Models.ModelViewHelper.Customers();
            model.Items = Models.ModelViewHelper.Items();
            model.Measurements = Models.ModelViewHelper.Measurements();

            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddSalesInvoice")]
        [FormValueRequiredAttribute("SaveSalesInvoice")]
        public ActionResult SaveSalesInvoice(Models.ViewModels.Sales.AddSalesInvoice model)
        {
            if (model.AddSalesInvoiceLines.Sum(i => i.Amount) == 0 || model.AddSalesInvoiceLines.Count < 1)
            {
                model.Customers = Models.ModelViewHelper.Customers();
                model.Items = Models.ModelViewHelper.Items();
                model.Measurements = Models.ModelViewHelper.Measurements();
                ModelState.AddModelError("Amount", "No invoice line");
                return View(model);
            }
            var invoiceHeader = new SalesInvoiceHeader();
            var invoiceLines = new List<SalesInvoiceLine>();
            foreach (var item in model.AddSalesInvoiceLines)
            {
                //var Item = _inventoryService.GetItemById(item.ItemId);
                var invoiceDetail = new SalesInvoiceLine();
                //invoiceDetail.TaxId = Item.ItemTaxGroupId;
                invoiceDetail.Item = item.Item;
                //invoiceDetail.MeasurementId = item.MeasurementId;
                invoiceDetail.Quantity = item.Quantity;
                invoiceDetail.Discount = item.Discount;
                invoiceDetail.Amount = Convert.ToDecimal(item.Quantity * model.Price);
                invoiceLines.Add(invoiceDetail);
            }
            invoiceHeader.SalesInvoiceLines = invoiceLines;
            invoiceHeader.CustomerId = model.CustomerId;
            invoiceHeader.InvoiceDate = model.Date;
            invoiceHeader.ShippingHandlingCharge = 4;// model.ShippingHandlingCharge;

            _salesService.AddSalesInvoice(invoiceHeader, model.SalesOrderId);
            return RedirectToAction("SalesInvoices");
        }
        [Audit]
        [HttpPost, ActionName("AddSalesInvoice")]
        [FormValueRequiredAttribute("AddSalesInvoiceLine")]
        public ActionResult AddSalesInvoiceLine(Models.ViewModels.Sales.AddSalesInvoice model)
        {
            model.Customers = Models.ModelViewHelper.Customers();
            model.Items = Models.ModelViewHelper.Items();
            model.Measurements = Models.ModelViewHelper.Measurements();
            if (model.Quantity > 0)
            {
                //var item = _inventoryService.GetItemById(model.ItemId);
                //if (!item.Price.HasValue)
                //{
                //    ModelState.AddModelError("Amount", "Selling price is not set.");
                //    return View(model);
                //}
                Models.ViewModels.Sales.AddSalesInvoiceLine itemModel = new Models.ViewModels.Sales.AddSalesInvoiceLine()
                {
                    Item = model.Item,
                    MeasurementId = model.MeasurementId,
                    Quantity = model.Quantity,
                    Discount = model.Discount,
                    Amount = model.Price * model.Quantity,
                    Price = model.Price,
                };
                if (model.AddSalesInvoiceLines.FirstOrDefault(i => i.Item == model.Item) == null)
                    model.AddSalesInvoiceLines.Add(itemModel);
            }
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddSalesInvoice")]
        [FormValueRequiredAttribute("DeleteInvoiceLineItem")]
        public ActionResult DeleteInvoiceLineItem(Models.ViewModels.Sales.AddSalesInvoice model)
        {
            model.Customers = Models.ModelViewHelper.Customers();
            model.Items = Models.ModelViewHelper.Items();
            model.Measurements = Models.ModelViewHelper.Measurements();

            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            model.AddSalesInvoiceLines.Remove(model.AddSalesInvoiceLines.Where(i => i.Item == deletedItem.ToString()).FirstOrDefault());

            return View(model);
        }

        public ActionResult AddSalesReceipt(int? salesInvoiceId = null)
        {
            var model = new Models.ViewModels.Sales.AddSalesReceipt();
            if (salesInvoiceId.HasValue)
            {
                var salesInvoice = _salesService.GetSalesInvoiceById(salesInvoiceId.Value);
                model.SalesInvoiceId = salesInvoice.Id;
                model.SalesInvoiceNo = salesInvoice.InvoiceNo;
                model.InvoiceDate = salesInvoice.InvoiceDate;
                model.Date = DateTime.Now;
                model.CustomerId = salesInvoice.CustomerId;

                foreach (var line in salesInvoice.SalesInvoiceLines)
                {
                    model.AddSalesReceiptLines.Add(new Models.ViewModels.Sales.AddSalesReceiptLine()
                    {
                        SalesInvoiceLineId = line.Id,
                        Item = line.Item,
                        //MeasurementId = line.MeasurementId,
                        Quantity = line.Quantity,
                        Discount = line.Discount,
                        Amount = line.Amount
                    });
                }
            }
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddSalesReceipt")]
        [FormValueRequiredAttribute("SaveSalesReceipt")]
        public ActionResult SaveSalesReceipt(Models.ViewModels.Sales.AddSalesReceipt model)
        {
            var receipt = new SalesReceiptHeader()
            {
                AccountToDebitId = model.AccountToDebitId,
                //SalesInvoiceHeaderId = model.SalesInvoiceId,
                CustomerId = model.CustomerId.Value,
                Date = model.Date,
            };
            foreach(var line in model.AddSalesReceiptLines)
            {
                if(line.AmountToPay > line.Amount)
                    return RedirectToAction("SalesInvoices");

                receipt.SalesReceiptLines.Add(new SalesReceiptLine()
                {
                    SalesInvoiceLineId = line.SalesInvoiceLineId,
                    Item = line.Item,
                   // MeasurementId = line.MeasurementId,
                    Quantity = line.Quantity,
                    Discount = line.Discount,
                    Amount = line.Amount,
                    AmountPaid = line.AmountToPay,
                });
            }
            //_salesService.AddSalesReceipt(receipt);
            return RedirectToAction("SalesInvoices");
        }
        [Audit]
        public ActionResult Customers()
        {
            var customers = _salesService.GetCustomers();
            var model = new Models.ViewModels.Sales.Customers();
            foreach(var customer in customers)
            {
                model.CustomerListLines.Add(new Models.ViewModels.Sales.CustomerListLine()
                {
                    Id = customer.Id,
                    Name = customer.Username,
                    Balance = customer.Balance
                });
            }
            return View(model);
        }

        public ActionResult Customer(int id = 0)
        {
            var model = new Models.ViewModels.Sales.CustomerViewModel();
            if (id == 0)
            {
                model.Id = id;
            }
            else
            {
                var customer = _salesService.GetCustomerById(id);
                var allocations = _salesService.GetCustomerReceiptsForAllocation(id);
                model.Id = customer.Id;
                model.Name = customer.Username;
                model.Balance = customer.Balance;
                foreach (var receipt in allocations)
                {
                    model.CustomerAllocations.Add(new Models.ViewModels.Sales.CustomerAllocation()
                    {
                        Id = receipt.Id,
                        AmountAllocated = receipt.SalesReceiptLines.Sum(a => a.AmountPaid),
                        AvailableAmountToAllocate = receipt.AvailableAmountToAllocate
                    });
                }
                foreach (var invoice in customer.SalesInvoices)
                {
                    model.CustomerInvoices.Add(new Models.ViewModels.Sales.CustomerSalesInvoice()
                    {
                        Id = invoice.Id,
                        InvoiceNo = invoice.InvoiceNo,
                        Date = invoice.InvoiceDate,
                        Amount = invoice.SalesInvoiceLines.Sum(a => a.Amount),
                        Status = invoice.IsFullPaid() ? "Paid" : "Open"
                    });
                }            
            }
            return View(model);
        }

        public ActionResult AddCustomer()
        {
            return View();
        }
        [Audit]
        [HttpPost, ActionName("AddCustomer")]
        [FormValueRequiredAttribute("SaveCustomer")]
        public ActionResult AddCustomer(Customer model)
        {
            _salesService.AddCustomer(new Customer()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Website = model.Website,
                Email = model.Email,
                CustNo = model.CustNo,
                Phone= model.Phone,
                ResaleNo = model.ResaleNo,
                Mobile = model .Mobile ,
                AccountsReceivableAccountId = model.AccountsReceivableAccountId,
                Address = model.Address,
                CardHoldersName = model.CardHoldersName,
                City= model.City,
                Country = model.Country,
                CreditCardNo = model.CreditCardNo,
                ExpirationDate = model.ExpirationDate,
                IsActive = model.IsActive,
                OpenPONumber = model.OpenPONumber,
                PriceLevel = model.PriceLevel,
                Username = model.Username,
                State = model.State,
                Shipping = model.Shipping,
                SalesRep = model.SalesRep,
                SalesTax = model.SalesTax,
                Zip = model.Zip
            });
            //var customer = new Customer()
            //{
            //    FirstName = model.FirstName,
            //    LastName = model.LastName,
            //    Website = model.Website,
            //    Email = model.Email,
            //    No = model.No

            //};
            //_salesService.AddCustomer(customer);
            return RedirectToAction("Customers");
        }

        public ActionResult AddOrEditCustomer(int id = 0)
        {
            Models.ViewModels.Sales.EditCustomer model = new Models.ViewModels.Sales.EditCustomer();
            var accounts = _financialService.GetAccounts();
            if (id != 0)
            {
                var customer = _salesService.GetCustomerById(id);
                model.Id = customer.Id;
                //model.Name = customer.Party.Name;
                model.Name = customer.Username;
                //model.PrimaryContactId = customer.PrimaryContactId.HasValue ? customer.PrimaryContactId.Value : -1;
                //model.PrimaryContactId = customer.PrimaryContactId.HasValue ? customer.SalesRep : -1;
                model.AccountsReceivableAccountId = customer.AccountsReceivableAccountId.HasValue ? customer.AccountsReceivableAccountId : -1;
                model.SalesAccountId = customer.SalesAccountId.HasValue ? customer.SalesAccountId : -1;
                model.SalesDiscountAccountId = customer.SalesDiscountAccountId.HasValue ? customer.SalesDiscountAccountId : -1;
                model.PromptPaymentDiscountAccountId = customer.PromptPaymentDiscountAccountId.HasValue ? customer.PromptPaymentDiscountAccountId : -1;
                model.CustomerAdvancesAccountId = customer.CustomerAdvancesAccountId.HasValue ? customer.CustomerAdvancesAccountId : -1;
            }
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddOrEditCustomer")]
        [FormValueRequiredAttribute("SaveCustomer")]
        public ActionResult AddOrEditCustomer(Models.ViewModels.Sales.EditCustomer model)
        {
            Customer customer = null;
            if (model.Id != 0)
            {
                customer = _salesService.GetCustomerById(model.Id);                
            }
            else
            {
                customer = new Customer();
                //customer.Party = new Core.Domain.Party();
            }
            
            //customer.Party.Name = model.Name;
            customer.Username = model.Name;
            //if (model.PrimaryContactId != -1) customer.PrimaryContactId = model.PrimaryContactId;
            customer.AccountsReceivableAccountId = model.AccountsReceivableAccountId.Value == -1 ? null : model.AccountsReceivableAccountId;
            customer.SalesAccountId = model.SalesAccountId.Value == -1 ? null : model.SalesAccountId;
            customer.SalesDiscountAccountId = model.SalesDiscountAccountId.Value == -1 ? null : model.SalesDiscountAccountId;
            customer.PromptPaymentDiscountAccountId = model.PromptPaymentDiscountAccountId.Value == -1 ? null : model.PromptPaymentDiscountAccountId;
            customer.CustomerAdvancesAccountId = model.CustomerAdvancesAccountId.Value == -1 ? null : model.CustomerAdvancesAccountId;

            if (model.Id != 0)
                _salesService.UpdateCustomer(customer);
            else
                _salesService.AddCustomer(customer);

            return RedirectToAction("Customers");
        }

        public ActionResult CustomerDetail(int id)
        {
            var customer = _salesService.GetCustomerById(id);
            var allocations = _salesService.GetCustomerReceiptsForAllocation(id);
            var model = new Models.ViewModels.Sales.CustomerDetail()
            {
                Id = customer.Id,
                Name = customer.Username,
                Balance = customer.Balance
            };
            foreach(var receipt in allocations)
            {
                model.CustomerAllocations.Add(new Models.ViewModels.Sales.CustomerAllocation()
                {
                    Id = receipt.Id,
                    AmountAllocated = receipt.SalesReceiptLines.Sum(a => a.AmountPaid),
                    AvailableAmountToAllocate = receipt.AvailableAmountToAllocate
                });
            }
            foreach (var allocation in customer.CustomerAllocations)
            {
                model.ActualAllocations.Add(new Models.ViewModels.Sales.Allocations()
                {
                    InvoiceNo = allocation.SalesInvoiceHeader.InvoiceNo,
                    ReceiptNo = allocation.SalesReceiptHeader.ReceiptNo,
                    Date = allocation.Date,
                    Amount = allocation.Amount
                });
            }
            foreach(var invoice in customer.SalesInvoices)
            {
                model.CustomerInvoices.Add(new Models.ViewModels.Sales.CustomerSalesInvoice()
                {
                    Id = invoice.Id,
                    InvoiceNo = invoice.InvoiceNo,
                    Date = invoice.InvoiceDate,
                    Amount = invoice.ComputeTotalAmount(),
                    Status = invoice.Status.ToString()
                 });
            }
            return View(model);
        }

        public ActionResult Allocate(int id)
        {
            var receipt = _salesService.GetSalesReceiptById(id);
            var customer = _salesService.GetCustomerById(receipt.CustomerId);
            var allocations = _salesService.GetCustomerReceiptsForAllocation(customer.Id);
            var model = new Models.ViewModels.Sales.Allocate();
            model.ReceiptId = id;
            model.TotalAmountAvailableToAllocate = allocations.Sum(a => a.AvailableAmountToAllocate);
            model.LeftToAllocateFromReceipt = receipt.AvailableAmountToAllocate;
            var openInvoices = _salesService.GetSalesInvoices().Where(inv => inv.IsFullPaid() == false);
            foreach(var invoice in openInvoices)
            {
                model.OpenInvoices.Add(new SelectListItem()
                {
                    Value = invoice.Id.ToString(),
                    Text = invoice.InvoiceNo + " - " + (invoice.ComputeTotalAmount() - invoice.SalesInvoiceLines.Sum(a => a.GetAmountPaid()))
                });
            }
            return PartialView(model);
        }
        [Audit]
        [HttpPost, ActionName("Allocate")]
        [FormValueRequiredAttribute("SaveAllocation")]
        public ActionResult SaveAllocation(Models.ViewModels.Sales.Allocate model)
        {
            var receipt = _salesService.GetSalesReceiptById(model.ReceiptId);
            var customer = _salesService.GetCustomerById(receipt.CustomerId);
            var invoice = _salesService.GetSalesInvoiceById(model.InvoiceId);
            var allocations = _salesService.GetCustomerReceiptsForAllocation(customer.Id);
            model.InvoiceId = model.InvoiceId;
            model.TotalAmountAvailableToAllocate = allocations.Sum(a => a.AvailableAmountToAllocate);
            model.LeftToAllocateFromReceipt = receipt.AvailableAmountToAllocate;
            if (invoice == null)
            {
                return View(model);
            }
            else
            {
                var invoiceTotalAmount = invoice.ComputeTotalAmount();
                if (model.AmountToAllocate > invoiceTotalAmount
                    || model.AmountToAllocate > receipt.AvailableAmountToAllocate
                    || invoice.Status == Core.Domain.SalesInvoiceStatus.Closed)
                {
                    return View(model);
                }

                var allocation = new CustomerAllocation()
                {
                    CustomerId = customer.Id,
                    SalesReceiptHeaderId = receipt.Id,
                    SalesInvoiceHeaderId = invoice.Id,
                    Amount = model.AmountToAllocate,
                    Date = DateTime.Now
                };
                //_salesService.SaveCustomerAllocation(allocation);
            }
            return RedirectToAction("CustomerDetail", new { id = customer.Id });
        }

        /// <summary>
        /// Add new receipt with no invoice
        /// </summary>
        /// <returns></returns>
        public ActionResult AddReceipt()
        {
            var model = new Models.ViewModels.Sales.AddSalesReceipt();
            //TODO: get the default customer advances account from GL setting if there is.
            model.AccountToCreditId = _financialService.GetAccounts().Where(a => a.AccountName == "Customer Advances").FirstOrDefault() != null
                ? _financialService.GetAccounts().Where(a => a.AccountName == "Customer Advances").FirstOrDefault().Id
                : -1;

            return View(model);
        }

        [HttpPost, ActionName("AddReceipt")]
        [FormValueRequiredAttribute("SaveReceipt")]
        public ActionResult SaveReceipt(Models.ViewModels.Sales.AddSalesReceipt model)
        {
            if (model.AccountToDebitId.Value == -1 || model.CustomerId.Value == -1)
                return View(model);

            var receipt = new SalesReceiptHeader()
            {
                AccountToDebitId = model.AccountToDebitId,
                CustomerId = model.CustomerId.Value,
                Date = model.Date,
                Amount = model.PaymentAmount
            };

            receipt.SalesReceiptLines.Add(new SalesReceiptLine()
            {
                AccountToCreditId = model.AccountToCreditId,
                Amount = model.Amount,
                AmountPaid = model.PaymentAmount,
            });

           // _salesService.AddSalesReceiptNoInvoice(receipt);
            return RedirectToAction("Receipts");
        }
        [Audit]
        [HttpPost, ActionName("AddReceipt")]
        [FormValueRequiredAttribute("AddReceiptItem")]
        public ActionResult AddReceiptItem(Models.ViewModels.Sales.AddSalesReceipt model)
        {
            var rowId = Guid.NewGuid();
            if ( model.Quantity > 0)
            {
                //var item = _inventoryService.GetItemById(model.ItemId.Value);
                //if (!item.Price.HasValue)
                //{
                //    ModelState.AddModelError("Amount", "Selling price is not set.");
                //    return View(model);
                //}
                Models.ViewModels.Sales.AddSalesReceiptLine itemModel = new Models.ViewModels.Sales.AddSalesReceiptLine()
                {
                    RowId = rowId.ToString(),
                    Item = model.Item,
                    //MeasurementId = model.MeasurementId.Value,
                    Quantity = model.Quantity,
                    Discount = model.Discount,
                    Price  = model.Price,
                    Amount = model.Price.Value * model.Quantity,
                    AmountToPay = model.AmountToPay
                };
                if (model.AddSalesReceiptLines.FirstOrDefault(i => i.Item == model.Item) == null)
                    model.AddSalesReceiptLines.Add(itemModel);
            }
            else if(!string.IsNullOrEmpty(model.AccountCode) && model.Amount != 0)
            {
                var account = _financialService.GetAccounts().Where(a => a.AccountCode.ToString() == model.AccountCode).FirstOrDefault();
                if(account != null)
                {
                    Models.ViewModels.Sales.AddSalesReceiptLine accountItemModel = new Models.ViewModels.Sales.AddSalesReceiptLine()
                    {
                        RowId = rowId.ToString(),
                        AccountToCreditId = account.Id,
                        Amount = model.AmountToPay,
                        AmountToPay = model.AmountToPay,
                    };
                    model.AddSalesReceiptLines.Add(accountItemModel);
                }                
            }
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddReceipt")]
        [FormValueRequiredAttribute("DeleteReceiptLineItem")]
        public ActionResult DeleteReceiptLineItem(Models.ViewModels.Sales.AddSalesReceipt model)
        {
            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            model.AddSalesReceiptLines.Remove(model.AddSalesReceiptLines.Where(i => i.Item == deletedItem.ToString()).FirstOrDefault());
            return View(model);
        }
        [Audit]
        public ActionResult Receipts()
        {
            var receipts = _salesService.GetSalesReceipts();
            var model = new Models.ViewModels.Sales.SalesReceipts();
            foreach(var receipt in receipts)
            {
                model.SalesReceiptListLines.Add(new Models.ViewModels.Sales.SalesReceiptListLine()
                {
                    No = receipt.ReceiptNo,
                    //InvoiceNo = receipt.SalesInvoiceHeader != null ? receipt.SalesInvoiceHeader.No : string.Empty,
                    CustomerId = receipt.CustomerId,
                    CustomerName = receipt.Customer.CustNo,
                    Date = receipt.Date,
                    Amount = receipt.SalesReceiptLines.Sum(r => r.Amount),
                    AmountPaid = receipt.SalesReceiptLines.Sum(r => r.AmountPaid)
                });
            }
            return View(model);
        }

        public ActionResult SalesDelivery(int id = 0)
        {
            var model = new Web.Models.ViewModels.Sales.SalesHeaderViewModel(_inventoryService, _financialService);
            model.DocumentType = Core.Domain.DocumentTypes.SalesDelivery;

            if (id == 0)
            {
                return View(model);
            }
            else
            {
                var invoice = _salesService.GetSalesInvoiceById(id);
                model.Id = invoice.Id;
                model.CustomerId = invoice.CustomerId;
                model.Date = invoice.InvoiceDate;
                model.No = invoice.InvoiceNo;
                foreach (var line in invoice.SalesInvoiceLines)
                {
                    var lineItem = new Models.ViewModels.Sales.SalesLineItemViewModel(_financialService);
                    lineItem.Id = line.Id;
                    lineItem.Item = line.Item;
                    //lineItem.ItemNo = line.Item.No;
                    lineItem.ItemDescription = line.ItemDescription;
                    //lineItem.Measurement = line.Measurement.Description;
                    lineItem.Quantity = line.Quantity;
                    lineItem.Discount = line.Discount;
                    lineItem.Price = line.Amount;
                    model.SalesLine.SalesLineItems.Add(lineItem);
                }
                return View(model);
            }
        }

        public ActionResult SalesInvoice(int id = 0)
        {
            var model = new Web.Models.ViewModels.Sales.SalesHeaderViewModel(_inventoryService, _financialService);
            model.DocumentType = Core.Domain.DocumentTypes.SalesInvoice;

            if (id == 0)
            {   
                return View(model);
            }
            else
            {
                var invoice = _salesService.GetSalesInvoiceById(id);
                model.Id = invoice.Id;
                model.CustomerId = invoice.CustomerId;
                model.Date = invoice.InvoiceDate;
                model.No = invoice.InvoiceNo;
                foreach (var line in invoice.SalesInvoiceLines)
                {
                    var lineItem = new Models.ViewModels.Sales.SalesLineItemViewModel(_financialService);
                    lineItem.SetServiceHelpers(_financialService);
                    lineItem.Id = line.Id;
                    lineItem.CustomerId = invoice.CustomerId;
                    lineItem.Item = line.Item;
                    //lineItem.ItemNo = line.Item.No;
                    lineItem.ItemDescription = line.ItemDescription;
                    //lineItem.Measurement = line.Measurement.Description;
                    lineItem.Quantity = line.Quantity;
                    lineItem.Discount = line.Discount;
                    lineItem.Price = line.Amount;
                    model.SalesLine.SalesLineItems.Add(lineItem);
                }
                return View(model);
            }
        }

        public ActionResult SalesOrder(int id = 0)
        {
            var model = new Models.ViewModels.Sales.SalesHeaderViewModel(_inventoryService, _financialService);
            model.DocumentType = Core.Domain.DocumentTypes.SalesOrder;

            if (id == 0)
            {
                return View(model);
            }
            else
            {
                var order = _salesService.GetSalesOrderById(id);
                model.Id = order.Id;
                model.CustomerId = order.CustomerId;
                //model.PaymentTermId = order.PaymentTermId;
                model.Date = order.Date;
                model.Reference = order.ReferenceNo;
                model.No = order.No;

                foreach (var line in order.SalesOrderLines)
                {
                    var lineItem = new Models.ViewModels.Sales.SalesLineItemViewModel(_financialService);
                    lineItem.Id = line.Id;
                    lineItem.Item = line.Item;
                    //lineItem.ItemNo = line.Item.No;
                    lineItem.ItemDescription = line.ItemDescription;
                    //lineItem.Measurement = line.Measurement.Description;
                    lineItem.Quantity = line.Quantity;
                    lineItem.Discount = line.Discount;
                    lineItem.Price = line.Amount;
                    model.SalesLine.SalesLineItems.Add(lineItem);
                }
                return View(model);
            }
        }
        [Audit]
        [HttpPost, ActionName("SalesOrder")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult SaveOrder(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            SalesOrderHeader order = null;
            if (model.Id == 0)
            {
                order = new SalesOrderHeader();
            }
            else
            {
                order = _salesService.GetSalesOrderById(model.Id);
            }
            
            order.CustomerId = model.CustomerId.Value;
            //order.PaymentTermId = model.PaymentTermId;
            order.Date = model.Date;
            
            foreach (var line in model.SalesLine.SalesLineItems)
            {
                SalesOrderLine lineItem = null;
                //var item = _inventoryService.GetItemByNo(line.ItemNo);
                if (!line.Id.HasValue)
                {
                    lineItem = new SalesOrderLine();
                    order.SalesOrderLines.Add(lineItem);
                }
                else
                {
                    lineItem = order.SalesOrderLines.Where(i => i.Id == line.Id).FirstOrDefault();
                }
                
                lineItem.Item = line.Item;
                //lineItem.MeasurementId = item.SellMeasurementId.Value;
                lineItem.Quantity = line.Quantity;
                lineItem.Discount = line.Discount;
                lineItem.Amount = line.Price;
            }

            if (model.Id == 0)
                _salesService.AddSalesOrder(order, true);
            else
                _salesService.UpdateSalesOrder(order);

            return RedirectToAction("SalesOrders");
        }
        [Audit]
        [HttpPost, ActionName("SalesDelivery")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult SaveDelivery(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            SalesDeliveryHeader delivery = null;
            if (model.Id == 0)
            {
                delivery = new SalesDeliveryHeader();
            }
            else
            {
                delivery = _salesService.GetSalesDeliveryById(model.Id);
            }
            
            delivery.CustomerId = model.CustomerId.Value;
            //delivery.PaymentTermId = model.PaymentTermId;
            delivery.Date = model.Date;

            foreach (var line in model.SalesLine.SalesLineItems)
            {
                SalesDeliveryLine lineItem = null;
              //  var item = _inventoryService.GetItemByNo(line.ItemNo);
                if (!line.Id.HasValue)
                {
                    lineItem = new SalesDeliveryLine();
                    delivery.SalesDeliveryLines.Add(lineItem);
                }
                else
                {
                    lineItem = delivery.SalesDeliveryLines.Where(i => i.Id == line.Id).FirstOrDefault();
                }
                
                lineItem.Item = line.Item;
                //lineItem.MeasurementId = item.SellMeasurementId.Value;
                lineItem.Quantity = line.Quantity;
                lineItem.Discount = line.Discount;
                lineItem.Price = line.Price;
            }

            //if (model.Id == 0)
            //{
            //    _salesService.AddSalesDelivery(delivery, true);
            //}
            //else
            //{
                
            //}

            return RedirectToAction("SalesDeliveries");
        }

        [Audit]
        [HttpPost, ActionName("SalesInvoice")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult SaveInvoice(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            SalesInvoiceHeader invoice = null;
            if (model.Id == 0)
            {
                invoice = new SalesInvoiceHeader();
            }
            else
            {
                invoice = _salesService.GetSalesInvoiceById(model.Id);
            }
            
            invoice.CustomerId = model.CustomerId.Value;
            invoice.InvoiceDate = model.Date;
            invoice.ShippingHandlingCharge = model.ShippingHandlingCharges;
            
            foreach (var line in model.SalesLine.SalesLineItems)
            {
                SalesInvoiceLine lineItem = null;
                //var item = _inventoryService.GetItemByNo(line.ItemNo);
                if (!line.Id.HasValue)
                {
                    lineItem = new SalesInvoiceLine();
                    invoice.SalesInvoiceLines.Add(lineItem);
                }
                else
                {
                    lineItem = invoice.SalesInvoiceLines.Where(i => i.Id == line.Id).FirstOrDefault();
                }
                
                lineItem.Item = line.Item;
                //lineItem.MeasurementId = item.SellMeasurementId.Value;
                lineItem.Quantity = line.Quantity;
                lineItem.Discount = line.Discount;
                lineItem.Amount = line.Price;
            }

            if (model.Id == 0)
            {
                _salesService.AddSalesInvoice(invoice, null);
            }
            else
            {
                _salesService.UpdateSalesInvoice(invoice);
            }

            return RedirectToAction("SalesInvoices");
        }
        [Audit]
        [HttpPost, ActionName("SalesOrder")]
        [FormValueRequiredAttribute("AddLineItem")]
        public ActionResult AddLineItemOrder(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            AddLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("SalesInvoice")]
        [FormValueRequiredAttribute("AddLineItem")]
        public ActionResult AddLineItemInvoice(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            AddLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("SalesDelivery")]
        [FormValueRequiredAttribute("AddLineItem")]
        public ActionResult AddLineItemDelivery(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            AddLineItem(model);
            return ReturnView(model);
        }

        public ActionResult ReturnView(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            string actionName = string.Empty;
            switch (model.DocumentType)
            {
                case Core.Domain.DocumentTypes.SalesOrder:
                    actionName = "SalesOrder";
                    break;
                case Core.Domain.DocumentTypes.SalesInvoice:
                    actionName = "SalesInvoice";
                    break;
                case Core.Domain.DocumentTypes.SalesDelivery:
                    actionName = "SalesDelivery";
                    break;
            }

            return View(actionName, model);
        }

        private void AddLineItem(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            //if (!model.CustomerId.HasValue)
            //    throw new Exception("Please enter customer.");
            //var item = _inventoryService.GetInventoryById(model.Id);
            //var item = _inventoryService.GetItemByNo(model.SalesLine.ItemNo);
            var newLine = new Models.ViewModels.Sales.SalesLineItemViewModel(_financialService);
            newLine.CustomerId = model.CustomerId.HasValue ? model.CustomerId.Value : 0;
            newLine.Item = model.SalesLine.Item;
            //newLine.ItemNo = item.No;
            newLine.ItemDescription = model.SalesLine.ItemDescription;
            //newLine.Measurement = item.SellMeasurement.Description;
            newLine.Quantity = model.SalesLine.Quantity;
            newLine.Price = model.SalesLine.Price;
            newLine.Discount = model.SalesLine.Discount;            
            model.SalesLine.SalesLineItems.Add(newLine);

            foreach (var line in model.SalesLine.SalesLineItems)
                line.SetServiceHelpers(_financialService);
        }
        [Audit]
        [HttpPost, ActionName("SalesOrder")]
        [FormValueRequiredAttribute("DeleteLineItem")]
        public ActionResult DeleteLineItemOrder(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            DeleteLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("SalesInvoice")]
        [FormValueRequiredAttribute("DeleteLineItem")]
        public ActionResult DeleteLineItemInvoice(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            DeleteLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("SalesDelivery")]
        [FormValueRequiredAttribute("DeleteLineItem")]
        public ActionResult DeleteLineItemDelivery(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            DeleteLineItem(model);
            return ReturnView(model);
        }

        private void DeleteLineItem(Models.ViewModels.Sales.SalesHeaderViewModel model)
        {
            model.SetServiceHelpers(_inventoryService, _financialService);
            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            if (!string.IsNullOrEmpty(deletedItem))
            {
                model.SalesLine.SalesLineItems.RemoveAt(int.Parse(deletedItem));
            }
        }

        public ActionResult AddCustomerContact()
        {
            var model = new Web.Models.ViewModels.ContactViewModel();
            return View("Contact", model);
        }
        [Audit]
        [HttpPost]
        public ActionResult AddCustomerContact(Web.Models.ViewModels.ContactViewModel model)
        {
            var contact = new Core.Domain.Contact();
            contact.ContactType = Core.Domain.ContactTypes.Customer;
            contact.FirstName = model.FirstName;
            contact.MiddleName = model.MiddleName;
            contact.LastName = model.LastName;
            _salesService.SaveContact(contact);
            if(contact.Id > 0)
                return Json(new { Status = "success" });
            else
                return Json(new { Status = "falied" });
        }
    }
}

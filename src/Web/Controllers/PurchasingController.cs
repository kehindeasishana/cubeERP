using Core.Domain.Purchases;
using Services.Financial;
using Services.Inventory;
using Services.Purchasing;
using Services.TaxSystem;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class PurchasingController : BaseController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IFinancialService _financialService;
        private readonly IPurchasingService _purchasingService;
        private readonly ITaxService _taxService;

        public PurchasingController(IInventoryService inventoryService,
            IFinancialService financialService,
            IPurchasingService purchasingService,
            ITaxService taxService)
        {
            _inventoryService = inventoryService;
            _financialService = financialService;
            _purchasingService = purchasingService;
            _taxService = taxService;
        }
        [Audit]
        public ActionResult PurchaseOrders()
        {
            var purchaseOrders = _purchasingService.GetPurchaseOrders();
          
            var model = new Models.ViewModels.Purchases.PurchaseOrders();
            foreach (var po in purchaseOrders)
            {
                model.PurchaseOrderListLines.Add(new Models.ViewModels.Purchases.PurchaseOrderListLine()
                {
                    Id = po.Id,
                    No = po.No,
                    Date = po.Date,
                    Vendor = po.Vendor.No,
                    Amount = po.PurchaseOrderLines.Sum(e => e.Amount),
                    Completed = po.IsCompleted(),
                    Paid = po.IsPaid(),
                    HasInvoiced = po.PurchaseInvoiceHeaderId.HasValue
                });
            }
            return View(model);
        }

        public ActionResult AddPurchaseOrder()
        {
            var model = new Models.ViewModels.Purchases.AddPurchaseOrder();
            var items = _inventoryService.GetAllInventories();
            //var items = _inventoryService.GetAllItems();
            var accounts = _financialService.GetAccounts();
            
            //var measurements = _inventoryService.GetMeasurements();
           // var taxes = _financialService.GetTaxes();
            var categories = _inventoryService.GetAllCategories();
            var itemCategories = _inventoryService.GetItemCategories();
            var vendors = _purchasingService.GetVendors();
            model.Items = Models.ModelViewHelper.Items();
            model.Vendors = Models.ModelViewHelper.Vendors();
            //model.UnitOfMeasurements = Models.ModelViewHelper.Measurements();
            return View(model);
        }

        [HttpPost, ActionName("AddPurchaseOrder")]
        [FormValueRequiredAttribute("AddPurchaseOrderLine")]
        public ActionResult AddPurchaseOrderLine(Models.ViewModels.Purchases.AddPurchaseOrder model)
        {
            
            var items = _inventoryService.GetAllInventories();
            var accounts = _financialService.GetAccounts();
            var measurements = _inventoryService.GetMeasurements();
            //var taxes = _financialService.GetTaxes();
            var itemCategories = _inventoryService.GetItemCategories();
            var vendors = _purchasingService.GetVendors();
           // model.Items = Models.ListHelper.GetInventoryList();
            model.Items = Models.ModelViewHelper.Items();
            model.Vendors = Models.ModelViewHelper.Vendors();
            model.UnitOfMeasurements = Models.ModelViewHelper.Measurements();
            try
            {
                if (model.Quantity > 0)
                {
                    //var item = _inventoryService.GetInventoryById(model.ItemId);
                   // var item = _inventoryService.GetItemById(model.ItemId);
                    model.PurchaseOrderLines.Add(new Models.ViewModels.Purchases.AddPurchaseOrderLine()
                    {
                        Item = model.Item,
                        Description = model.Description,
                        UnitPrice = model.UnitPrice,
                        Amount = model.Amount,
                        TotalLineAmount = model.UnitPrice * model.Quantity,
                        GLAccount = model.GLAccount,
                        //Cost = item.Cost,
                        //UnitOfMeasurementId = model.UnitOfMeasurementId,
                        Quantity = model.Quantity,
                       // TotalLineCost = item.Cost.Value * model.Quantity
                    });
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost, ActionName("AddPurchaseOrder")]
        [FormValueRequiredAttribute("SavePurchaseOrder")]
        public ActionResult SavePurchaseOrder(Models.ViewModels.Purchases.AddPurchaseOrder model)
        {
            //var items = _inventoryService.GetAllItems();
            var items = _inventoryService.GetAllInventories();
            var accounts = _financialService.GetAccounts();
            var measurements = _inventoryService.GetMeasurements();
            //var taxes = _financialService.GetTaxes();
            var itemCategories = _inventoryService.GetItemCategories();
            var vendors = _purchasingService.GetVendors();
            model.Items = Models.ModelViewHelper.Items();
            model.Vendors = Models.ModelViewHelper.Vendors();
            model.UnitOfMeasurements = Models.ModelViewHelper.Measurements();
            try
            {
                var po = new PurchaseOrderHeader()
                {
                    VendorId = model.VendorId,
                    Date = model.Date,
                    ShippingAddress = model.ShippingAddress,
                    CustInvoiceNo = model.CustInvoiceNo,
                    CustSalesOrder = model.CustInvoiceNo,
                    Discount = model .DiscountAmount,
                    ShipVia  = model .ShipVia ,
                    PayableAccountId = model.AccountPayableAcc,
                    Terms = model .Terms ,
                };
                foreach (var item in model.PurchaseOrderLines)
                {
                    //var persistedItem = _inventoryService.GetItemById(item.ItemId);
                    po.PurchaseOrderLines.Add(new PurchaseOrderLine()
                    {
                        Amount = item.TotalLineAmount,
                        Item = item.Item,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        GLAccountId = item.GLAccount,
                        //GlAccountId = item.GLAccount,
                       Description = item.Description
                       // MeasurementId = item.UnitOfMeasurementId,
                        //Cost = persistedItem.Cost.Value,
                    });
                }
                _purchasingService.AddPurchaseOrder(po);
                return RedirectToAction("PurchaseOrders");
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost, ActionName("AddPurchaseOrder")]
        [FormValueRequiredAttribute("DeletePurchaseOrderLine")]
        public ActionResult DeletePurchaseOrderLine(Web.Models.ViewModels.Purchases.AddPurchaseOrder model)
        {
            var items = _inventoryService.GetAllInventories();
            var accounts = _financialService.GetAccounts();
            //var measurements = _inventoryService.GetMeasurements();
            //var taxes = _financialService.GetTaxes();
            var categories = _inventoryService.GetAllCategories();
            var itemCategories = _inventoryService.GetItemCategories();
            var vendors = _purchasingService.GetVendors();
            model.Items = Models.ModelViewHelper.Items();
            model.Vendors = Models.ModelViewHelper.Vendors();
            //model.UnitOfMeasurements = Models.ModelViewHelper.Measurements();

            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            model.PurchaseOrderLines.Remove(model.PurchaseOrderLines.Where(i => i.Item == deletedItem.ToString()).FirstOrDefault());
            return View(model);
        }

        //public ActionResult AddPurchaseOrder()
        //{
           
        //    return View();
        //}
        //[Audit]
        //[HttpPost, ActionName("AddPurchaseOrder")]
        ////[FormValueRequiredAttribute("AddPurchaseOrderLine")]
        //public ActionResult AddPurchaseOrder(PurchaseOrderHeader model)
        //{
        //    _purchasingService.AddPurchaseOrder(new PurchaseOrderHeader()
        //    {
        //        VendorId = model.VendorId,
        //        Amount = model.Amount,
        //        Date = model.Date,
        //        PurchaseOrderNo = model.PurchaseOrderNo,
        //        QuantityReceived = model.QuantityReceived,
        //        CustInvoiceNo = model.CustInvoiceNo,
        //        UnitPrice = model.UnitPrice,
        //        CustSalesOrder = model.CustSalesOrder,
        //        Discount = model.Discount,
        //        GLAccountId = model .GLAccountId ,
        //        ShippingAddress = model.ShippingAddress,
        //        Item = model.Item,
        //        ShipVia = model .ShipVia,
        //        Terms = model.Terms,
        //        Description = model.Description,
                
        //        AccountPayableCategory = model.AccountPayableCategory
                
        //    });

        //    return RedirectToAction("ViewPurchaseOrder");
        //}

        [Audit]
        [Authorize]
        public ViewResult GetPurchaseOrder(int id)
        {
            PurchaseOrderHeader purchaseOrder = _purchasingService.GetPurchaseOrderById(id);
            
            return View(purchaseOrder);

        }
        [Audit]
        public ActionResult ViewPurchaseOrder()
        {
            var purchaseOrder = _purchasingService.GetPurchaseOrders();
            
            return View(purchaseOrder);

        }

        public ActionResult EditPurchaseOrder(int id)
        {
            return View(this ._purchasingService.GetPurchaseOrderById(id));
            
        }
        [Audit]
        [HttpPost]
        public ActionResult EditPurchaseOrder(PurchaseOrderHeader model)
        {
            var purchaseOrder = _purchasingService.GetPurchaseOrderById(model.Id);
            purchaseOrder.PurchaseOrderNo = model.PurchaseOrderNo;
            purchaseOrder.QuantityReceived = model.QuantityReceived;
            purchaseOrder.Item = model.Item;
            purchaseOrder.ShippingAddress = model.ShippingAddress;
            purchaseOrder.ShipVia = model.ShipVia;
            purchaseOrder.Terms = model.Terms;
            purchaseOrder.UnitPrice = model.UnitPrice;
            purchaseOrder.VendorId = model.VendorId;
            purchaseOrder.AccountPayableCategory = model.AccountPayableCategory;
            purchaseOrder.Amount = model.Amount;
            purchaseOrder.CustInvoiceNo = model.CustInvoiceNo;
            purchaseOrder.CustSalesOrder = model.CustSalesOrder;
            purchaseOrder.Date = model.Date;
            purchaseOrder.Description = model.Description;
            purchaseOrder.Discount = model.Discount;
            _purchasingService.UpdatePurchaseOrder(purchaseOrder);
            
            return RedirectToAction("ViewPurchaseOrder");
        }

        [Audit]
        public ActionResult DeletePurchaseOrder(int id)
        {
            var purchaseOrder = _purchasingService.GetPurchaseOrderById(id);
            return View(purchaseOrder);
        }

        [HttpPost]
        public ActionResult DeletePurchaseOrder(int id, PurchaseOrderHeader purchaseOrder)
        {
            var purchaseOrders = _purchasingService.GetPurchaseOrderById(id);
            _purchasingService.DeletePurchaseOrder(purchaseOrders);
            return RedirectToAction("ViewPurchaseOrder");

        }
        //[Audit]
        //[HttpPost, ActionName("AddPurchaseOrder")]
        //[FormValueRequiredAttribute("SavePurchaseOrder")]
        //public ActionResult SavePurchaseOrder(Models.ViewModels.Purchases.AddPurchaseOrder model)
        //{
        //    var items = _inventoryService.GetAllItems();
        //    var accounts = _financialService.GetAccounts();
        //    var measurements = _inventoryService.GetMeasurements();
        //    //var taxes = _financialService.GetTaxes();
        //    var itemCategories = _inventoryService.GetItemCategories();
        //    var vendors = _purchasingService.GetVendors();
        //    model.Items = Models.ModelViewHelper.Items();
        //    model.Vendors = Models.ModelViewHelper.Vendors();
        //    model.UnitOfMeasurements = Models.ModelViewHelper.Measurements();
        //    try
        //    {
        //        var po = new PurchaseOrderHeader()
        //        {
        //            VendorId = model.VendorId,
        //            Date = model.Date,
        //            //No = _settingService.GetNextNumber(Core.Module.Common.Data.SequenceNumberTypes.PurchaseOrder).ToString(),
        //            //DocumentTypeId = (int)DocumentTypes.PurchaseOrder
        //        };
        //        foreach (var item in model.PurchaseOrderLines)
        //        {
        //            var persistedItem = _inventoryService.GetItemById(item.ItemId);
        //            po.PurchaseOrderLines.Add(new PurchaseOrderLine()
        //            {
        //                Amount = item.TotalLineCost,
        //                ItemId = item.ItemId,
        //                Quantity = item.Quantity,
        //                MeasurementId = item.UnitOfMeasurementId,
        //                Cost = persistedItem.Cost.Value,
        //            });
        //        }
        //        _purchasingService.AddPurchaseOrder(po, true);
        //        return RedirectToAction("PurchaseOrders");
        //    }
        //    catch
        //    {
        //        return View(model);
        //    }
        //}
        //[Audit]
        //[HttpPost, ActionName("AddPurchaseOrder")]
        //[FormValueRequiredAttribute("DeletePurchaseOrderLine")]
        //public ActionResult DeletePurchaseOrderLine(Web.Models.ViewModels.Purchases.AddPurchaseOrder model)
        //{
        //    var items = _inventoryService.GetAllItems();
        //    var accounts = _financialService.GetAccounts();
        //    var measurements = _inventoryService.GetMeasurements();
        //    //var taxes = _financialService.GetTaxes();
        //    var itemCategories = _inventoryService.GetItemCategories();
        //    var vendors = _purchasingService.GetVendors();
        //    model.Items = Models.ModelViewHelper.Items();
        //    model.Vendors = Models.ModelViewHelper.Vendors();
        //    model.UnitOfMeasurements = Models.ModelViewHelper.Measurements();

        //    var request = HttpContext.Request;
        //    var deletedItem = request.Form["DeletedLineItem"];
        //    model.PurchaseOrderLines.Remove(model.PurchaseOrderLines.Where(i => i.ItemId == int.Parse(deletedItem.ToString())).FirstOrDefault());
        //    return View(model);
        //}

        public ActionResult AddPurchaseReceipt(int id)
        {
            var po = _purchasingService.GetPurchaseOrderById(id);
            var model = new Models.ViewModels.Purchases.AddPurchaseReceipt();
            model.PreparePurchaseReceiptViewModel(po);
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddPurchaseReceipt")]
        [FormValueRequiredAttribute("SavePurchaseReceipt")]
        public ActionResult AddPurchaseReceipt(Models.ViewModels.Purchases.AddPurchaseReceipt model)
        {
            bool hasChanged = false;
            foreach (var line in model.PurchaseReceiptLines)
            {
                if (line.InQty.HasValue && line.InQty.Value != 0)
                {
                    hasChanged = true;
                    break;
                }
            }

            if (!hasChanged)
                return RedirectToAction("PurchaseOrders");

            var po = _purchasingService.GetPurchaseOrderById(model.Id);

            var poReceipt = new PurchaseReceiptHeader()
            {
                Date = DateTime.Now,
                Vendor = po.Vendor,
                VendorId = po.VendorId.Value,
                PurchaseOrderHeaderId = po.Id,
            };

            foreach (var receipt in model.PurchaseReceiptLines)
            {
                if((receipt.InQty + receipt.ReceiptQuantity) > receipt.Quantity)
                    return RedirectToAction("PurchaseOrders");

                poReceipt.PurchaseReceiptLines.Add(new PurchaseReceiptLine()
                {
                    PurchaseOrderLineId = receipt.PurchaseOrderLineId,
                    Item = receipt.Item,
                    //MeasurementId = receipt.UnitOfMeasurementId,
                    Quantity = receipt.Quantity,
                    ReceivedQuantity = (receipt.InQty.HasValue ? receipt.InQty.Value : 0),
                    Cost = receipt.Cost.Value,
                    Amount = receipt.Cost.Value * (receipt.InQty.HasValue ? receipt.InQty.Value : 0),
                });
            }

            _purchasingService.AddPurchaseOrderReceipt(poReceipt);
            return RedirectToAction("PurchaseOrders");
        }

        public ActionResult AddPurchaseInvoice(int? id = null)
        {
            Models.ViewModels.Purchases.AddPurchaseInvoice model = new Models.ViewModels.Purchases.AddPurchaseInvoice();
            if (id != null)
            {
                var existingPO = _purchasingService.GetPurchaseOrderById(id.Value);
                model.Date = existingPO.Date;
                model.Vendor = existingPO.Vendor.No;
                model.No = existingPO.No;
                model.Amount = existingPO.PurchaseOrderLines.Sum(a => a.Amount);

                foreach (var line in existingPO.PurchaseOrderLines)
                {
                    model.PurchaseInvoiceLines.Add(new Models.ViewModels.Purchases.AddPurchaseInvoiceLine()
                    {
                        Id = line.Id,
                        Item = line.Item,
                        //UnitOfMeasurementId = line.MeasurementId,
                        Description = line.Description,
                        Quantity = line.Quantity,
                        Cost = line.UnitPrice,
                        TotalLineCost = line.UnitPrice * line.Quantity,
                        ReceivedQuantity = line.GetReceivedQuantity().Value
                    });
                }
            }
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddPurchaseInvoice")]
        [FormValueRequiredAttribute("SavePurchaseInvoice")]
        public ActionResult AddPurchaseInvoice(Models.ViewModels.Purchases.AddPurchaseInvoice model)
        {
            if(string.IsNullOrEmpty(model.VendorInvoiceNo))
                return RedirectToAction("PurchaseOrders");

            var existingPO = _purchasingService.GetPurchaseOrderById(model.Id);
            var vendor = _purchasingService.GetVendorById(existingPO.VendorId.Value);

            var purchInvoice = new PurchaseInvoiceHeader()
            {
                InvoiceDate = model.Date,
                VendorInvoiceNo = model.VendorInvoiceNo,
                Vendor = vendor,
                VendorId = vendor.Id,
            };

            foreach (var line in model.PurchaseInvoiceLines)
            {
                //var item = _inventoryService.GetItemById(line.ItemId);
                //var measurement = _inventoryService.GetMeasurementById(line.UnitOfMeasurementId);
                purchInvoice.PurchaseInvoiceLines.Add(new PurchaseInvoiceLine()
                {
                    Item = line.Item,
                    //MeasurementId = measurement.Id,
                    Quantity = line.Quantity,
                    ReceivedQuantity = line.ReceivedQuantity,
                    Cost = line.Cost,
                    //Cost = item.Cost.Value,
                    Discount = 0,
                    Amount = line.Cost.Value * line.ReceivedQuantity,
                });
            }
            _purchasingService.AddPurchaseInvoice(purchInvoice, existingPO.Id);
            return RedirectToAction("PurchaseOrders");
        }
        //[Audit]
        //public ActionResult Vendors()
        //{
        //    var vendors = _purchasingService.GetVendors();
        //    var model = new Web.Models.ViewModels.Purchases.Vendors();
        //    if (vendors != null)
        //    {
        //        foreach (var vendor in vendors)
        //        {
        //            model.VendorsList.Add(new Models.ViewModels.Purchases.VendorsListLine()
        //            {
        //                Id = vendor.Id,
        //                Name = vendor.VendorName == null ? "Name" : vendor.VendorName,
        //                //Name = vendor.Party.Name == null ? "Name" : vendor.Party.Name,
        //                Balance = vendor.GetBalance()
        //            });
        //        }
        //    }
        //    return View(model);
        //}

        [Audit]
        public ActionResult Vendors()
        {
            var vendors = _purchasingService.GetVendors();
            return View(vendors);

        }

        public ActionResult AddVendor()
        {
            return View();
        }
        [Audit]
        [HttpPost]

        public ActionResult AddVendor(Vendor model)
        {
            _purchasingService.AddVendor(new Vendor()
            {
                VendorName = model.VendorName,
                VendorType = model.VendorType,
                VendorUserName = model.VendorUserName,
                Website = model.Website,
                Zip = model.Zip,
                AccountNo = model.AccountNo,
                City = model.City,
                ContactName = model.ContactName,
                Country = model.Country,
                MailingAddress = model.MailingAddress,
                IsActive = model.IsActive,
                BatchDeliveryMethod = model.BatchDeliveryMethod,
                Mobile = model.Mobile,
                ExpenseAccount = model.ExpenseAccount,
                Email = model.Email,
                No = model.No,
                PaymentAddress = model.PaymentAddress,
                PurchaseAccountId = model.PurchaseAccountId,
                PhoneNo = model.PhoneNo,
                PurchaseOrderAdd = model.PurchaseOrderAdd,
                PurchaseRep = model.PurchaseRep,
                ShipmentAddress = model.ShipmentAddress,
                State = model.State,
                TaxIdNum = model.TaxIdNum,
                TaxType = model.TaxType,

                ShipVia = model.ShipVia,

            });
            TempData["Msg"] = "Data has been saved succeessfully";
            return RedirectToAction("Vendors");
        }

        [Audit]
        [Authorize]
        public ViewResult GetVendor(int id)
        {
            Vendor vendor = _purchasingService.GetVendorById(id);
            return View(vendor);

        }
       
        public ActionResult EditVendor(int id)
        {
            return View(this._purchasingService.GetVendorById(id));

        }
        [Audit]
        [HttpPost]
        public ActionResult EditVendor(Vendor model)
        {
            var vendor = _purchasingService.GetVendorById(model.Id);
            vendor.TaxType = model.TaxType;
            vendor.TaxIdNum = model.TaxIdNum;
            vendor.State = model.State;
            vendor.ShipVia = model.ShipVia;
            vendor.ShipmentAddress = model.ShipmentAddress;
            vendor.PurchaseRep = model.PurchaseRep;
            vendor.PurchaseOrderAdd = model.PurchaseOrderAdd;
            vendor.PurchaseAccountId = model.PurchaseAccountId;
            vendor.PhoneNo = model.PhoneNo;
            vendor.Mobile = model.Mobile;
            vendor.MailingAddress = model.MailingAddress;
            vendor.IsActive = model.IsActive;
            vendor.PaymentAddress = model.PaymentAddress;
            vendor.PurchaseDiscountAccountId = model.PurchaseDiscountAccountId;
            vendor.VendorName = model.VendorName;
            vendor.VendorType = model.VendorType;
            vendor.VendorUserName = model.VendorUserName;
            vendor.Website = model.Website;
            vendor.Zip = model.Zip;
            
            return RedirectToAction("Vendors");
        }

        [Audit]
        public ActionResult DeleteVendor(int id)
        {
            var vendor = _purchasingService.GetVendorById(id);
         
            return View(vendor);
        }

        [HttpPost]
        public ActionResult DeleteVendor(int id, Vendor vendor)
        {
            var vendors = _purchasingService.GetVendorById(id);
            _purchasingService.DeleteVendor(vendors);
           
            return RedirectToAction("Vendors");

        }
        public ActionResult AddOrEditVendor(int id = 0)
        {
            Vendor vendor = null;
            var model = new Web.Models.ViewModels.Purchases.Vendor();
            model.Id = id;
            if (id != 0)
            {
                vendor = _purchasingService.GetVendorById(id);                
                model.VendorName = vendor.VendorName;
                model.AccountsPayableAccountId = vendor.AccountsPayableAccountId;
                model.PurchaseAccountId = vendor.PurchaseAccountId;
                model.PurchaseDiscountAccountId = vendor.PurchaseDiscountAccountId;
            }

            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddOrEditVendor")]
        [FormValueRequiredAttribute("SaveVendor")]
        public ActionResult AddOrEditVendor(Web.Models.ViewModels.Purchases.Vendor model)
        {
            Vendor vendor = null;
            if (model.Id != 0)
            {
                vendor = _purchasingService.GetVendorById(model.Id.Value);
            }
            else
            {
                vendor = new Vendor();
            }
            
            vendor.VendorName = model.VendorName;
            vendor.AccountsPayableAccountId = model.AccountsPayableAccountId.Value == -1 ? null : model.AccountsPayableAccountId;
            vendor.PurchaseAccountId = model.PurchaseAccountId.Value == -1 ? null : model.PurchaseAccountId;
            vendor.PurchaseDiscountAccountId = model.PurchaseDiscountAccountId.Value == -1 ? null : model.PurchaseDiscountAccountId;

            if (model.Id != 0)
                _purchasingService.UpdateVendor(vendor);
            else
                _purchasingService.AddVendor(vendor);

            return RedirectToAction("Vendors");
        }
        //public ActionResult AddVendor()
        //{
        //    return View();
        //}
        //[Audit]
        //[HttpPost, ActionName("AddVendor")]
        //public ActionResult AddVendor(Vendor model)
        //{
        //    _purchasingService.AddVendor(new Vendor()
        //    {
        //       MailingAddress = model .MailingAddress ,
        //       Mobile = model.Mobile,
        //       AccountNo = model.AccountNo,
        //       City = model.City,
        //       ContactName = model.ContactName,
        //       Country = model.Country,
        //       Email = model.Email,
        //       ExpenseAccount = model.ExpenseAccount,
        //       IsActive = model.IsActive,
        //       PaymentAddress = model.PaymentAddress,
        //       PaymentTerm = model.PaymentTerm,
        //       PhoneNo = model.PhoneNo,
        //       No = model.No,
        //       PurchaseAccountId = model.PurchaseAccountId,
        //       PurchaseOrderAdd = model.PurchaseOrderAdd,
        //       ShipVia = model.ShipVia,
        //       VendorName = model.VendorName,
        //       VendorUserName = model.VendorUserName,
        //       Zip = model.Zip,
        //       Website = model.Website,
        //       VendorType = model.VendorType,
        //       TaxType = model.TaxType,
        //       State = model.State,
        //       ShipmentAddress= model.ShipmentAddress,
        //       PurchaseRep = model.PurchaseRep,
        //       TaxIdNum =model.TaxIdNum,
        //       AccountsPayableAccountId = model.AccountsPayableAccountId,
        //       PaymentTermId = model.PaymentTermId
        //    });

        //    return RedirectToAction("Vendors");
        //}

        //[Audit]
        //public ActionResult Vendors(Vendor model)
        //{
        //    var viewVendor = _purchasingService.GetVendors();

        //    return View(viewVendor);

        //}

        //public ActionResult EditVendor(int id)
        //{
        //    return View(this._purchasingService.GetVendorById(id));
        //}
        //[Audit]
        //[HttpPost, ActionName("EditAccountClass")]

        //public ActionResult EditVendor(Vendor model)
        //{

        //    var vendor = _purchasingService.GetVendorById(model.Id);
        //    vendor.MailingAddress = model.MailingAddress;
        //      vendor. Mobile = model.Mobile;
        //       vendor.AccountNo = model.AccountNo;
        //       vendor.City = model.City;
        //       vendor.ContactName = model.ContactName;
        //       vendor.Country = model.Country;
        //       vendor.Email = model.Email;
        //       vendor.ExpenseAccount = model.ExpenseAccount;
        //       vendor.IsActive = model.IsActive;
        //       vendor.PaymentAddress = model.PaymentAddress;
        //       vendor.PaymentTerm = model.PaymentTerm;
        //       vendor.PhoneNo = model.PhoneNo;
        //       vendor.No = model.No;
        //       vendor.PurchaseAccountId = model.PurchaseAccountId;
        //       vendor.PurchaseOrderAdd = model.PurchaseOrderAdd;
        //       vendor.ShipVia = model.ShipVia;
        //       vendor.VendorName = model.VendorName;
        //       vendor.VendorUserName = model.VendorUserName;
        //       vendor.Zip = model.Zip;
        //       vendor.Website = model.Website;
        //       vendor.VendorType = model.VendorType;
        //       vendor.TaxType = model.TaxType;
        //       vendor.State = model.State;
        //       vendor.ShipmentAddress= model.ShipmentAddress;
        //       vendor.PurchaseRep = model.PurchaseRep;
        //       vendor.TaxIdNum =model.TaxIdNum;
        //       vendor.AccountsPayableAccountId = model.AccountsPayableAccountId;
        //       vendor.PaymentTermId = model.PaymentTermId;

        //    _purchasingService.UpdateVendor(vendor);
            
        //    return RedirectToAction("Vendors");
        //}

        //[Audit]
        //[Authorize]
        //public ViewResult GetVendor(int id)
        //{
        //    Vendor vendor = _purchasingService.GetVendorById(id);
            
        //    return View(vendor);

        //}
        //[Audit]
        //public ActionResult DeleteVendor(int id)
        //{
        //    var vendor = _purchasingService.GetVendorById(id);

        //    return View(vendor);
        //}

        //[HttpPost]
        //public ActionResult DeleteVendor(int id, Vendor vendor)
        //{
        //    var vendors = _purchasingService.GetVendorById(id);
        //    _purchasingService.DeleteVendor(vendors);
            
        //    return RedirectToAction("Vendors");

        //}
        //public ActionResult AddVendor()
        //{
        //    var model = new Models.ViewModels.Purchases.AddVendor();
        //    model.Accounts = Models.ModelViewHelper.Accounts();
        //    return View(model);
        //}
        //[Audit]
        //[HttpPost, ActionName("AddVendor")]
        //[FormValueRequiredAttribute("SaveVendor")]
        //public ActionResult AddVendor(Models.ViewModels.Purchases.AddVendor model)
        //{
        //    var vendor = new Vendor()
        //    {
        //        AccountsPayableAccountId = model.AccountsPayableAccountId.Value == -1 ? null : model.AccountsPayableAccountId,
        //        PurchaseAccountId = model.PurchaseAccountId.Value == -1 ? null : model.PurchaseAccountId,
        //        PurchaseDiscountAccountId = model.PurchaseDiscountAccountId.Value == -1 ? null : model.PurchaseDiscountAccountId,
        //    };
        //    vendor.VendorName = model.VendorName;

        //    _purchasingService.AddVendor(vendor);

        //    return RedirectToAction("Vendors");
        //}
        [Audit]
        public ActionResult PurchaseInvoices()
        {
            var invoices = _purchasingService.GetPurchaseInvoices();
            var model = new Models.ViewModels.Purchases.PurchaseInvoices();
                        
            foreach(var invoice in invoices)
            {
                var invoiceModel = new Models.ViewModels.Purchases.PurchaseInvoiceListLine()
                {
                    Id = invoice.Id,
                    No = invoice.CustInvoiceNo,
                    Date = invoice.InvoiceDate,
                    Vendor = invoice.Vendor.VendorName,
                    TotalAmount = invoice.PurchaseInvoiceLines.Sum(a => a.Amount),
                    //IsPaid = invoice.IsPaid(),
                    //TotalTax = _taxService.GetPurchaseTaxes(invoice.VendorId.Value, invoice.PurchaseInvoiceLines.AsEnumerable()).Sum(t => t.Value)
                };

                model.PurchaseInvoiceListLines.Add(invoiceModel);
            }
            return View(model);
        }

        public ActionResult MakePayment(int id)
        {
            var model = new Models.ViewModels.Purchases.MakePayment();
            var invoice = _purchasingService.GetPurchaseInvoiceById(id);

            model.InvoiceId = invoice.Id;
            model.InvoiceNo = invoice.CustInvoiceNo;
            model.Vendor = invoice.Vendor.VendorName;
            //model.Amount = invoice.GeneralLedgerHeader.GeneralLedgerLines.Where(dr => dr.DrCr == Core.Domain.DrOrCrSide.Dr).Sum(l => l.Amount);

            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("MakePayment")]
        [FormValueRequiredAttribute("SavePaymentToVendor")]
        public ActionResult SavePaymentToVendor(Models.ViewModels.Purchases.MakePayment model)
        {
            var invoice = _purchasingService.GetPurchaseInvoiceById(model.InvoiceId);
            if(model.AmountToPay < 1)
                return RedirectToAction("MakePayment", new { id = model.InvoiceId });
            _purchasingService.SavePayment(invoice.Id, invoice.VendorId.Value, model.AccountId, model.AmountToPay, DateTime.Now);
            return RedirectToAction("PurchaseInvoices");
        }
        [Audit]
        public ActionResult PurchaseOrder(int id = 0)
        {
            var model = new Models.ViewModels.Purchases.PurchaseHeaderViewModel();
            model.DocumentType = Core.Domain.DocumentTypes.PurchaseOrder;
            if (id == 0)
            {
                model.Id = id;
                return View(model);
            }
            else
            {
                var order = _purchasingService.GetPurchaseOrderById(id);
                model.Id = order.Id;
                model.VendorId = order.VendorId;
                model.Date = order.Date;
                model.ReferenceNo = string.Empty;
                foreach (var line in order.PurchaseOrderLines)
                {
                    var lineItem = new Models.ViewModels.Purchases.PurchaseLineItemViewModel();
                    lineItem.Id = line.Id;
                    lineItem.Item = line.Item;
                    //lineItem.ItemNo = line.Item.SerialNo;
                    lineItem.ItemDescription = line.Description;
                    //lineItem.Measurement = line.Measurement.Description;
                    lineItem.Quantity = line.Quantity;
                    lineItem.Price = line.Amount;
                    model.PurchaseLine.PurchaseLineItems.Add(lineItem);
                }
                return View(model);
            }
        }
        [Audit]
        public ActionResult PurchaseDelivery(int id = 0, int orderid = 0)
        {
            var model = new Models.ViewModels.Purchases.PurchaseHeaderViewModel();
            model.DocumentType = Core.Domain.DocumentTypes.PurchaseReceipt;
            if (id == 0)
            {
                model.Id = id;
                if (orderid != 0)
                {
                    var order = _purchasingService.GetPurchaseOrderById(orderid);
                    model.Id = order.Id;
                    model.VendorId = order.VendorId;
                    model.Date = order.Date;
                    model.ReferenceNo = string.Empty;
                    foreach (var line in order.PurchaseOrderLines)
                    {
                        var lineItem = new Models.ViewModels.Purchases.PurchaseLineItemViewModel();
                        lineItem.Id = line.Id;
                        lineItem.Item = line.Item;
                       // lineItem.ItemNo = line.Item.SerialNo;
                        lineItem.ItemDescription = line.Description;
                        //lineItem.Measurement = line.Measurement.Description;
                        lineItem.Quantity = line.Quantity;
                        lineItem.Price = line.Amount;
                        model.PurchaseLine.PurchaseLineItems.Add(lineItem);
                    }
                }
                return View(model);
            }
            else
            {
                var delivery = _purchasingService.GetPurchaseReceiptById(id);
                model.Id = delivery.Id;
                model.VendorId = delivery.VendorId;
                model.Date = delivery.Date;
                model.ReferenceNo = string.Empty;
                foreach (var line in delivery.PurchaseReceiptLines)
                {
                    var lineItem = new Models.ViewModels.Purchases.PurchaseLineItemViewModel();
                    lineItem.Id = line.Id;
                    lineItem.Item = line.Item;
                    //lineItem.ItemNo = line.Item.No;
                    lineItem.ItemDescription = line.Description;
                    //lineItem.Measurement = line.Measurement.Description;
                    lineItem.Quantity = line.Quantity;
                    lineItem.Price = line.Amount;
                    model.PurchaseLine.PurchaseLineItems.Add(lineItem);
                }
                return View(model);
            }
        }
        [Audit]
        public ActionResult PurchaseInvoice(int id = 0, int deliveryId = 0)
        {
            var model = new Models.ViewModels.Purchases.PurchaseHeaderViewModel();
            model.DocumentType = Core.Domain.DocumentTypes.PurchaseInvoice;
            model.IsDirect = deliveryId == 0;
            model.Id = id;
            if (id == 0)
            {
                return View(model);
            }
            else
            {
                var invoice = _purchasingService.GetPurchaseInvoiceById(id);
                model.Id = invoice.Id;
                model.VendorId = invoice.VendorId;
                model.Date = invoice.InvoiceDate;
                model.ReferenceNo = string.Empty;
                foreach (var line in invoice.PurchaseInvoiceLines)
                {
                    var lineItem = new Models.ViewModels.Purchases.PurchaseLineItemViewModel();
                    lineItem.Id = line.Id;
                    lineItem.Item = line.Item;
                    //lineItem.ItemNo = line.Item.No;
                    lineItem.ItemDescription = line.Description;
                    //lineItem.Measurement = line.Measurement.Description;
                    lineItem.Quantity = line.Quantity;
                    lineItem.Price = line.Amount;
                    model.PurchaseLine.PurchaseLineItems.Add(lineItem);
                }
                return View(model);
            }
        }

        [NonAction]
        protected void AddLineItem(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            //var item = _inventoryService.GetItemByNo(model.PurchaseLine.ItemNo);
            var newLine = new Models.ViewModels.Purchases.PurchaseLineItemViewModel()
            {
                
                //ItemNo = item.No,
                ItemDescription = model.PurchaseLine.ItemDescription,
                //Measurement = item.SellMeasurement.Description,
                Quantity = model.PurchaseLine.Quantity,
                Price = model.PurchaseLine.Price,
                Item = model.PurchaseLine.Item,
            };
            model.PurchaseLine.PurchaseLineItems.Add(newLine);

            foreach (var line in model.PurchaseLine.PurchaseLineItems)
            {
             //   var taxes = _financialService.ComputeInputTax(model.VendorId.Value, line.Item, line.Quantity, line.Price, decimal.Zero);
                var taxVM = new Models.ViewModels.Purchases.PurchaseLineItemTaxViewModel();
                //foreach (var tax in taxes)
                //{
                //    var t = _financialService.GetTaxes().Where(tx => tx.Id == int.Parse(tax.Key.ToString())).FirstOrDefault();
                //    taxVM.TaxId = int.Parse(tax.Key.ToString());
                //    taxVM.Amount = tax.Value;
                //    taxVM.TaxRate = t.Rate;
                //    taxVM.TaxName = t.TaxName;
                //    model.PurchaseLine.PurchaseLineItemsTaxes.Add(taxVM);
                //}
            }
        }

        public ActionResult ReturnView(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            string actionName = string.Empty;
            switch (model.DocumentType)
            {
                case Core.Domain.DocumentTypes.SalesOrder:
                    actionName = "PurchaseOrder";
                    break;
                case Core.Domain.DocumentTypes.SalesInvoice:
                    actionName = "Purchasevoice";
                    break;
                case Core.Domain.DocumentTypes.SalesDelivery:
                    actionName = "PurchaseDelivery";
                    break;
            }

            return View(actionName, model);
        }
        [Audit]
        [HttpPost, ActionName("PurchaseOrder")]
        [FormValueRequiredAttribute("AddLineItem")]
        public ActionResult AddLineItemOrder(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            AddLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("PurchaseInvoice")]
        [FormValueRequiredAttribute("AddLineItem")]
        public ActionResult AddLineItemInvoice(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            AddLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("PurchaseDelivery")]
        [FormValueRequiredAttribute("AddLineItem")]
        public ActionResult AddLineItemDelivery(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            AddLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("PurchaseOrder")]
        [FormValueRequiredAttribute("DeleteLineItem")]
        public ActionResult DeleteLineItemOrder(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            DeleteLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("PurchaseInvoice")]
        [FormValueRequiredAttribute("DeleteLineItem")]
        public ActionResult DeleteLineItemInvoice(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            DeleteLineItem(model);
            return ReturnView(model);
        }
        [Audit]
        [HttpPost, ActionName("PurchaseDelivery")]
        [FormValueRequiredAttribute("DeleteLineItem")]
        public ActionResult DeleteLineItemDelivery(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            DeleteLineItem(model);
            return ReturnView(model);
        }

        [NonAction]
        protected void DeleteLineItem(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            if (!string.IsNullOrEmpty(deletedItem))
            {
                model.PurchaseLine.PurchaseLineItems.RemoveAt(int.Parse(deletedItem));
            }
        }
        //[Audit]
        //[HttpPost, ActionName("PurchaseOrder")]
        //[FormValueRequiredAttribute("Save")]
        //public ActionResult SaveOrder(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        //{
        //    PurchaseOrderHeader order = null;
        //    if (model.Id.HasValue == false || model.Id == 0)
        //    {
        //        order = new PurchaseOrderHeader();
        //    }
        //    else
        //    {
        //        order = _purchasingService.GetPurchaseOrderById(model.Id.Value);
        //    }
        //    order.VendorId = model.VendorId.Value;
        //    order.Date = model.Date;
        //    order.Description = string.Empty;

        //    foreach (var line in model.PurchaseLine.PurchaseLineItems)
        //    {
        //        var item = _inventoryService.GetItemById(line.ItemId);
        //        order.PurchaseOrderLines.Add(new PurchaseOrderLine()
        //        {
        //            Amount = line.Price,
        //            ItemId = item.Id,
        //            Quantity = line.Quantity,
        //            MeasurementId = item.PurchaseMeasurementId.Value,
        //            Cost = item.Cost.Value,
        //        });
        //    }

        //    _purchasingService.AddPurchaseOrder(order, true);
        //    return RedirectToAction("PurchaseOrders");
        //}
        [Audit]
        [HttpPost, ActionName("PurchaseDelivery")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult SaveDelivery(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            if (model.Id == 0)
            {
            }
            else
            {
            }
            return RedirectToAction("PurchaseDeliveries");
        }
        [Audit]
        [HttpPost, ActionName("PurchaseInvoice")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult SaveInvoice(Models.ViewModels.Purchases.PurchaseHeaderViewModel model)
        {
            if (model.Id == 0)
            {
                var invoice = new PurchaseInvoiceHeader();
                invoice.InvoiceDate = model.Date;
                invoice.Description = string.Empty;
                invoice.VendorId = model.VendorId;
                invoice.VendorInvoiceNo = model.ReferenceNo;
            }
            else
            {

            }
            return RedirectToAction("PurchaseInvoices");
        }
    }
}

using Core.Domain.Items;
using Services.Financial;
using Services.Inventory;
using Services.Purchasing;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
 //   [Authorize(Roles ="SuperAdmin, Cashier, Accountant, account Manager, Store Keeper, Store Manager, Inventory Manager")]
   [Authorize]
    public class InventoryController : BaseController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IFinancialService _financialService;
        private readonly IPurchasingService _purchasingService;

        public InventoryController(IInventoryService inventoryService,
            IFinancialService financialService,
            IPurchasingService purchasingService)
        {
            _inventoryService = inventoryService;
            _financialService = financialService;
            _purchasingService = purchasingService;
        }
        [Audit]
        public ActionResult Items()
        {
            var items = _inventoryService.GetAllItems();
            var model = new Models.ViewModels.Items.Items();
            foreach(var item in items)
            {
                model.ItemsList.Add(new Models.ViewModels.Items.Items.ItemListLine
                {
                    ItemId = item.Id,
                    No = item.No,
                    Description = item.Description,
                    QtyOnHand = item.ComputeQuantityOnHand()
                });
            }
            return View(model);
        }
        [Audit]
        [Authorize]
        public ViewResult CategoryDetail(int id)
        {
            Category category = _inventoryService.GetCategoryById(id);

            return View(category);

        }
        [Audit]
        public ActionResult ViewCategory()
        {
            var category = _inventoryService.GetAllCategories();
            return View(category);
            
        }
        public ActionResult AddCategory()
        {
            return View();
        }
        [Audit]
        [HttpPost]

        public ActionResult AddCategory(Category model)
        {
            _inventoryService.AddCategory(new Category()
            
            {
                CategoryName = model.CategoryName
            });
            return RedirectToAction("ViewCategory");
        }

        public ActionResult EditCategory(int id)
        {
            return View(this._inventoryService.GetCategoryById(id));
           
        }
        [Audit]
        [HttpPost]
        
        public ActionResult EditCategory(Category model)
        {
            var category = _inventoryService.GetCategoryById(model.Id);
            category.CategoryName = model.CategoryName;
            _inventoryService.UpdateCategory(category);
           
            return RedirectToAction("ViewCategory");
        }

        [Audit]
        public ActionResult DeleteCategory(int id)
        {
            var category = _inventoryService.GetCategoryById(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id, Category category)
        {
            var Category = _inventoryService.GetCategoryById(id);
            _inventoryService.DeleteCategory(Category);
            return RedirectToAction("ViewCategory");

        }


        [Authorize]
       [Audit]
        public ViewResult ReorderDetail(int id)
        {
            ReoderingRule reorder = _inventoryService.GetRuleById(id);

            return View(reorder);

        }
        [Audit]
        public ActionResult ViewReorder()
        {
            var reoder = _inventoryService.GetAllRules();
            return View(reoder);

        }
        public ActionResult AddReorder()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddReorder(ReoderingRule model)
        {
            _inventoryService.AddRules(new ReoderingRule()

            {
                MaximumQty = model.MaximumQty,
                MinimumQty = model.MinimumQty,
                InventoryId = model.InventoryId
               
            });
            return RedirectToAction("ViewReorder");
        }

        public ActionResult EditReorder(int id)
        {
            return View(this._inventoryService.GetRuleById(id));

        }
        [Audit]
        [HttpPost]

        public ActionResult EditReorder(ReoderingRule model)
        {
            var reoder = _inventoryService.GetRuleById(model.Id);
            reoder.MaximumQty = model.MaximumQty;
            reoder.MinimumQty = model.MinimumQty;
            
            _inventoryService.UpdateRule(reoder);

            return RedirectToAction("ViewReorder");
        }

        [Audit]
        public ActionResult DeleteReorder(int id)
        {
            var rule = _inventoryService.GetRuleById(id);
            return View(rule);
        }

        [HttpPost]
        public ActionResult DeleteReorder(int id, ReoderingRule reoder)
        {
            var rule = _inventoryService.GetRuleById(id);
            _inventoryService.DeleteRules(rule);
            return RedirectToAction("ViewReorder");

        }


        [Audit]
        [Authorize]
        public ViewResult BranchDetail(int id)
        {
            Branch branch = _inventoryService.GetBranchById(id);

            return View(branch);

        }
        [Audit]
        public ActionResult ViewBranch()
        {
            var branch = _inventoryService.GetAllBranches();
            return View(branch);

        }
        public ActionResult AddBranch()
        {
            return View();
        }

        [Audit]
        [HttpPost]
        public ActionResult AddBranch(Branch model)
        {
            _inventoryService.AddBranch(new Branch()

            {
                BranchName = model .BranchName,
                StreetAddress = model.StreetAddress,
                LocationId = model.LocationId
                
            });
            return RedirectToAction("ViewBranch");
        }

        public ActionResult EditBranch(int id)
        {
            return View(this._inventoryService.GetBranchById(id));

        }
        [Audit]
        [HttpPost]
        public ActionResult EditBranch(Branch model)
        {
            var branch = _inventoryService.GetBranchById(model.Id);
            branch.LocationId = model.LocationId;
            branch.BranchName = model.BranchName;
            branch.StreetAddress = model.StreetAddress;
            
            _inventoryService.UpdateBranch(branch);

            return RedirectToAction("ViewBranch");
        }
        [Audit]
        public ActionResult DeleteBranch(int id)
        {
            var branch =_inventoryService.GetBranchById(id);
            return View(branch);
        }

       [HttpPost]
       public ActionResult DeleteBranch(int id, Branch branch)
       {
           var Branch = _inventoryService.GetBranchById(id);
           _inventoryService.DeleteBranch(Branch);
           return RedirectToAction("ViewBranch");
           
       }

        [Audit]
        public ActionResult ViewLocation()
        {
            var location = _inventoryService.GetAllLocations();
            return View(location);
           
        }
        public ActionResult AddLocation()
        {
            return View();
        }
        [Audit]
        [HttpPost]

        public ActionResult AddLocation(Location model)
        {
            _inventoryService.AddLocation(new Location()

            {
                Country = model.Country,
                City = model .City,
                State = model .State ,
                LocationName = model.LocationName,
              
                Zip = model .Zip
               
            });
            return RedirectToAction("ViewLocation");
        }
        [Authorize]
        public ViewResult LocationDetail(int id)
        {
            Location location = _inventoryService.GetLocationById(id);
            
            return View(location);

        }
        public ActionResult EditLocation(int id)
        {
            return View(this._inventoryService.GetLocationById(id));

        }
        [Audit]
        [HttpPost]

        public ActionResult EditLocation(Location model)
        {
            var location = _inventoryService.GetLocationById(model.Id);
            location.Country = model.Country;
            location.City = model.City;
            location.State = model.State;
            location.Zip = model.Zip;
            location.LocationName = model.LocationName;
            return RedirectToAction("ViewLocation");
        }

        [Audit]
        public ActionResult DeleteLocation(int id)
        {
            var location = _inventoryService.GetLocationById(id);
            return View(location);
        }

        [HttpPost]
        public ActionResult DeleteLocation(int id, Location location)
        {
            var Locations = _inventoryService.GetLocationById(id);
            _inventoryService.DeleteLocation(Locations);
            return RedirectToAction("ViewLocation");

        }


        [Authorize]
        public ViewResult ManufacturerDetail(int id)
        {
            Manufacturer manufacture = _inventoryService.GetManufacturerById(id);

            return View(manufacture);

        }
        [Audit]
        public ActionResult ViewManufacturer()
        {
            var manufacturer = _inventoryService.Manufacturers();
            return View(manufacturer);

        }
        public ActionResult AddManufacturer()
        {
            return View();
        }
        [Audit]
        [HttpPost]

        public ActionResult AddManufacturer(Manufacturer model)
        {
            _inventoryService.AddManufacturer(new Manufacturer()

            {
                ManufacturerName = model.ManufacturerName
                
            });
            return RedirectToAction("ViewManufacturer");
        }

        public ActionResult EditManufacturer(int id)
        {
            return View(this._inventoryService.GetManufacturerById(id));

        }
        [Audit]
        [HttpPost]

        public ActionResult EditManufacturer(Manufacturer model)
        {
            var manufacturer = _inventoryService.GetManufacturerById(model.Id);
            manufacturer.ManufacturerName = model.ManufacturerName;
       
            _inventoryService.UpdateManufacturer(manufacturer);

            return RedirectToAction("ViewManufacturer");
        }

        [Audit]
        public ActionResult DeleteManufacturer(int id)
        {
            var manufacturer = _inventoryService.GetManufacturerById(id);
            return View(manufacturer);
        }

        [HttpPost]
        public ActionResult DeleteManufacturer(int id, Manufacturer manufacturer)
        {
            var manufacture = _inventoryService.GetManufacturerById(id);
            _inventoryService.DeleteManufacturer(manufacture);
            return RedirectToAction("ViewManufacturer");

        }


        [Audit]
        public ActionResult ViewInventory()
        {
            var inventory = _inventoryService.GetAllInventories();
            return View(inventory);

        }
        [Authorize]
        public ViewResult InventoryDetail(int id)
        {
            InventoryCatalog inventory = _inventoryService.GetInventoryById(id);

            return View(inventory);

        }
        public ActionResult AddInventory()
        {
            return View();
        }
        [Audit]
        [HttpPost]

        public ActionResult AddInventory(InventoryCatalog model)
        {
            _inventoryService.AddInventory(new InventoryCatalog()

            {
                ItemName = model.ItemName,
                Label = model.Label,
                VendorId = model.VendorId,
                //MinimumCount = model.MinimumCount,
                Quantity = model.Quantity,
                SerialNo = model.SerialNo,
                Description = model.Description,
                TransactionDate = model.TransactionDate,
                SKU = model.SKU,
                TransactionEntry = model.TransactionEntry,
                UnitCost = model.UnitCost,
                UPC = model.UPC,
                Barcode = model.Barcode
               
            });
            return RedirectToAction("ViewInventory");
        }

        public ActionResult EditInventory(int id)
        {
            return View(this._inventoryService.GetInventoryById(id));

        }
        [Audit]
        [HttpPost]

        public ActionResult EditInventory(InventoryCatalog model)
        {
            var inventory = _inventoryService.GetInventoryById(model.Id);
            inventory.ItemName = model.ItemName;
            inventory.Label = model.Label;
            
            inventory.VendorId = model.VendorId;
            inventory.Quantity = model.Quantity;
            inventory.SerialNo = model.SerialNo;
            inventory.SKU = model.SKU;
            inventory.Description = model.Description;
            inventory.TransactionDate = model.TransactionDate;
            inventory.TransactionEntry = model.TransactionEntry;
            inventory.UnitCost = model.UnitCost;
            inventory.UPC = model.UPC;
            inventory.Barcode = model.Barcode;
            _inventoryService.UpdateInventory(inventory);

            return RedirectToAction("ViewInventory");
        }

        [Audit]
        public ActionResult DeleteInventory(int id)
        {
            var inventory = _inventoryService.GetInventoryById(id);
            return View(inventory);
        }

        [HttpPost]
        public ActionResult DeleteInventory(int id, InventoryCatalog inventory)
        {
            var inventories = _inventoryService.GetInventoryById(id);
            _inventoryService.DeleteInventory(inventories);
            return RedirectToAction("ViewInventory");

        }

        public ActionResult AddItemCategory()
        {
            return View();
        }
        [Audit]
        [HttpPost]

        public ActionResult AddItemCategory(ItemCategory model)
        {
            _inventoryService.AddItemCategory(new ItemCategory()
            {
                Code = model.Code,
                Name = model.Name
            });
            return RedirectToAction("Items");
        }
        public ActionResult AddItem()
        {
            var model = new Models.ViewModels.Items.Items.AddItem();
            return View(model);
        }

       
        [Audit]
        [HttpPost, ActionName("AddItem")]
        [FormValueRequiredAttribute("SaveItem")]
        public ActionResult AddItem(Models.ViewModels.Items.Items.AddItem model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Description))
                    throw new Exception("Description cannot be empty.");

                _inventoryService.AddItem(new Core.Domain.Items.Item() {
                    Code = model.Code,
                    Description = model.Description,
                    SellDescription = model .SellDescription,
                    PurchaseCost = model.PurchaseCost,
                    PurchaseDescription = model.PurchaseDescription,
                    Name = model.Name,
                    Price = model.SellPrice,
                    SalesAccountId = model.SalesAccountId,
                    InventoryAccountId = model.InventoryAccountId,
                    ItemCategoryId = model.ItemCategoryId,
                    InventoryAdjustmentAccountId = model.InventoryAdjustmentAccountId,
                    CostOfGoodsSoldAccountId = model.CostOfGoodsSoldAccountId
                });

                return RedirectToAction("Items");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Inventory/Edit/5
        public ActionResult EditItem(int id)
        {
            var item = _inventoryService.GetItemById(id);
            var accounts = _financialService.GetAccounts();
            var measurements = _inventoryService.GetMeasurements();
            //var taxes = _financialService.GetTaxes();
            var itemCategories = _inventoryService.GetItemCategories();
            var vendors = _purchasingService.GetVendors();
            //var itemTaxGroups = _financialService.GetItemTaxGroups();

            var model = new Models.ViewModels.Items.EditItem();
            model.PrepareEditItemViewModel(item);
            model.Accounts = ModelViewHelper.Accounts();
            model.UnitOfMeasurements = ModelViewHelper.Measurements();
            model.Taxes = ModelViewHelper.Taxes();
            model.ItemTaxGroups = ModelViewHelper.ItemTaxGroups();
            model.Vendors = ModelViewHelper.Vendors();
            model.ItemCategories = ModelViewHelper.ItemCategories();
            model.ItemTaxGroupId = model.ItemTaxGroupId == null ? -1 : model.ItemTaxGroupId;
            model.InventoryAccountId = model.InventoryAccountId == null ? -1 : model.InventoryAccountId;
            model.SellAccountId = model.SellAccountId == null ? -1 : model.SellAccountId;
            model.InventoryAdjustmentAccountId = model.InventoryAdjustmentAccountId == null ? -1 : model.InventoryAdjustmentAccountId;
            model.PurchaseMeasurementId = model.PurchaseMeasurementId == null ? -1 : model.PurchaseMeasurementId;
            model.CostOfGoodsSoldAccountId = model.CostOfGoodsSoldAccountId == null ? -1 : model.CostOfGoodsSoldAccountId;

            return View(model);
        }

        // POST: Inventory/Edit/5
        [Audit]
        [HttpPost, ActionName("EditItem")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult EditItem(Models.ViewModels.Items.EditItem model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Description))
                    throw new Exception("Item description cannot be empty.");

                var item = _inventoryService.GetItemById(model.Id);
                item.Id = model.Id;
                item.SmallestMeasurementId = model.SmallestMeasurementId;
                //item.InventoryId = model.InventoryId;
                item.ItemTaxGroupId = model.ItemTaxGroupId;
                item.PreferredVendorId = model.PreferredVendorId;
                item.No = model.No;
                item.Name = model.Name;
                item.Code = model.Code;
                item.Description = model.Description;
                item.PurchaseDescription = model.PurchaseDescription;
                item.SellDescription = model.SellDescription;
                item.Cost = model.Cost;
                item.Price = model.Price;
                item.ItemTaxGroupId = model.ItemTaxGroupId == -1 ? null : model.ItemTaxGroupId;
                item.SalesAccountId = model.SellAccountId == -1 ? null : model.SellAccountId;
                item.InventoryAdjustmentAccountId = model.InventoryAdjustmentAccountId == -1 ? null : model.InventoryAdjustmentAccountId;
                item.InventoryAccountId = model.InventoryAccountId == -1 ? null : model.InventoryAccountId;
                item.SellMeasurementId = model.SellMeasurementId == -1 ? null : model.SellMeasurementId;
                item.PurchaseMeasurementId = model.PurchaseMeasurementId == -1 ? null : model.PurchaseMeasurementId;
                item.CostOfGoodsSoldAccountId = model.CostOfGoodsSoldAccountId == -1 ? null : model.CostOfGoodsSoldAccountId;
                _inventoryService.UpdateItem(item);
                return RedirectToAction("Items");
            }
            catch
            {
                return View();
            }
        }
        [Audit]
        [HttpGet]
        public JsonResult GetItemByNo(string itemNo)
        {
            var item = _inventoryService.GetItemByNo(itemNo);
            var data = new { Price = item.Price, Cost = item.Cost, Measurement = item.SellMeasurement.Description };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [Audit]
        public ActionResult InventoryControlJournal()
        {
            var invControlJournals = _inventoryService.GetInventoryControlJournals();
            var model = new List<Models.ViewModels.Items.InventoryControlJournal>();
            foreach (var icj in invControlJournals)
            {
                model.Add(new Models.ViewModels.Items.InventoryControlJournal()
                {
                    In = icj.INQty,
                    Out = icj.OUTQty,
                    Inventory = icj.InventoryCatalog.Description,
                    Item = icj.Item.Description,
                    Measurement = icj.Measurement.Code,
                    Date = icj.Date
                });
            }
            return View(model);
        }
    }
}

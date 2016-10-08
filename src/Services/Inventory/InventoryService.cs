using Core.Data;
using Core.Domain;
using Core.Domain.Items;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Core.Domain.Financials;
//using Core.Domain.TaxSystem;

namespace Services.Inventory
{
    public partial class InventoryService :  IInventoryService
    {
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Location> _locationRepo;
        private readonly IRepository<Manufacturer> _manufacturerRepo;
        private readonly IRepository<InventoryCatalog> _inventoryRepo;
        //private readonly IRepository<ItemCategory> _itemCategoryRepo;
        private readonly IRepository<InventoryControlJournal> _icjRepo;
        private readonly IRepository<Measurement> _measurementRepo;
        private readonly IRepository<ItemCategory> _itemCategoryRepo;
        private readonly IRepository<SequenceNumber> _sequenceNumberRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<ReoderingRule> _reorderRepo;
        private readonly IRepository<Branch> _branchRepo;
        //private readonly IRepository<ItemTaxGroup> _itemTaxGroup;

        public InventoryService(IRepository<Item> itemRepo,
            //IRepository<ItemCategory> itemCategoryRepo,
            IRepository<Measurement> measurementRepo, 
            IRepository<InventoryControlJournal> icjRepo,
            IRepository<ItemCategory> itemCategoryRepo,
            IRepository<SequenceNumber> sequenceNumberRepo,
            IRepository<Bank> bankRepo,
            IRepository<Account> accountRepo,
            IRepository<Category> categoryRepo,
            IRepository<Manufacturer> manufacturerRepo,
            IRepository<Location> locationRepo,
            IRepository<InventoryCatalog> inventoryRepo,
            IRepository<ReoderingRule> reoderRepo,
            IRepository<Branch>  branchRepo
           // IRepository<ItemTaxGroup> itemTaxGroup
            )
        {
            _branchRepo = branchRepo;
            _reorderRepo = reoderRepo;
            _itemRepo = itemRepo;
            _measurementRepo = measurementRepo;
            _icjRepo = icjRepo;
            _itemCategoryRepo = itemCategoryRepo;
            _sequenceNumberRepo = sequenceNumberRepo;
            _bankRepo = bankRepo;
            _accountRepo = accountRepo;
            _categoryRepo = categoryRepo;
            _locationRepo = locationRepo;
            _manufacturerRepo = manufacturerRepo;
            _inventoryRepo = inventoryRepo;
            //_itemTaxGroup = itemTaxGroup;
        }

        public InventoryControlJournal CreateInventoryControlJournal(int inventoryCatalagId,  DocumentTypes documentType, decimal? inQty, decimal? outQty, decimal? totalCost, decimal? totalAmount)
        {
            if (!inQty.HasValue && !outQty.HasValue)
                throw new MissingFieldException();

            var icj = new InventoryControlJournal()
            {
                InventoryCatalogId = inventoryCatalagId,
               
                DocumentType = documentType,
                Date = DateTime.Now,
                INQty = inQty,
                OUTQty = outQty,
                TotalCost = totalCost,
                TotalAmount = totalAmount,
            };
            return icj;
        }

        public void AddItem(Item item)
        {
            //item.No = GetNextNumber(SequenceNumberTypes.Item).ToString();

            //var sales = _accountRepo.Table.Where(a => a.AccountCode == "40100").FirstOrDefault();
            //var inventory = _accountRepo.Table.Where(a => a.AccountCode == "10800").FirstOrDefault();
            //var invAdjusment = _accountRepo.Table.Where(a => a.AccountCode == "50500").FirstOrDefault();
            //var cogs = _accountRepo.Table.Where(a => a.AccountCode == "50300").FirstOrDefault();
            //var assemblyCost = _accountRepo.Table.Where(a => a.AccountCode == "10900").FirstOrDefault();

            //item.SalesAccount = sales;
            //item.InventoryAccount = inventory;
            //item.CostOfGoodsSoldAccount = cogs;
            //item.InventoryAdjustmentAccount = invAdjusment;

            //item.ItemTaxGroup = _itemTaxGroup.Table.Where(m => m.Name == "Regular").FirstOrDefault();
            
            _itemRepo.Insert(item);
        }
        public void AddItemCategory(ItemCategory category)
        {
            _itemCategoryRepo.Insert(category);
        }
        public void UpdateItem(Item item)
        {
            _itemRepo.Update(item);
        }

        public void DeleteItem(Item item)
        {
            _itemRepo.Delete(item);
        }

        public IEnumerable<Item> GetAllItems()
        {
            var query = from item in _itemRepo.Table select item;
            var items = query.ToList();
            return items;
        }

        public Item GetItemById(int id)
        {
            var query = from item in _itemRepo.Table
                        where item.Id == id
                        select item;
            return query.FirstOrDefault();
        }

        public IEnumerable<Measurement> GetMeasurements()
        {
            var query = from f in _measurementRepo.Table
                        select f;
            return query.AsEnumerable();
        }

        public Measurement GetMeasurementById(int id)
        {
            return _measurementRepo.GetById(id);
        }

        public IEnumerable<ItemCategory> GetItemCategories()
        {
            var query = from f in _itemCategoryRepo.Table
                        select f;
            return query;
        }

        public IEnumerable<InventoryControlJournal> GetInventoryControlJournals()
        {
            var query = from f in _icjRepo.Table
                        select f;
            return query.AsEnumerable();
        }

        public Item GetItemByNo(string itemNo)
        {
            var query = from item in _itemRepo.Table
                        where item.No == itemNo
                        select item;
            return query.FirstOrDefault();
        }
        public InventoryCatalog GetInventoryByNo(string inventoryNo)
        {
            var query = from item in _inventoryRepo.Table
                        where item.SerialNo == inventoryNo
                        select item;
            return query.FirstOrDefault();
        }

        public void AddRules(ReoderingRule rules)
        {

            _reorderRepo.Insert(rules);
        }
        public void UpdateRule(ReoderingRule rules)
        {
            _reorderRepo.Update(rules);
            //_itemRepo.Update(item);
        }
        
        public IEnumerable<ReoderingRule> GetAllRules()
        {
            var query = from rules in _reorderRepo.Table select rules;
            var rule = query.ToList();
            return rule;

        }

        public ReoderingRule GetRuleById(int id)
        {
            var query = from reoders in _reorderRepo.Table
                        where reoders.Id == id
                        select reoders;
            return query.FirstOrDefault();

        }
        public void DeleteRules(ReoderingRule rules)
        {
            _reorderRepo.Delete(rules);
        }
        public void AddBranch(Branch branch)
        {
            _branchRepo.Insert(branch);
        }
        public void UpdateBranch(Branch branch)
        {
            _branchRepo.Update(branch);
            
        }
        public void DeleteBranch(Branch branch)
        {
            _branchRepo.Delete(branch);
        }

        public IEnumerable<Branch> GetAllBranches()
        {
            var query = from branch in _branchRepo.Table select branch;
            var branches = query.ToList();
            return branches;

        }

        public Branch GetBranchById(int id)
        {
            var query = from branches in _branchRepo.Table
                        where branches.Id == id
                        select branches;
            return query.FirstOrDefault();

        }

        public void AddCategory(Category category)
        {
            _categoryRepo.Insert(category);
        }
        public void UpdateCategory(Category category)
        {
            _categoryRepo.Update(category);
            //_itemRepo.Update(item);
        }
        public void DeleteCategory(Category category)
        {
            _categoryRepo.Delete(category);
            
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var query = from category in _categoryRepo.Table select category;
            var categories = query.ToList();
            return categories;

        }

        public Category GetCategoryById(int id)
        {
            var query = from categories in _categoryRepo.Table
                        where categories.Id == id
                        select categories;
            return query.FirstOrDefault();

        }

        public void AddLocation(Location location)
        {
            _locationRepo.Insert(location);
            
        }
        public void UpdateLocation(Location location)
        {
            _locationRepo.Update(location);
        
        }
        public void DeleteLocation(Location location)
        {
            _locationRepo.Delete(location);
            
        }

        public IEnumerable<Location> GetAllLocations()
        {
            var query = from locations in _locationRepo.Table select locations;
            var location = query.ToList();
            return location;
        }

        public Location GetLocationById(int id)
        {
            var query = from location in _locationRepo.Table
                        where location.Id == id
                        select location;
            return query.FirstOrDefault();
            
        }

        public void AddInventory(InventoryCatalog inventory)
        {
            _inventoryRepo.Insert(inventory);

        }
        public void UpdateInventory(InventoryCatalog inventory)
        {
            _inventoryRepo.Update(inventory);

        }
        public void DeleteInventory(InventoryCatalog inventory)
        {
            _inventoryRepo.Delete(inventory);
            
        }

        public IEnumerable<InventoryCatalog> GetAllInventories()
        {
            var query = from inventories in _inventoryRepo.Table select inventories;
            var inventory = query.ToList();
            return inventory;

        }

        public InventoryCatalog GetInventoryById(int id)
        {
            var query = from inventory in _inventoryRepo.Table
                        where inventory.Id == id
                        select inventory;
            return query.FirstOrDefault();
            
        }

        public void AddManufacturer(Manufacturer manufacturer)
        {
            _manufacturerRepo.Insert(manufacturer);

        }
        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            _manufacturerRepo.Update(manufacturer);

        }
        public void DeleteManufacturer(Manufacturer manufacturer)
        {
            _manufacturerRepo.Delete(manufacturer);
        }

        public IEnumerable<Manufacturer> Manufacturers()
        {
            var query = from manufacturers in _manufacturerRepo.Table select manufacturers;
            var manufacturer = query.ToList();
            return manufacturer;

        }

        public Manufacturer GetManufacturerById(int id)
        {
            var query = from manufacturer in _manufacturerRepo.Table
                        where manufacturer.Id == id
                        select manufacturer;
            return query.FirstOrDefault();

        }
    }
}

using Core.Domain;
using Core.Domain.Items;
using System.Collections.Generic;

namespace Services.Inventory
{
    public partial interface IInventoryService
    {
        InventoryControlJournal CreateInventoryControlJournal(int InventoryCatalogId,
            //int itemId,
            //int measurementId,
            DocumentTypes documentType,
            decimal? inQty,
            decimal? outQty,
            decimal? totalCost,
            decimal? totalAmount);

        void AddItem(Item item);
        void AddItemCategory(ItemCategory category);
        void UpdateItem(Item item);
        void DeleteItem(Item item);
        Item GetItemById(int id);
        Item GetItemByNo(string itemNo);
        InventoryCatalog GetInventoryByNo(string inventoryNo);
        IEnumerable<Item> GetAllItems();
        IEnumerable<Measurement> GetMeasurements();
        Measurement GetMeasurementById(int id);
        IEnumerable<ItemCategory> GetItemCategories();
        IEnumerable<InventoryControlJournal> GetInventoryControlJournals();

        void AddBranch(Branch branch);
        void UpdateBranch(Branch branch);
        void DeleteBranch(Branch branch);
        Branch GetBranchById(int id);
        IEnumerable<Branch> GetAllBranches();
        void AddInventory(InventoryCatalog inventory);
        void UpdateInventory(InventoryCatalog inventory);
        void DeleteInventory(InventoryCatalog inventory);
        InventoryCatalog GetInventoryById(int id);
        IEnumerable<InventoryCatalog> GetAllInventories();
        void AddRules(ReoderingRule rules);
        void UpdateRule(ReoderingRule rules);
        //void DeleteCategory(int categoryId);
        ReoderingRule GetRuleById(int id);
        IEnumerable<ReoderingRule> GetAllRules();
        void DeleteRules(ReoderingRule rules);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Category GetCategoryById(int id);
        IEnumerable<Category> GetAllCategories();
        void AddManufacturer(Manufacturer manufacturer);
        void UpdateManufacturer(Manufacturer manufacturer);
        void DeleteManufacturer(Manufacturer manufacturer);
        Manufacturer GetManufacturerById(int id);
        IEnumerable<Manufacturer> Manufacturers();

        void AddLocation(Location location);
        void UpdateLocation(Location location);
        void DeleteLocation(Location location);
        Location GetLocationById(int id);
        IEnumerable<Location> GetAllLocations();
    }
}

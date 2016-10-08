using Core.Domain.Financials;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Web.Models.ViewModels.Items
{
    public class Items
    {
        public Items()
        {
            ItemsList = new HashSet<ItemListLine>();
        }

        public ICollection<ItemListLine> ItemsList { get; set; }

        public class ItemListLine
        {
            public int ItemId { get; set; }
            public string No { get; set; }
            public string Description { get; set; }
            public decimal QtyOnHand { get; set; }
        }

        public class AddItem
        {
            public AddItem()
            {
                //ItemTypes = new HashSet<SelectListItem>();
            }
            public int? InventoryAccountId  { get; set; }
            public int? CostOfGoodsSoldAccountId { get; set; }
            public int? InventoryAdjustmentAccountId { get; set; }
            public int? SalesAccountId { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public int? ItemCategoryId { get; set; }
            public int? ItemType { get; set; }
            public string Name { get; set; }
            public decimal SellPrice { get; set; }
            public decimal  PurchaseCost { get; set; }
            public string SellDescription { get; set; }
            public string PurchaseDescription { get; set; }
            
            //public ICollection<SelectListItem> ItemTypes { get; set; }
        }
    }
}

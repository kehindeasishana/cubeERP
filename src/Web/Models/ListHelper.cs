using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;

namespace Web.Models
{
    public class ListHelper
    {
        public static IEnumerable<SelectListItem> GetEmployeeList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Employees
                           orderby l.EmployeeName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.EmployeeName };

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetLeaveBenefitList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.LeaveBenefits
                           orderby l.LeaveBenefitName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.LeaveBenefitName};

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetExitTypeList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.ExitTypes
                           orderby l.ExitTypeName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.ExitTypeName};

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetShiftList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Shifts
                           orderby l.ShiftName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.ShiftName };

                return list.ToList();
            }
        }
        public static IEnumerable<SelectListItem> GetLeaveTypeList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.LeaveTypes
                           orderby l.LeaveTypeName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.LeaveTypeName };

                return list.ToList();
            }
        }
        public static IEnumerable<SelectListItem> GetEmployeeStatusList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.EmployeeStatuses
                           orderby l.EmployeeStatusName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.EmployeeStatusName};

                return list.ToList();
            }
        }
        public static IEnumerable<SelectListItem> GetLocationList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Locations
                           orderby l.LocationName 
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.LocationName};

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetVendorsList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Vendors
                           orderby l.VendorUserName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.VendorUserName};

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetContactsList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Contacts
                           orderby l.FirstName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.FirstName };

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetCustomersList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Customers
                           orderby l.Username
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.Username};

                return list.ToList();
            }
        }
        public static IEnumerable<SelectListItem> GetManufacturerList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Manufacturers
                           orderby l.ManufacturerName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.ManufacturerName};

                return list.ToList();
            }
        }
        public static IEnumerable<SelectListItem> GetItemCategoryList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Categories
                           orderby l.CategoryName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.CategoryName};

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetBranchList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Branches
                           orderby l.BranchName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.BranchName};

                return list.ToList();
            }
        }
        public static IEnumerable<SelectListItem> GetCategoryList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Accounts
                           orderby l.AccountName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.AccountName };

                return list.ToList();
            }
        }
        public static IEnumerable<SelectListItem> GetInventoryList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.InventoryCatalog
                           orderby l.ItemName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.ItemName};

                return list.ToList();
            }
        }
        public static IEnumerable<SelectListItem> GetSubCategoryList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.AccountSubCategory
                           orderby l.AccountSubCategoryName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.AccountSubCategoryName };

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetAccountClassList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.AccountClasses
                           orderby l.Name
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.Name };

                return list.ToList();
            }
        }
    }
}
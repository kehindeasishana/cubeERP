using Services.Financial;
using System.Web.Mvc;
using System;
using System.Linq;
using Core.Domain.Financials;
using Web.Models.ViewModels.Financials;
using System.Collections.Generic;
using Web.Models.ViewModels.Administration;
using PagedList;
using PagedList.Mvc;
using System.Web;
using System.IO;
using Data;
using System.Data;
using Core.Domain;

namespace Web.Controllers
{
    //[Authorize(Roles= "SuperAdmin, Account Manager, CFO, Accountant, Chief Financial Officer, Inventory Manager, Store Manager, Store Keeper")]
   [Authorize(Roles = "SuperAdmin, Accountant, CFO, Account Manager")]
    public class FinancialController : BaseController
    {
        private readonly IFinancialService _financialService;
       public ActionResult Index()
        {
            return View();
        }
       public ActionResult loaddata()
       {
           using (ApplicationContext db = new ApplicationContext ())
           {
               var data = db.Accounts.OrderBy(s => s.AccountName).ToList();
               return Json(new { data = data },JsonRequestBehavior.AllowGet);
           }
           
       }
       [Audit]
        public ActionResult Accounts(string sortOn, string orderBy,
        string pSortOn, string keyword, int? page)
        {
            int recordsPerPage = 3;
            if (!page.HasValue)
            {
                page = 1; // set initial page value
                if (string.IsNullOrWhiteSpace(orderBy) || orderBy.Equals("asc"))
                {
                    orderBy = "desc";
                }
                else
                {
                    orderBy = "asc";
                }
            }

            if (!string.IsNullOrWhiteSpace(sortOn) && !sortOn.Equals(pSortOn,
StringComparison.CurrentCultureIgnoreCase))
            {
                orderBy = "asc";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SortOn = sortOn;
            ViewBag.Keyword = keyword;
            var accounts = _financialService.GetAccounts();
            //var list = db.PersonalDetails.AsQueryable();

            switch (sortOn)
            {
                case "Category":
                    if (orderBy.Equals("desc"))
                    {
                        accounts = accounts.OrderByDescending(p => p.AccountName);
                        //list = list.OrderByDescending(p => p.FirstName);
                    }
                    else
                    {
                        accounts = accounts.OrderBy(p => p.AccountName);
                        //list = list.OrderBy(p => p.FirstName);
                    }
                    break;
                case "AccountName":
                    if (orderBy.Equals("desc"))
                    {
                        accounts = accounts.OrderByDescending(p => p.AccountClass.Name);
                        //list = list.OrderByDescending(p => p.LastName);
                    }
                    else
                    {
                        accounts = accounts.OrderBy(p => p.AccountClass.Name);
                        //list = list.OrderBy(p => p.LastName);
                    }
                    break;
                case "AccountCode":
                    if (orderBy.Equals("desc"))
                    {
                        accounts = accounts.OrderByDescending(p => p.AccountCode);
                        //list = list.OrderByDescending(p => p.Age);
                    }
                    else
                    {
                        accounts = accounts.OrderBy(p => p.AccountCode);
                       // list = list.OrderBy(p => p.Age);
                    }
                    break;
                default:
                    //list = list.OrderBy(p => p.AutoId);
                    accounts = accounts.OrderBy(p => p.Id);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                accounts = accounts.Where(f=>f.AccountClass.Name.StartsWith(keyword));
                //list = list.Where(f => f.FirstName.StartsWith(keyword));
            }
            var finalAccounts = accounts.ToPagedList(page.Value, recordsPerPage);
            //var finalList = list.ToPagedList(page.Value, recordsPerPage);
            //return View(finalList);
            return View(finalAccounts);
        }
        public FinancialController(IFinancialService financialService)
        {
            _financialService = financialService;
            
        }
       [Audit]
        public ActionResult Company()
        {
            var company = _financialService.GetDefaultCompany();

            return View(company);
        }
        public ActionResult AddCompany()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddCompany(CompanySetUp model, HttpPostedFileBase upload)
        {

            if (upload != null && upload.ContentLength > 0)
            {
                var avatar = new Files
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Avatar,
                    ContentType = upload.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    avatar.Content = reader.ReadBytes(upload.ContentLength);
                }
                model.files = new List<Files> { avatar };
            }
            _financialService.AddCompany(new CompanySetUp()
            {
                CompanyName = model.CompanyName,
                BusinessType = model.BusinessType,
                ShortName = model.ShortName,
                Logo = model.Logo,
                Address = model.Address,
                Email = model.Email,
                Website = model.Website
            });
            return RedirectToAction("ListCompany");
        }
        public ActionResult EditCompany()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult EditCompany(CompanySetUp model)
        {
            var company = _financialService.GetCompanyById(model.Id);
            company.Logo = model.Logo;
            company.ShortName = model.ShortName;
            company.Website = model.Website;
            company.Email = model.Email;
            company.CompanyName = model.CompanyName;
            company.BusinessType = model.BusinessType;
            company.Address = model.Address;
            _financialService.EditCompany(company);
            return RedirectToAction("ListCompany");
        }
        [Audit]
        public ActionResult ListCompany()
        {
            var company = _financialService.ListCompany();
            return View(company);
        }
       [Audit]
       public ActionResult GetCompany(int id)
        {
            var company = _financialService.GetCompanyById(id);
            return View(company);
        }
        public ActionResult AddTax()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddTax(Tax model)
        {
            _financialService.AddNewTax(new Tax()
            {
                TaxCode = model.TaxCode,
                TaxName = model.TaxName,
                TaxRate = model.TaxRate,
                TaxType = model.TaxType
            });
            return RedirectToAction("Tax");
        }
       [Audit]
        public ActionResult Tax()
        {
            var tax = _financialService.GetAllTax();
            return View(tax);
        }
        
       public ActionResult EditTax()
       {
           return View();
       }
       [Audit ]
       [HttpPost]
       public ActionResult EditTax(Tax model)
       {
           var tax = _financialService.GetTax(model.Id);
           tax.IsActive = model.IsActive;
           tax.TaxCode = model.TaxCode;
           tax.TaxName = model.TaxName;
           tax.TaxRate = model.TaxRate;
           tax.TaxType = model.TaxType;
           _financialService.UpdateTax(tax);
           return RedirectToAction("Tax");
       }
        [Audit]
        public ActionResult Banks()
        {
            var banks = _financialService.Banks();
            return View(banks);
        }
        public ActionResult AddBank()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddBank(Bank model)
        {
            _financialService.AddBank(new Bank()
            {
                BankBranch = model.BankBranch,
                AccountId = model.AccountId,
                IsActive = model.IsActive,
                Address = model.Address,
                Name = model.Name,
                Number = model.Number,
                Type = model.Type
            });
            return RedirectToAction("Banks");
        }
        public ActionResult EditBank()
        {
            return View();
        }
        [Audit]
       [HttpPost]
       public ActionResult EditBank(Bank model)
        {
            var banks = _financialService.GetBank(model.Id);
            banks.IsActive = model.IsActive;
            banks.Name = model.Name;
            banks.Number = model.Number;
            banks.AccountId = model.AccountId;
            banks.Address = model.Address;
            banks.BankBranch = model.BankBranch;
            _financialService.EditBank(banks);
            return RedirectToAction("Banks");
        }
        //[Authorize]
        public ActionResult DeleteBank(int id)
        {
            Bank bank = _financialService.GetBank(id);
            return View(bank);

        }
       [Audit]
        [HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteBank(int id, Bank bank)
        {
            var banks = _financialService.GetBank(id);
            _financialService.DeleteBank(banks);
            return RedirectToAction("Banks");
        }
       [Audit]
       public ViewResult GetBank(int id)
       {
           Bank bank = _financialService.GetBank(id);
           return View(bank);

       }
        [Audit]
        public ActionResult GetFinancialYears()
        {
            var year = _financialService.GetFinancialYears();
            return View(year);
        }
       [Audit]
       public ActionResult AddFinancialYear()
        {
            return View();
        }
       [HttpPost]
       public ActionResult AddFinancialYear(FinancialYear model)
       {
         
           _financialService.AddFiscalYear(new FinancialYear()
               {
                   //FiscalYearName = model.FiscalYearName,
                   FiscalYearCode = model.FiscalYearCode,
                   StartDate = model.StartDate,
                   EndDate = model.EndDate,
                   IsActive = model.IsActive
               });
           return RedirectToAction("GetFinancialYears");
       }
       [Audit]
       public ViewResult GetFinancialYear(int id)
       {
           FinancialYear year = _financialService.GetFinancialYear(id);
           return View(year);

       }
        public ActionResult AddAccountClass()
        {
            return View();
        }
        [Audit]
        [HttpPost, ActionName("AddAccountClass")]
        public ActionResult AddAccountClass(AccountClass model)
        {
            _financialService.AddAccountClass(new AccountClass() 
            { 
                Name = model.Name,
                NormalBalance = model.NormalBalance,
                Description = model.Description,
                AccountId = model.AccountId,
                AccountNo = model.AccountNo
            });
            
            return RedirectToAction("ViewAccountClass");
        }
       [Audit]
        public ActionResult ViewAccountClass(AccountClass model)
        {
            var viewAccountClass = _financialService.ViewAccountClass();

            return View(viewAccountClass);
           
        }

       public ActionResult EditAccountClass(int id)
       {
           return View(this._financialService.GetAccountClass(id));
       }
       [Audit]
       [HttpPost, ActionName("EditAccountClass")]
      
       public ActionResult EditAccountClass(AccountClass model)
       {

           var accountClass = _financialService.GetAccountClass(model.Id);
           accountClass.Name = model.Name;
           accountClass.NormalBalance = model.NormalBalance;
           accountClass.AccountNo = model.AccountNo;
           accountClass.AccountId = model.AccountId;
           accountClass.Description = model.Description;
           _financialService.EditAccountClass(accountClass);
           

           return RedirectToAction("ViewAccountClass");
       }
       [Audit]
       [Authorize]
       public ViewResult GetAccountClass(int id)
       {
           //AccountingEntry account = AccountRepo.GetById(id);
           AccountClass accountClass = _financialService.GetAccountClass(id);

           return View(accountClass);

       }
       [Audit]
       [Authorize]
       public ActionResult DeleteAccountClass(int id)
       {
           AccountClass accountClass = _financialService.GetAccountClass(id);

           return View(accountClass);

       }

       [HttpPost, ActionName("DeleteAccountClass")]
       [Authorize]
       [ValidateAntiForgeryToken]
       public ActionResult DeleteAccountClass(int id, AccountClass accountClass)
       {
           var accountType = _financialService.GetAccountClass(id);
           _financialService.DeleteAccountClass(accountType);
           return RedirectToAction("ViewAccountClass");
       }
        public ActionResult AddAccount()
        {
            return View(new AddAccountViewModel());
        }
        [Audit]
        [HttpPost, ActionName("AddAccount")]
        [FormValueRequiredAttribute("Create")]
        public ActionResult AddAccount(AddAccountViewModel model)
        {
            Account account = new Account()
            {
                //AccountCode = model.AccountCode,
                AccountName = model.AccountName,
                AccountClassId = model.AccountClass,
                Description = model.Description
            };

            _financialService.AddAccount(account);

            return RedirectToAction("Accounts");
        }

        public ActionResult EditAccount(int id)
        {
            var account = _financialService.GetAccounts().Where(a => a.Id == id).FirstOrDefault();

            Models.ViewModels.Financials.EditAccountViewModel model = new Models.ViewModels.Financials.EditAccountViewModel()
            {
                Id = account.Id,
                //AccountCode = account.AccountCode,
                AccountName = account.AccountName,
                AccountClass = account.AccountClass.Name,
               
                Balance = account.Balance
               
            };


            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("EditAccount")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult EditAccount(Models.ViewModels.Financials.EditAccountViewModel model)
        {
            var account = _financialService.GetAccounts().Where(a => a.Id == model.Id).FirstOrDefault();

            //account.AccountCode = model.AccountCode;
            account.AccountName = model.AccountName;
           
            _financialService.UpdateAccount(account);

            return RedirectToAction("Accounts");
        }
        [Audit]
        public ActionResult ViewAccountsPDF()
        {
            var accounts = _financialService.GetAccounts();
            var model = new Models.ViewModels.Financials.Accounts();
            foreach (var account in accounts)
            {
                model.AccountsListLines.Add(new Models.ViewModels.Financials.AccountsListLine()
                {
                    Id = account.Id,
                    //AccountCode = account.AccountCode,
                    AccountName = account.AccountName,
                    Balance = account.Balance
                });
            }

            var html = base.RenderPartialViewToString("Accounts", model);
            HttpContext.Response.Clear();
            HttpContext.Response.AddHeader("Content-Type", "application/pdf");
            HttpContext.Response.Filter = new PdfFilter(HttpContext.Response.Filter, html);

            return Content(html);
        }
        //[Audit]
        //public ActionResult JournalEntries()
        //{
        //    var journalEntries = _financialService.GetJournalEntries();
        //    var model = new JournalEntries();
        //    foreach(var je in journalEntries)
        //    {
        //            model.JournalEntriesListLines.Add(new JournalEntriesListLine()
        //            {
        //                Id = je.Id,
        //                AccountId = je.AccountId,
        //                AccountCode = je.Account.AccountCode,
        //                AccountName = je.Account.AccountName,
        //                DrCr = (int)je.DrCr == 1 ? "Dr" : "Cr",
        //                Amount = je.Amount
        //        });
                
        //    }
        //    return View(model);
        //}
        [Audit]
        public ActionResult JournalEntries(string sortOn, string orderBy,
        string pSortOn, string keyword, int? page)
        {
            int recordsPerPage = 3;
            if (!page.HasValue)
            {
                page = 1; // set initial page value
                if (string.IsNullOrWhiteSpace(orderBy) || orderBy.Equals("asc"))
                {
                    orderBy = "desc";
                }
                else
                {
                    orderBy = "asc";
                }
            }

            if (!string.IsNullOrWhiteSpace(sortOn) && !sortOn.Equals(pSortOn,
            StringComparison.CurrentCultureIgnoreCase))
            {
                orderBy = "asc";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SortOn = sortOn;
            ViewBag.Keyword = keyword;
            var journals = _financialService.GetJournalEntries();
            switch (sortOn)
            {
                case "Category":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.Account.AccountName);
                       
                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.Account.AccountName);
                       
                    }
                    break;
                case "SubCategory":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.SubCategory.AccountSubCategoryName);

                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.SubCategory.AccountSubCategoryName);

                    }
                    break;

                

                case "Date":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.Date);

                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.Date);

                    }
                    break;

                case "DrCr":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.DrCr);

                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.DrCr);

                    }
                    break;

                case "AccountName":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.Account.AccountClass.Name);
                        //list = list.OrderByDescending(p => p.Age);
                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.Account.AccountClass.Name);
                        // list = list.OrderBy(p => p.Age);
                    }
                    break;
                default:
                    //list = list.OrderBy(p => p.AutoId);
                    journals = journals.OrderBy(p => p.Id);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                journals = journals.Where(f=>f.Account.AccountClass.Name.StartsWith(keyword));
                
            }
            var finalJournals = journals.ToPagedList(page.Value, recordsPerPage);
            
            return View(finalJournals);
        }
       public ActionResult AddAccountSubCategory()
        {
          
            return View();
        }

       [Audit]
       [HttpPost]
      
       public ActionResult AddAccountSubCategory(AccountSubCategory model)
       {
           _financialService.AddAccountSubCategory(new AccountSubCategory()
           {
               
               AccountSubCategoryName = model.AccountSubCategoryName,
               AccountId = model.AccountId,
              // AccountClassId = model.AccountClassId,
               Description = model.Description
                
               //AccountName = Model.AccountName,
               //SubCategory = Model.SubCategory
           });
           

          // return RedirectToAction("Accounts");
           return RedirectToAction("Subcategories");
       }
       public ActionResult EditSubCategory(int id)
       {
           return View(this._financialService.GetSubCategoryById(id));
       }
       [Audit]
       [HttpPost, ActionName("EditSubcategory")]
       //[FormValueRequiredAttribute("UpdateSubCategory")]
       public ActionResult EditSubCategory(AccountSubCategory model)
       {
           
           var subCategory = _financialService.GetSubCategoryById(model.Id);
           subCategory.AccountSubCategoryName = model.AccountSubCategoryName;
           subCategory.AccountId = model.AccountId;
           subCategory.Description = model.Description;
            _financialService.EditAccountSubCategory(subCategory);
           
           return RedirectToAction("Subcategories");
       }
       [Authorize]
       public ViewResult ViewSubCategory(int id)
       {
           //AccountingEntry account = AccountRepo.GetById(id);
          AccountSubCategory subCategory =  _financialService.GetSubCategoryById(id);
           
           return View(subCategory);

       }

       [Authorize]
       public ActionResult DeleteSubCategory(int id)
       {
           AccountSubCategory subCategory = _financialService.GetSubCategoryById(id);
           
           return View(subCategory);

       }

       [HttpPost, ActionName("DeleteSubCategory")]
       [Authorize]
       [ValidateAntiForgeryToken]
       public ActionResult DeleteSubCategory(int id, AccountSubCategory subCategory)
       {
           var subCategories = _financialService.GetSubCategoryById(id);
           _financialService.DeleteSubCategory(subCategories);
           return RedirectToAction("Subcategories");
       }
       [Audit]
       public ActionResult Subcategories()
       {
           var subCategories = _financialService.ListAccountSubCategory();
           
           return View(subCategories);
       }
        public ActionResult AddJournalEntry()
        {
            //ApplicationContext db = new ApplicationContext();
            //AccountClass accountClass = new AccountClass();
            //accountClass.Name = new List<Account>().ToString();
            //accountClass.Name = GetAllAccountCategory().ToString();
            
            //return View(accountClass);

            var model = new AddJournalEntry();
            return View(model);
        }

        [HttpPost]
        public ActionResult GetCategoryById(int id)
        {
            ApplicationContext db = new ApplicationContext();
            List<AccountSubCategory> accountSubCategory = db.AccountSubCategory.Where(s=>s.AccountId == id).ToList();
            //Models.TheDataContext db = new Models.TheDataContext();
            //List<Models.Model> models = db.Models.Where(p => p.MakeID == id).ToList();

            return Json(accountSubCategory);
        }

        //[HttpPost]
        //public ActionResult GetCategoryById(string Id)
        //{
        //    //List<Accounts> objcity = new List<Accounts>();
        //    List<SelectListItem> accountCategory = new List<SelectListItem>();
        //    using (ApplicationContext db = new ApplicationContext())
        //    {
        //        accountCategory = GetAllAccountCategory().Where(m => m.Value.Equals(Id)).ToList();
        //        SelectList obgcity = new SelectList(accountCategory, "AccountName", "Id");
        //        return Json(obgcity);
        //    }

        //}
        //public List<SelectListItem> GetAllAccounts()
        //{

        //    List<SelectListItem> account = new List<SelectListItem>();
        //    using (ApplicationContext db = new ApplicationContext())
        //    {
        //        var query = from u in db.Accounts select u;
        //        if (query.Count() > 0)
        //        {
        //            foreach (var v in query)
        //            {
        //                account.Add(new SelectListItem { Text = v.AccountName, Value = v.Id.ToString() });
        //            }
        //        }
        //        ViewBag.Accounts = account;
        //    }
        //    return account;
        //}

        //public List<SelectListItem> GetAllAccountCategory()
        //{
        //    // List<AccountSubCategory>accountCategory = new List<AccountSubCategory>().ToList();
        //    //accountCategory.Add(new AccountSubCategory { AccountSubCategoryName = acc, Id});
        //    List<SelectListItem> accountCategory = new List<SelectListItem>();
        //    using (ApplicationContext db = new ApplicationContext())
        //    {
        //        var query = from u in db.AccountSubCategory select u;
        //        if (query.Count() > 0)
        //        {
        //            foreach (var v in query)
        //            {
        //                accountCategory.Add(new SelectListItem { Text = v.AccountSubCategoryName, Value = v.Id.ToString() });
        //            }
        //        }
        //        ViewBag.AccountSubCategory = accountCategory;
        //    }
        //    return accountCategory;
        //}

      

        [Audit]
        [HttpPost, ActionName("AddJournalEntry")]
        [FormValueRequiredAttribute("AddJournalEntry")]

        public ActionResult AddJournalEntry(AddJournalEntry model)
        {
            JournalEntryLine journal = new JournalEntryLine()
            {
                AccountId = model.AccountId,
                ReferenceNo = model.ReferenceNo,
                DrCr = model.DrCr,
                Date = model.Date,
                Amount= model.Amount,
                Description = model.Memo,
                SubCategoryId = model.SubCategoryId
            };
           
            _financialService.AddJournalEntry(journal);

            return RedirectToAction("JournalEntries");
        }
        //public ActionResult AddJournalEntryLine(AddJournalEntry model)
        //{
        //    //if(model.AccountId != -1 && model.Amount > 0)
        //    if(model.Amount > 0)
        //    {
        //        //var rowId = Guid.NewGuid().ToString();
        //        model.AddJournalEntryLines.Add(new AddJournalEntryLine()
        //        {
        //            AccountId = model.AccountId,
        //            AccountName = _financialService.GetAccounts().Where(a => a.Id == model.AccountId).FirstOrDefault().AccountName,
        //            DrCr = model.DrCr,
        //            Amount = model.Amount,
        //            Memo = model.MemoLine
        //        });
        //    }
        //    return View(model);
        //}

        public ActionResult EditJournalEntry(int id)
        {
            // for now, use the same view model as add journal entry. nothing different
            var je = _financialService.GetJournalEntry(id);

            var model = new AddJournalEntry();
            model.Date = je.Date;
            model.Memo = je.Description;
            model.ReferenceNo = je.ReferenceNo;
            model.Id = je.Id;
            //model.JournalEntryId = je.Id;
           // model.Posted = je.Posted.HasValue ? je.Posted.Value : false;

          
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("EditJournalEntry")]
        [FormValueRequiredAttribute("UpdateJournalEntry")]
        public ActionResult EditJournalEntry(AddJournalEntry model)
        {
            if (model.AddJournalEntryLines.Count < 2)
                return View(model);

            var journalEntry = _financialService.GetJournalEntry(model.Id);

            journalEntry.Date = model.Date;
            journalEntry.Description = model.Memo;
            journalEntry.ReferenceNo = model.ReferenceNo;

            _financialService.UpdateJournalEntry(journalEntry);

            return RedirectToAction("JournalEntries");
        }


        [HttpPost, ActionName("AddJournalEntry")]
        [FormValueRequiredAttribute("DeleteJournalEntryLine")]
        public ActionResult DeleteJournalEntryLine(AddJournalEntry model)
        {
            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            
            return View(model);
        }

        [HttpPost, ActionName("EditJournalEntry")]
        [FormValueRequiredAttribute("DeleteJournalEntryLine")]
        public ActionResult UpdateJournalEntryLine(AddJournalEntry model)
        {
            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            //model.AddJournalEntryLines.Remove(model.AddJournalEntryLines.Where(i => i.RowId.ToString() == deletedItem.ToString()).FirstOrDefault());
            return View(model);
        }

        [HttpPost, ActionName("AddJournalEntry")]
        [FormValueRequiredAttribute("SaveJournalEntry")]
        public ActionResult SaveJournalEntry(AddJournalEntry model)
        {
            if(model.AddJournalEntryLines.Count < 2)
                return View(model);
            var journalEntry = new JournalEntryLine()
            {
                Date = model.Date,
                Description = model.Memo,
                ReferenceNo = model.ReferenceNo,
                
            };
       
            _financialService.AddJournalEntry(journalEntry);
            return RedirectToAction("JournalEntries");
        }
       [Audit]
        public ActionResult TrialBalance()
        {
            var model = _financialService.TrialBalance();
            return View(model);
        }
        [Audit]
        public ActionResult BalanceSheet()
        {
            var model = _financialService.BalanceSheet().ToList();
            var dt = Helpers.CollectionHelper.ConvertTo<BalanceSheet>(model);
            var incomestatement = _financialService.IncomeStatement();
            var netincome = incomestatement.Where(a => a.IsExpense == false).Sum(a => a.Amount) - incomestatement.Where(a => a.IsExpense == true).Sum(a => a.Amount);


            return View(model);
        }
        [Audit]
        public ActionResult IncomeStatement()
        {
            var model = _financialService.IncomeStatement();
            return View(model);
        }
        [Audit]
        public ActionResult PLStatement()
        {
            var model = _financialService.ProfitAndLoss();
            return View(model);
        }
        //public ActionResult Banks()
        //{
        //    var model = new Banks();
        //    var banks = _financialService.GetCashAndBanks();
        //    foreach (var bank in banks)
        //    {
        //        model.BankList.Add(new BankListLine()
        //        {
        //            Name = bank.Name,
        //            BankName = bank.BankBranch,
        //            AccountId = bank.AccountId,
        //            Number = bank.Number,
        //            Type = bank.Type,
        //            Address = bank.Address,
        //            IsActive = bank.IsActive,
        //            IsDefault = bank.IsDefault
        //        });
        //    }
        //    return View(model);
        //}

        //public ActionResult TaxGroups()
        //{
        //    var taxGroups = _financialService.GetTaxGroups();
        //    var model = new List<TaxGroupModel>();

        //    foreach(var group in taxGroups)
        //    {
        //        var groupTaxes = new List<TaxGroupTaxModel>();

        //        foreach (var groupTax in group.TaxGroupTax)
        //        {
        //            groupTaxes.Add(new TaxGroupTaxModel()
        //            {
        //                Id = groupTax.Id,
        //                TaxId = groupTax.TaxId,
        //                TaxGroupId = groupTax.TaxGroupId
        //            });
        //        }

        //        model.Add(new TaxGroupModel()
        //        {
        //            Id = group.Id,
        //            Description = group.Description,
        //            TaxAppliedToShipping = group.TaxAppliedToShipping,
        //            IsActive = group.IsActive,      
        //            TaxGroupTaxModel = groupTaxes
        //        });
        //    }

        //    return View(model);
        //}
    }
}

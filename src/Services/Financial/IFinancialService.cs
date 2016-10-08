using Core.Domain;
using Core.Domain.Financials;
//using Core.Domain.TaxSystem;
using System;
using System.Collections.Generic;

namespace Services.Financial
{
    public partial interface IFinancialService
    {
        ICollection<Tax> GetAllTaxes(bool includeInActive);
        IEnumerable<Tax> GetAllTax();
        void AddCompany(CompanySetUp company);
        void EditCompany(CompanySetUp company);
        CompanySetUp GetCompanyById(int id);
        ICollection<CompanySetUp> ListCompany();
        void AddNewTax(Tax tax);
        void UpdateTax(Tax tax);
        Tax GetTax(int id);
        void DeleteTax(Tax tax);
        void AddBank(Bank bank);
        IEnumerable<Bank> Banks();
        void EditBank(Bank bank);
        void DeleteBank(Bank bank);
        Bank GetBank(int id);
        void InitializeCompany();
       
        CompanySetUp GetDefaultCompany();
        FinancialYear GetFinancialYear(int id);
        ICollection<FinancialYear> GetFinancialYears();
        IEnumerable<Account> GetAccounts();
        IEnumerable<JournalEntryLine> GetJournalEntries();
        void AddJournalEntry(JournalEntryLine journalEntry);
        ICollection<TrialBalance> TrialBalance(DateTime? from = null, DateTime? to = null);
        ICollection<BalanceSheet> BalanceSheet(DateTime? from = null, DateTime? to = null);
        ICollection<IncomeStatement> IncomeStatement(DateTime? from = null, DateTime? to = null);
        ICollection<ProfitAndLoss> ProfitAndLoss(DateTime? from = null, DateTime? to = null);
        //ICollection<MasterGeneralLedger> MasterGeneralLedger(DateTime? from = null, DateTime? to = null, string accountCode = null, int? transactionNo = null);
        FinancialYear CurrentFiscalYear();
        //IEnumerable<Tax> GetTaxes();
        //IEnumerable<ItemTaxGroup> GetItemTaxGroups();
        //IEnumerable<TaxGroup> GetTaxGroups();
        IEnumerable<Bank> GetCashAndBanks();
        List<KeyValuePair<int, decimal>> ComputeInputTax(int vendorId, string item, decimal quantity, decimal amount, decimal discount);
        List<KeyValuePair<int, decimal>> ComputeOutputTax(int customerId, string item, decimal quantity, decimal amount, decimal discount);
       
        //void AddMainContraAccountSetting(int masterAccountId, int contraAccountId);
        void UpdateAccount(Account account);
        //JournalEntryLine GetJournalEntry(int id, bool fromGL = false);
        void UpdateJournalEntry(JournalEntryLine journalEntry);
        JournalEntryLine GetJournalEntry(int id);
        Account GetAccountByAccountCode(string accountcode);
        Account GetAccount(int id);
        IEnumerable<AccountSubCategory> ListAccountSubCategory();
        IEnumerable<AccountClass> ViewAccountClass();
        AccountSubCategory GetSubCategoryById(int id);
        void DeleteSubCategory(AccountSubCategory subCategory);
        void EditAccountSubCategory(AccountSubCategory accountSubCategory);
        void AddAccountSubCategory(AccountSubCategory accountSubCategory);
        void AddAccountClass(AccountClass accountClass);
        AccountClass GetAccountClass(int id);
        void EditAccountClass(AccountClass editAccountClass);
        void DeleteAccountClass(AccountClass accountClass);
        void AddAccount(Account account);
        void AddFiscalYear(FinancialYear financialYear);
       // void UpdateFinancialYear(FinancialYear financialYear);
    }
}

using Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models.ViewModels.Financials
{
    public class JournalEntries
    {
        public JournalEntries()
        {
            JournalEntriesListLines = new HashSet<JournalEntriesListLine>();
        }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public ICollection<JournalEntriesListLine> JournalEntriesListLines { get; set; }
    }

    public class JournalEntriesListLine
    {
        public int Id { get; set; }
        public int? SubCategoryId { get; set; }
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
       
        public string DrCr { get; set; }
        public decimal Amount { get; set; }
       
    }

   
    public class AddJournalEntry
    {
        public AddJournalEntry()
        {
            Accounts = new HashSet<SelectListItem>();
            AddJournalEntryLines = new List<AddJournalEntryLine>();
            Date = DateTime.Now;
        }

        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public string Memo { get; set; }
        public int Id { get; set; }
        //public int JournalEntryId { get; set; }
        public bool Posted { get; set; }
       
        public ICollection<SelectListItem> Accounts { get; set; }
        public IList<AddJournalEntryLine> AddJournalEntryLines { get; set; }

        #region Fields for New Journal Entry
        public int? SubCategoryId { get; set; }
        public int AccountId { get; set; }
        public DrOrCrSide DrCr { get; set; }
        public decimal Amount { get; set; }
        public string MemoLine { get; set; }

        
        #endregion
    }

    public class AddJournalEntryLine
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }
        public DrOrCrSide DrCr { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
    }
}

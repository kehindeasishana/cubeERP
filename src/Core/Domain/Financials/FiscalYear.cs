using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Financials
{
    public partial class FinancialYear : BaseEntity
    {
        //[Required]
        [StringLength(10)]
        [DisplayName("Fiscal Year Code")]
        public string FiscalYearCode { get; set; }

        //[Required]
        [StringLength(100)]
        public string FiscalYearName { get; set; }
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}

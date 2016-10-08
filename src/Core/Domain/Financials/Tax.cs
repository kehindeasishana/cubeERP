using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Financials
{
    public class Tax : BaseEntity
    {
        [DisplayName("Tax Name")]
        public string TaxName { get; set; }
        [DisplayName("Tax Type")]
        public string TaxType { get; set; }
        [DisplayName("Tax Code")]
        public string TaxCode { get; set; }
        [DisplayName("Tax Rate")]
        public string TaxRate { get; set; }
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
    }
}

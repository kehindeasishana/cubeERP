using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Financials
{
    public class CompanySetUp : BaseEntity
    {
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        [DisplayName("Business Type")]
        public string BusinessType { get; set; }
        [DisplayName("Short Name")]
        public string ShortName { get; set; }
        public byte[] Logo { get; set; }
        public virtual ICollection<Files> files { get; set; }
    }
}

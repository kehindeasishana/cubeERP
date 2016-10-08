using Core.Domain.Items;
using Core.Domain.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{

    public partial class Contact : BaseEntity
    {
        public Contact()
        {
        }
        /// <summary>
        /// Check ContactyType to determine whether CompanyNo is Customer No or Vendor No
        /// </summary>
        [DisplayName("Contact Type")]
        public ContactTypes ContactType { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNo { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string ResaleNo { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<InventoryCatalog> InventoryCatalogs { get; set; }
        //public int PartyId { get; set; }
        //public virtual Party Party { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{

    public partial class Address : BaseEntity
    {
        public string No { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }
}

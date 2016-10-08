using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public partial class SequenceNumber : BaseEntity
    {
        public SequenceNumberTypes SequenceNumberType { get; set; }
        public string Description { get; set; }
        public string Prefix { get; set; }
        public int NextNumber { get; set; }
        public bool UsePrefix { get; set; }
    }
}

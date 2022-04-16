using SessionService.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SessionService.Entities
{
    [ClassAttributes(ClassName = "Customer")]
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int SegmentId { get; set; }

        [ForeignKey("SegmentId")]
        public Segment segment { get; set; }
    }
}

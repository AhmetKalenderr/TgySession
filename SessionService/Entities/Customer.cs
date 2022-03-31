using System.ComponentModel.DataAnnotations.Schema;

namespace SessionService.Entities
{
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

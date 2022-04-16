using SessionService.Attributes;

namespace SessionService.Entities
{
    [ClassAttributes(ClassName = "Segment")]

    public class Segment
    {
        public int Id { get; set; }

        public string Code { get; set; }
    }
}

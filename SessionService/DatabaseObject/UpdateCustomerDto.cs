namespace SessionService.DatabaseObject
{
    public class UpdateCustomerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int SegmentId { get; set; }
    }
}

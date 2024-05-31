namespace FacadeApi.Models
{
    public class StoredRecord
    {
        public int RecordId { get; set; }
        public string? UserString { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
}

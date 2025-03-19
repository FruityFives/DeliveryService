namespace WorkerService.Models
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime BookingDate { get; set; }
        public int Priority { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace WorkerService.Models
{
    public class BookingRepository
    {
        private readonly List<BookingDTO> _bookings = new();

        public void Put(BookingDTO booking)
        {
            _bookings.Add(booking);
        }

        public List<BookingDTO> GetAll()
        {
            return _bookings.OrderBy(b => b.Priority).ToList();
        }
    }
}

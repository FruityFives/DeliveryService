using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService.Models
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly BookingRepository _repository = new();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Simuler modtagelse af booking fra RabbitMQ
                var newBooking = new BookingDTO
                {
                    Id = new Random().Next(1, 1000),
                    CustomerName = "Kunde " + new Random().Next(1, 100),
                    BookingDate = DateTime.Now,
                    Priority = new Random().Next(1, 10)
                };

                _repository.Put(newBooking);
                _logger.LogInformation($"Booking {newBooking.Id} tilføjet med prioritet {newBooking.Priority}");

                // **Skriv bookinger til CSV-fil**
                SaveBookingsToCsv(_repository.GetAll());

                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Worker stopped");
        }

        private void SaveBookingsToCsv(List<BookingDTO> bookings)
        {
            string csvFilePath = "bookings.csv";

            using (var writer = new StreamWriter(csvFilePath))
            {
                writer.WriteLine("Id,CustomerName,BookingDate,Priority");

                foreach (var booking in bookings)
                {
                    string line = $"{booking.Id},{booking.CustomerName},{booking.BookingDate},{booking.Priority}";
                    writer.WriteLine(line);
                }
            }

            _logger.LogInformation("Bookings saved to CSV.");
        }
    }
}

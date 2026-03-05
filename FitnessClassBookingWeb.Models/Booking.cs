using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessClassBookingWeb.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }

        public int ScheduleId { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }

        public Schedule Schedule { get; set; }
    }
}

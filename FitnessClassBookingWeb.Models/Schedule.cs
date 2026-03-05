using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessClassBookingWeb.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public int GroupId { get; set; }

        public int RoomId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Group Group { get; set; }

        public Room Room { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}

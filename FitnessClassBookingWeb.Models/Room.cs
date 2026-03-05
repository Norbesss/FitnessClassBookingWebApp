using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessClassBookingWeb.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}

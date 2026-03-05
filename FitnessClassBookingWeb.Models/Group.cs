using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessClassBookingWeb.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CoachId { get; set; }

        public int MaxParticipants { get; set; }

        public User Coach { get; set; }

        public ICollection<Schedule> Schedules { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}

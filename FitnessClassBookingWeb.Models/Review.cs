using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessClassBookingWeb.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int UserId { get; set; }

        public int GroupId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }

        public Group Group { get; set; }
    }
}

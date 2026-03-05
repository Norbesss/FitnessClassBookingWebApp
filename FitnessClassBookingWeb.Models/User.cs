using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FitnessClassBookingWeb.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public ICollection<UserRole> UserRoles { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<Group> CoachingGroups { get; set; }
    }
}

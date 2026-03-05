using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessClassBookingWeb.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}

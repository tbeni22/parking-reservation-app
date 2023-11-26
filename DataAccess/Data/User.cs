using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Data
{
    [Table("app_user")]
    public class User : IdentityUser<int>
    {

        public bool Disabled { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public ICollection<FailureReport> FailureReports { get; set; }

        public string? Address { get; set; }
        public string? Name { get; set; }
    }
}

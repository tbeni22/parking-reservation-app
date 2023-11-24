using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Data
{
    [Table("app_user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("UserName")]
        public String Name { get; set; }

        [Column("email")]
        public String Email { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

        public ICollection<FailureReport> FailureReports { get; set; }
    }
}

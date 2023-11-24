using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        internal static UserDto FromDataEntity(User user)
        {
            return new UserDto
            {
                Id = user.ID,
                Name = user.Name,
                Email = user.Email,
                Reservations = user.Reservations
            };
        }
    }
}

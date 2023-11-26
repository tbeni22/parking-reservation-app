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
        public string? Address { get; set; } 

        public bool Disabled { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public static UserDto FromUser(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Disabled = user.Disabled,
                Reservations = user.Reservations != null
                 ? new List<Reservation>(user.Reservations)
                 : new List<Reservation>()
            };
        }

        //to User from UserDto, name of the func is ToUser
        public static User ToUser(UserDto userDto)
        {
            return new User()
            {
                Id = userDto.Id,
                Address = userDto.Address,
                Name = userDto.Name,
                Email = userDto.Email,
                Disabled = userDto.Disabled,
                Reservations = null
            };
        }
    }
}

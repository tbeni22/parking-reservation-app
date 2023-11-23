using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserManagement
    {
        Task<UserDto> CreateUser();

        //returns the updated user
        Task<UserDto> UpdateUser(UserDto user);
        Task DeleteUser(int id);
        Task<UserDto> GetUser(int id);
        Task<UserDto> GetUserByEmail(string email);
        Task<bool> UpdatePassword(string password);

    }
}

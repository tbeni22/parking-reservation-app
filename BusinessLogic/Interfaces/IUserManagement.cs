using BusinessLogic.DTOs;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserManagement
    {
        Task<UserDto> CreateUser(UserDto dto, Role role, string password = "ChangeThis#!4");

        //returns the updated user
        Task<UserDto> UpdateUser(UserDto user);
        Task DeleteUser(int id);
        Task<UserDto> GetUser(int id);
        Task<UserDto> GetUserByEmail(string email);
        Task<bool> UpdatePassword(UserDto dto, string password, string oldPassword = null);

        public Task SignOutAsync();
        Task AddUserRole(User user, string Role = Consts.Roles.User);

    }
}

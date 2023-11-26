using BusinessLogic.DTOs;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static BusinessLogic.Consts;


namespace BusinessLogic.Interfaces.Implementations
{
    public class UserService : IUserManagement
    {
        private ParkingContext context;

        public UserService(ParkingContext context)
        {
            this.context = context;
        }
        public async Task<UserDto> CreateUser(UserDto dto, string password, string role = Consts.Roles.User)
        {
            var dbUser = new User() { UserName = dto.Name, Email = dto.Email };
            var user = await userManager.CreateAsync(dbUser, password);
            if(user.Succeeded) {
                await userManager.AddToRoleAsync(dbUser, role);
                return dto;
            }
            else
                return null;
        }

        public async Task DeleteUser(int id)
        {
            var user = await context.User.FindAsync(id);
            if (user != null)
            {
                context.User.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await context.User.FindAsync(id);
            if (user != null)
            {
                return new UserDto() { Id = user.Id, Name = user.Name, Email = user.Email, Reservations = new List<Reservation>(user.Reservations) };
            }
            else return null;
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var querry = from users in context.User
                         where users.Email.Equals(email)
                         select users;

            var user = await querry.FirstOrDefaultAsync();
            if (user != null)
            {
                return new UserDto() { Email = user.Email, Id = user.Id, Name = user.Name, Reservations = new List<Reservation>(user.Reservations) };
            }
            return null;
        }


        public Task<bool> UpdatePassword(string password)
        {

            var user = await context.User.FindAsync(dto.Id);
            if (user != null) {
                if (oldPassword != null)
                {
                    var result = await userManager.ChangePasswordAsync(user, oldPassword, password);
                    return result.Succeeded;
                }
                else
                {
                    var result = await userManager.AddPasswordAsync(user, password);
                    return result.Succeeded;
                }

            }
            throw new UserNotfoundException();
        }

        public async Task<UserDto> UpdateUser(UserDto user)
        {
            var dbUser = await context.User.FindAsync(user.Id);
            if (dbUser != null)
            {
                dbUser.Name = user.Name;
                dbUser.Email = user.Email;
                dbUser.Reservations = new List<Reservation>(user.Reservations);
                await context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<bool> LoginAsync(LoginRequest loginRequest)
        {
            var result = await signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, false, false);
            return result.Succeeded;
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public class UserNotfoundException : Exception
        {

        }
    }
}

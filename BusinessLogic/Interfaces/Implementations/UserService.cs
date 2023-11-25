using BusinessLogic.DTOs;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogic.Interfaces.Implementations
{
    public class UserService : IUserManagement
    {
        private ParkingContext context;

        public UserService(ParkingContext context)
        {
            this.context = context;
        }
        public Task<UserDto> CreateUser()
        {
            var user = new User();
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
            throw new NotImplementedException();
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
    }
}

using BusinessLogic.DTOs;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static BusinessLogic.Consts;


namespace BusinessLogic.Interfaces.Implementations
{
    public class UserService : IUserManagement
    {
        private readonly ParkingContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IHttpContextAccessor accessor;

        public UserService(ParkingContext context, UserManager<User> userManager, SignInManager<User> signInManager,
            IHttpContextAccessor accessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.accessor = accessor;
        }
        public async Task<UserDto> CreateUser(UserDto dto, Role role, string password="ChangeThis#!4")
        {
            var user = Activator.CreateInstance<User>();
            user.SecurityStamp = Guid.NewGuid().ToString();
            
            user.UserName = dto.Email;
            user.Email = dto.Email;
            user.Name = dto.Name;
            user.Address = dto.Address;
            var result = await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role.ToString());
            if (result.Succeeded)
                return dto;
            else
                throw new System.Exception($"User creation failed. {result.Errors.First().Description}");
        }

        public async Task DeleteUser(int id)
        {

            var user = await context.User.FindAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await context.User.FindAsync(id);
            if (user != null)
            {
                return new UserDto() { Id = user.Id, Name = user.UserName, Email = user.Email, Reservations = new List<Reservation>(user.Reservations) };
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
                return new UserDto() { Email = user.Email, Id = user.Id, Name = user.UserName, Reservations = new List<Reservation>(user.Reservations) };
            }
            return null;
        }


        public async Task<bool> UpdatePassword(UserDto dto, string password, string oldPassword = null)
        {

            var user = await context.User.FindAsync(dto.Id);
            if (user != null)
            {
                if (oldPassword != null)
                {
                    var result = await userManager.ChangePasswordAsync(user, oldPassword, password);
                    await context.SaveChangesAsync();
                    return result.Succeeded;
                }
                else
                {
                    var result = await userManager.AddPasswordAsync(user, password);
                    await context.SaveChangesAsync();
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
                dbUser.UserName = user.Name;
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

        public async Task AddUserRole(User user, string role = "User")
        {
            await userManager.AddToRoleAsync(user, role);

        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            //retreive all users from db
            var users = await context.User.ToListAsync();
            //convert them to DTOs
            var dtos = users.Select(u => UserDto.FromUser(u)).ToList();
            //return the list
            return dtos;
        }

        public async Task<UserDto> GetCurrentUser()
        {
            var email = accessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var query = from user in context.User
                        where user.Email == email
                        select user;

            
            var current = await query.FirstOrDefaultAsync();
            if (current != null)
                return UserDto.FromUser(current);
            else return null;

        }

        public class UserNotfoundException : Exception
        {

        }
    }
}

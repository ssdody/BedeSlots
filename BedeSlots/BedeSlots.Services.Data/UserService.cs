using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class UserService : IUserService
    {
        private readonly BedeSlotsDbContext context;
        private readonly UserManager<User> userManager;

        public UserService(BedeSlotsDbContext bedeSlotsDbContext, UserManager<User> userManager)
        {
            this.context = bedeSlotsDbContext ?? throw new ServiceException("bedeSlotsDbContext can not be null!");
            this.userManager = userManager ?? throw new ServiceException("userManager can not be null!");
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            if (id == null)
            {
                throw new ServiceException("Id can not be null!");
            }

            var user = await this.context.Users
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new ServiceException($"There is no user with ID {id}");
            }

            return user;
        }

        public IQueryable<UserDto> GetAllUsers()
        {
            var usersRoles = context.UserRoles.Where(x => x.RoleId != null);

            var allRoles = this.context.Roles.ToList();
            var roleDictionary = new Dictionary<string, string>();

            foreach (var role in usersRoles)
            {
                roleDictionary.Add(role.UserId, allRoles.FirstOrDefault(x => x.Id == role.RoleId).Name);
            }

            var users = this.context
                .Users
                .Where(u => u.IsDeleted == false)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Firstname = u.FirstName,
                    Lastname = u.LastName,
                    Email = u.Email,
                    Birthdate = u.Birthdate,
                    Balance = u.Balance,
                    Currency = u.Currency,
                    Role = roleDictionary[u.Id]
                })
                .AsQueryable();

            return users;
        }

        public async Task<string> GetUserRoleIdAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
            var roleId = role.RoleId;

            return roleId;
        }

        public async Task<IdentityUserRole<string>> GetUserRoleAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);

            return role;
        }

        public async Task<string> GetUserRoleNameAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
            var roleId = role.RoleId;
            var roleName = this.context.Roles.SingleOrDefault(r => r.Id == roleId).Name;

            return roleName;
        }

        public async Task<ICollection<IdentityRole>> GetAllRolesAsync()
        {
            var roles = await this.context.Roles.ToListAsync();

            return roles;
        }

        public async Task<IdentityRole> EditUserRoleAsync(string userId, string newRoleId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            if (newRoleId == null)
            {
                throw new ServiceException("User role id can not be null!");
            }

            var userRole = this.context.UserRoles.FirstOrDefault(ur => ur.UserId == userId) ?? throw new ServiceException("UserRole not exist!");

            var newRole = await this.context.Roles.FirstOrDefaultAsync(r => r.Id == newRoleId) ?? throw new ServiceException("Role not exist!");

            var user = await this.GetUserByIdAsync(userId);

            var newIdentityRole = new IdentityUserRole<string>() { RoleId = newRole.Id, UserId = user.Id };

            this.context.UserRoles.Remove(userRole);
            await this.context.UserRoles.AddAsync(newIdentityRole);
            await this.context.SaveChangesAsync();

            return newRole;
        }

        public async Task<User> DeleteUserAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            var user = await this.GetUserByIdAsync(userId);
            user.IsDeleted = true;
            await this.context.SaveChangesAsync();

            return user;
        }

        public async Task<Currency> GetUserCurrencyByIdAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user.Currency;
        }
    }
}

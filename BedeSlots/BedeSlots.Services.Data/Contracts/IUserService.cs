using BedeSlots.Data.Models;
using BedeSlots.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string id);

        IQueryable<UserDto> GetAllUsers();

        Task<string> GetUserRoleIdAsync(string userId);

        Task<ICollection<IdentityRole>> GetAllRolesAsync();

        Task<IdentityRole> EditUserRoleAsync(string userId, string newRoleId);

        Task<IdentityUserRole<string>> GetUserRoleAsync(string userId);

        Task<string> GetUserRoleNameAsync(string userId);

        Task<User> DeleteUserAsync(string userId);
    }
}
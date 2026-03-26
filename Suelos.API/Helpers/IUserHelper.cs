using Microsoft.AspNetCore.Identity;
using Suelos.Shared.DTOs;
using Suelos.Shared.Entities;

namespace Suelos.API.Helpers;

public interface IUserHelper
{
    Task<User?> GetUserAsync(string email);
    Task<User?> GetUserByIdAsync(string userId);
    Task<IdentityResult> AddUserAsync(User user, string password);
    Task CheckRoleAsync(string roleName);
    Task AddUserToRoleAsync(User user, string roleName);
    Task<bool> IsUserInRoleAsync(User user, string roleName);
    Task<SignInResult> LoginAsync(LoginDTO model);
}

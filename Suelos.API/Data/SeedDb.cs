using Microsoft.EntityFrameworkCore;
using Suelos.API.Helpers;
using Suelos.Shared.Entities;
using Suelos.Shared.Enums;

namespace Suelos.API.Data;

public class SeedDb(AppDbContext context, IUserHelper userHelper)
{
    private readonly AppDbContext _context = context;
    private readonly IUserHelper _userHelper = userHelper;

    public async Task SeedDbAsync()
    {
        await _context.Database.MigrateAsync();
        await CheckRolesAsync();
        await CheckAdminUserAsync();
        await CheckTiposEnsayoAsync();
    }

    private async Task CheckRolesAsync()
    {
        await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
        await _userHelper.CheckRoleAsync(UserType.Analyst.ToString());
    }

    private async Task CheckAdminUserAsync()
    {
        const string email = "admin@suelos.local";
        var user = await _userHelper.GetUserAsync(email);
        if (user is not null)
        {
            await EnsureUserSetupAsync(user, UserType.Admin);
            return;
        }

        user = new User
        {
            Document = "1001",
            FirstName = "Admin",
            LastName = "Suelos",
            Email = email,
            UserName = email,
            Address = "Laboratorio principal",
            UserType = UserType.Admin
        };

        var result = await _userHelper.AddUserAsync(user, "123456");
        if (result.Succeeded)
        {
            await EnsureUserSetupAsync(user, UserType.Admin);
        }
    }

    private async Task EnsureUserSetupAsync(User user, UserType userType)
    {
        if (!await _userHelper.IsUserInRoleAsync(user, userType.ToString()))
        {
            await _userHelper.AddUserToRoleAsync(user, userType.ToString());
        }
    }

    private async Task CheckTiposEnsayoAsync()
    {
        if (_context.TiposEnsayo.Any())
        {
            return;
        }

        _context.TiposEnsayo.Add(new TipoEnsayo { Codigo = "GRAN", Nombre = "Granulometria" });
        _context.TiposEnsayo.Add(new TipoEnsayo { Codigo = "LLPL", Nombre = "Limites de Atterberg" });
        _context.TiposEnsayo.Add(new TipoEnsayo { Codigo = "HUME", Nombre = "Contenido de Humedad" });
        await _context.SaveChangesAsync();
    }
}

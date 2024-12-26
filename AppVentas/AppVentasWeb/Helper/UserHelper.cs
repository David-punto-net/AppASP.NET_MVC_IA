using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppVentasWeb.Helper
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContex _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(DataContex context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(AddUserViewModel model)
        {
            User user = new User
            {
                Direccion = model.Direccion,
                Rut = model.Rut,
                Email = model.Username,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                ImageId = model.ImageId,
                PhoneNumber = model.PhoneNumber,
                Ciudad = await _context.Ciudades.FindAsync(model.CiudadId),
                UserName = model.Username,
                UserType = model.UserType
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            User newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());
            return newUser;
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users
                        .Include(u => u.Ciudad)
                        .ThenInclude(c => c.Comuna)
                        .ThenInclude(s => s.Region)
                        .ThenInclude(s => s.Pais)
            .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _context.Users
                        .Include(u => u.Ciudad)
                        .ThenInclude(c => c.Comuna)
                        .ThenInclude(s => s.Region)
                        .ThenInclude(s => s.Pais)
                        .FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)

        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
            model.Username,model.Password,model.RememberMe,true);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
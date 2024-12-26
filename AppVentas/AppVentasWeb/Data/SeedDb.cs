using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Enum;
using AppVentasWeb.Helper;

namespace AppVentasWeb.Data
{
    public class SeedDb
    {
        private readonly DataContex _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContex context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckRolesAsync();
            await CheckUserAsync("1010", "Nehuen", "Torres", "nehuen@yopmail.com", "3223114620", "Calle Luna Calle Sol", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Nombres = firstName,
                    Apellidos = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Direccion = address,
                    Rut = document,
                    Ciudad = _context.Ciudades.FirstOrDefault(),
                    UserType = userType,
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }
    }
}
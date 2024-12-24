using AppVentasWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppVentasWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly DataContex _context;

        public UserController(DataContex context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(u => u.Ciudad)
                .ThenInclude(c => c.Comuna)
                .ThenInclude(cc => cc.Region)
                .ThenInclude(r => r.Pais)
                .ToListAsync());
        }






    }
}
using Proyecto_PAA.Helpers;
using Proyecto_PAA.Models;
using Proyecto_PAA.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Proyecto_PAA.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext db;

        public AuthController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            RegisterViewModel vm = new RegisterViewModel();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await db.Users.AnyAsync(x => x.Email == model.Email)) // select * from users where email = leonardo.norambuena@inacap.cl
                {
                    ViewData["ErrorMessage"] = "El mail ya se encuentra registrado";
                    return View();
                }
                var user = new User();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                byte[] psHash, psSalt;
                PasswordHelper.CreatePasswordHash(model.Password, out psHash, out psSalt);
                user.PasswordHash = psHash;
                user.PasswordSalt = psSalt;
                var role = await db.Roles.FirstOrDefaultAsync(x => x.RoleName == StringHelper.ROLE_CLIENT);
                if (role == null)
                {
                    TempData["ErrorMessage"] = "Imposible crear al usuario, el rol no existe";
                    return View();
                }
                db.Users.Add(user);
                await db.SaveChangesAsync(); // guarda los cambios
                var userRole = new UserRole {
                    UserId = user.UserId,
                    RoleId = role.RoleId
                };
                db.UserRoles.Add(userRole);
                await db.SaveChangesAsync();

                TempData["SuccessMessage"] = "Usuario creado correctamente";
                return RedirectToAction("Index", "Home");
            }
            
            

            return View(model);
        }

        

        public ActionResult Login()
        {
            OutOwin();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user != null && PasswordHelper.CheckPassword(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    await InitOwin(user);
                    TempData["SuccessMessage"] = $"Bienvenido {user.FullName}";
                    return RedirectToAction("Index", "Home"); //json
                }

                TempData["ErrorMessage"] = "Inicio de sesión incorrecto";
                return RedirectToAction("Login");

            }

            TempData["ErrorMessage"] = "Existieron errores de validación";
            return RedirectToAction("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut(LoginViewModel model)
        {
            OutOwin();
            return RedirectToAction("Login");
        }

        private void OutOwin()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;

            authManager.SignOut();
        }

        private async Task InitOwin(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.FullName), 
            };



            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var roles = await db.UserRoles.Where(x => x.UserId == user.UserId).Select(x => x.Role).ToListAsync();
            foreach (var role in roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));

            var context = Request.GetOwinContext();
            var authManager = context.Authentication;

            authManager.SignIn(identity);
        }
    }
}
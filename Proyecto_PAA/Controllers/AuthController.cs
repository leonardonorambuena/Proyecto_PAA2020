using Proyecto_PAA.Helpers;
using Proyecto_PAA.Models;
using Proyecto_PAA.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

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
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(x => x.Email == model.Email)) // select * from users where email = leonardo.norambuena@inacap.cl
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
                var role = db.Roles.FirstOrDefault(x => x.RoleName == StringHelper.ROLE_CLIENT);
                if (role == null)
                {
                    TempData["ErrorMessage"] = "Imposible crear al usuario, el rol no existe";
                    return View();
                }
                db.Users.Add(user);
                db.SaveChanges(); // guarda los cambios
                var userRole = new UserRole {
                    UserId = user.UserId,
                    RoleId = role.RoleId
                };
                db.UserRoles.Add(userRole);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Usuario creado correctamente";
                return RedirectToAction("Index", "Home");
            }
            
            

            return View(model);
        }

        

        public ActionResult Login()
        {
            Session["UserId"] = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(x => x.Email == model.Email);

                if (user != null && PasswordHelper.CheckPassword(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    InitOwin(user);
                    TempData["SuccessMessage"] = $"Bienvenido {user.FullName}";
                    return RedirectToAction("Index", "Home"); //json
                }

                TempData["ErrorMessage"] = "Inicio de sesión incorrecto";
                return RedirectToAction("Login");

            }

            TempData["ErrorMessage"] = "Existieron errores de validación";
            return RedirectToAction("Login");
        }

        private void InitOwin(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.FullName), 
            };

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var roles = db.UserRoles.Where(x => x.UserId == user.UserId).Select(x => x.Role);
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
            }

            var context = Request.GetOwinContext();
            var authManager = context.Authentication;

            authManager.SignIn(identity);
        }
    }
}
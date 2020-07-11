using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto_PAA.Models;
using Proyecto_PAA.ViewModels;

namespace Proyecto_PAA.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Budgets
        public async Task<ActionResult> Index()
        {
            var budgets = await db.Budgets
                .Include(x => x.Author)
                .OrderByDescending(x => x.BudgetId)
                .ToListAsync(); // SELECT * FROM[Proyecto_PAA].[dbo].[Budgets] as b inner join users as u on b.AuthorId = u.UserId order by b.BudgetId desc
            return View(budgets);
        }

        public async Task<ActionResult> Create()
        {
            var budget = await CreateBudget();
            return RedirectToAction("index", new {id = budget.BudgetId});
        }

        public async Task<ActionResult> Edit(int id)
        {
            var budget = await db.Budgets
                .Include(x => x.BudgetProducts)
                .FirstOrDefaultAsync(x => x.BudgetId == id);
            var vm = new BudgetViewModel();
            vm.Budget = budget;
            vm.Products = await db.Products.OrderBy(x => x.ProductName).ToListAsync();
            return View(vm);
        }

        protected async Task<Budget> CreateBudget()
        {
            Budget budget = new Budget
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AuthorId = User.Identity.GetUserId<int>(),
                BudgetState = BudgetState.Creado
            };

            db.Budgets.Add(budget);
            await db.SaveChangesAsync();

            return budget;
        }

        public async Task<ActionResult> AddProduct(BudgetViewModel model)
        {
            if (ModelState.IsValid)
            {
                // quantity <= stock siempre se debe hacer 
                var product = await db.Products.FindAsync(model.ProductId);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Producto no encontrado";
                    return RedirectToAction("Edit", new { id = model.BudgetId });
                }

                if (product.ProductStock < model.Quantity)
                {
                    TempData["ErrorMessage"] = $"No hay stock suficiente, nos quedan {product.ProductStock} unidades ";
                    return RedirectToAction("Edit", new { id = model.BudgetId });
                }
                var budgetProduct = new BudgetProduct
                {
                    ProductId = model.ProductId,
                    Price = product.ProductPrice,
                    BudgetId = model.BudgetId,
                    Quantity = model.Quantity
                    
                };
                db.BudgetProducts.Add(budgetProduct);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Se agrego correctamente el producto";
                return RedirectToAction("Edit", new { id = model.BudgetId });
            }

            TempData["ErrorMessage"] = "Faltan datos requeridos";
            return RedirectToAction("Edit", new {id = model.BudgetId});

        }


        protected override void Dispose(bool disposing)
        {
            // MVC
            // ado.net Select * from  ORM linq EntityFramework
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
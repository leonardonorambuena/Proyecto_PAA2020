using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Proyecto_PAA.Models;
using Proyecto_PAA.ViewModels;

namespace Proyecto_PAA.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private IQueryable<Product> GetQuery(string q, int? searchCategoryId)
        {
            var query = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(q))
                query = query.Where(x => x.ProductName.Contains(q));
            if (searchCategoryId != null)
                query = query.Where(x => x.CategoryId == searchCategoryId);

            return query;
        }

        public ActionResult GetProductsJson(string q, int? searchCategoryId)
        {
            IQueryable<Product> query = GetQuery(q, searchCategoryId);
            var products = query.Include(x => x.Category).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        // GET: Products
        public ActionResult Index(string q, int? searchCategoryId)
        {
            ProductsViewModel vm = new ProductsViewModel();

            var query = GetQuery(q, searchCategoryId);
            vm.Products = query.OrderBy(x => x.ProductName).ToList(); // select * from products order by productName

            vm.Categories = db.Categories.OrderBy(x => x.CategoryName).ToList();

            foreach (var product in vm.Products)
                product.Category = vm.Categories.FirstOrDefault(x => x.CategoryId == product.CategoryId);


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    ProductName = vm.ProductName,
                    ProductPrice = (int)vm.ProductPrice,
                    ProductStock = (int)vm.ProductStock,
                    CategoryId = vm.CategoryId
                };
                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            vm.Products = db.Products.OrderBy(x => x.ProductName).ToList();
            vm.Categories = db.Categories.OrderBy(x => x.CategoryName).ToList();

            foreach (var product in vm.Products)
            {
                product.Category = vm.Categories.FirstOrDefault(x => x.CategoryId == product.CategoryId);
            }

            return View("Index", vm);
        }

        public ActionResult Delete(int id)
        {

            var product = db.Products.Find(id);

            if (product == null)
            {
                TempData["ErrorMessage"] = "El identificador no fue encontrado";
                return RedirectToAction("Index");
            }

            db.Products.Remove(product);
            db.SaveChanges();

            TempData["SuccessMessage"] = $"El {product.ProductName} fue eliminado con éxito";

            return RedirectToAction("Index");

        }

        public ActionResult Update(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                TempData["ErrorMessage"] = "El identificador no fue encontrado";
                return RedirectToAction("Index");
            }
            ProductsViewModel vm = new ProductsViewModel();
            vm.ProductName = product.ProductName;
            vm.CategoryId = product.CategoryId;
            vm.ProductPrice = product.ProductPrice;
            vm.ProductStock = product.ProductStock;
            vm.ProductId = product.ProductId;

            vm.Categories = db.Categories.OrderBy(x => x.CategoryName).ToList();


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ProductsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var product = db.Products.Find(vm.ProductId);

                if (product == null)
                {
                    TempData["ErrorMessage"] = "El identificador no fue encontrado";
                    return RedirectToAction("Index");
                }

                product.ProductName = vm.ProductName;
                product.CategoryId = vm.CategoryId;
                product.ProductPrice = (int)vm.ProductPrice;
                product.ProductStock = (int)vm.ProductStock;

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = $"Producto {product.ProductName} actualizado correctamente";
                return RedirectToAction("Index");
            }

            vm.Categories = db.Categories.OrderBy(x => x.CategoryName).ToList();

            return View(vm);
        }

        /*
        public bool init()
        {
            return Session["UserId"] != null; // true, false
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
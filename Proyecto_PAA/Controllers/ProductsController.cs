using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_PAA.Models;
using Proyecto_PAA.ViewModels;

namespace Proyecto_PAA.Controllers
{
    public class ProductsController : Controller
    {
        public ProductsController()
        {
            
        }

        // GET: Products
        public ActionResult Index(string q)
        {
            if(init() == false)
                return RedirectToAction("Login", "Auth");

            ProductsViewModel vm = new ProductsViewModel();
            vm.Products = (List<Product>)Session["ProductList"];

            if (!string.IsNullOrEmpty(q))
                vm.Products = vm.Products
                                .Where(x => x.ProductName.ToUpper().Contains(q.ToUpper()))
                                .ToList();

            vm.Categories = (List<Category>) Session["CategoryList"];

            foreach (var product in vm.Products)
            {
                product.Category = vm.Categories.FirstOrDefault(x => x.CategoryId == product.CategoryId);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductsViewModel vm)
        {
            var products = (List<Product>) Session["ProductList"];
            var lastId = products.Select(x => x.ProductId).Max();


            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    ProductId =  lastId + 1,
                    ProductName = vm.ProductName,
                    ProductPrice = (int)vm.ProductPrice,
                    ProductStock = (int)vm.ProductStock,
                    CategoryId = vm.CategoryId
                };
                products.Add(product);
                Session["ProductList"] = products;

                return RedirectToAction("Index");
            }

            vm.Products = products;
            vm.Categories = (List<Category>)Session["CategoryList"];

            foreach (var product in vm.Products)
            {
                product.Category = vm.Categories.FirstOrDefault(x => x.CategoryId == product.CategoryId);
            }

            return View("Index", vm);
        }

        public ActionResult Delete(int id)
        {
            var products = (List<Product>)Session["ProductList"];

            var product = products.FirstOrDefault(x => x.ProductId == id);

            if (id == 0 || product == null)
            {
                TempData["ErrorMessage"] = "El identificador no fue encontrado";
                return RedirectToAction("Index");
            }

            products.Remove(product);
            Session["ProductList"] = products;

            return RedirectToAction("Index");

        }

        public ActionResult Update(int id)
        {
            var products = (List<Product>)Session["ProductList"];

            var product = products.FirstOrDefault(x => x.ProductId == id);

            if (id == 0 || product == null)
            {
                TempData["ErrorMessage"] = "El identificador no fue encontrado";
                return RedirectToAction("Index");
            }

            ProductsViewModel vm = new ProductsViewModel();

            vm.Categories = (List<Category>)Session["CategoryList"];
            vm.ProductId = product.ProductId;
            vm.ProductName = product.ProductName;
            vm.ProductPrice = product.ProductPrice;
            vm.ProductStock = product.ProductStock;
            vm.CategoryId = product.CategoryId;


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ProductsViewModel vm)
        {
            var products = (List<Product>)Session["ProductList"];

            var product = products.FirstOrDefault(x => x.ProductId == vm.ProductId);

            if (product == null)
            {
                TempData["ErrorMessage"] = "El identificador no fue encontrado";
                return RedirectToAction("Index");
            }

            product.ProductName = vm.ProductName;
            product.CategoryId = vm.CategoryId;
            product.ProductPrice = (int)vm.ProductPrice;
            product.ProductStock = (int)vm.ProductStock;

            Session["ProductList"] = products;

            return RedirectToAction("Index");
        }


        public bool init()
        {
            if (Session["UserId"] == null)
                return false;


            if (Session["ProductList"] == null)
            {
                List<Product> products = new List<Product>();
                List<Category> categories = new List<Category>();

                categories.Add(new Category
                {
                    CategoryId = 1,
                    CategoryName = "Video juegos"
                });
                categories.Add(new Category
                {
                    CategoryId = 2,
                    CategoryName = "Tecnología"
                });

                Session["CategoryList"] = categories;

                products.Add(new Product
                {
                    ProductId = 1,
                    ProductName = "Monitor Gamer",
                    CategoryId = 2,
                    ProductPrice = 150000,
                    ProductStock = 10,
                });
                products.Add(new Product
                {
                    ProductId = 2,
                    ProductName = "Call Of duty MW",
                    CategoryId = 1,
                    ProductPrice = 35000,
                    ProductStock = 15
                });

                products.Add(new Product
                {
                    ProductId = 3,
                    ProductName = "Mario",
                    CategoryId = 1,
                    ProductPrice = 45000,
                    ProductStock = 8
                });

                Session["ProductList"] = products;
            }

            return true;
        }
    }
}
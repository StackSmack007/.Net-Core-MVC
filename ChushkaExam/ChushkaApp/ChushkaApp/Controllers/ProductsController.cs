using AutoMapper;
using ChushkaApp.Data;
using ChushkaApp.DTOS.Products;
using ChushkaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ChushkaApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ChushkaDBContext db;
        private readonly IMapper mapper;

        public ProductsController(ChushkaDBContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public IActionResult Details(int id)
        {
            Product product = db.Products.Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == id);
            if (product is null)
            {
                return View("Error", new ErrorViewModel("Unfound Product"));
            }
            var dto = mapper.Map<ProductDetailsDTO>(product);
            return View(dto);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
           return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public IActionResult Create(ProductDetailsDTO dto)
        {
            if (ModelState.IsValid)
            {
            Product product = mapper.Map<Product>(dto);
            db.Products.Add(product);
            db.SaveChanges();
                return RedirectToAction("Details",new { id = product.Id });
            }
            return RedirectToAction("Index","Home");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Product product = db.Products.Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == id);
            if (product is null)
            {
                return View("Error", new ErrorViewModel("Unfound Product"));
            }
            var dto = mapper.Map<ProductDetailsDTO>(product);
           ViewBag.Old = dto;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public IActionResult Edit(ProductDetailsDTO dto)
        {
            Product product = db.Products.Where(x => !x.IsDeleted).First(x => x.Id == dto.Id);
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.Type = Enum.Parse<ProductType>(dto.Type);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = product.Id });
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Product product = db.Products.Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == id);
            if (product is null)
            {
                return View("Error", new ErrorViewModel("Unfound Product"));
            }
            var dto = mapper.Map<ProductDetailsDTO>(product);
            return View(dto);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(ProductDetailsDTO dto)
        {
            Product product = db.Products.Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == dto.Id);
            if (product is null)
            {
                return View("Error", new ErrorViewModel("Unfound Product"));
            }
            product.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}
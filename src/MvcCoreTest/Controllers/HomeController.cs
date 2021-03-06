﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcCoreTest.Entities;
using MvcCoreTest.Services;
using MvcCoreTest.ViewModels;

namespace MvcCoreTest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // comment to force push of new branch.
        private IRestaurantData _restaurantData;
        private IGreeter _greeter;

        public HomeController(IRestaurantData restaurantData, IGreeter greeter)
        {
            _restaurantData = restaurantData;
            _greeter = greeter;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            //this.HttpContext.
            //this.File()
            //return Content("Hello, from a controller!");

            var model = new HomePageViewModel();
            model.Restaurants = _restaurantData.GetAll();
            
            // The result automatically serializes
            // to JSON!
            //return new ObjectResult(model);
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var restaurant = _restaurantData.Get(id);

            if (restaurant == null)
            {
                return RedirectToAction("Index");
            }

            return View(restaurant);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var restaurant = _restaurantData.Get(id);

            if (restaurant == null)
            {
                return RedirectToAction("Index");
            }

            return View(restaurant);
        }

        [HttpPost]
        public IActionResult Edit(int id, RestaurantEditViewModel model)
        {
            var r = _restaurantData.Get(id);

            if (r == null && !ModelState.IsValid)
            {
                return View();
            }

            r.Name = model.Name;
            r.Cuisine = model.Cuisine;
            _restaurantData.Commit();

            return RedirectToAction("Details", new { id = r.Id });
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RestaurantEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var r = new Restaurant();
            r.Name = model.Name;
            r.Cuisine = model.Cuisine;

            _restaurantData.Add(r);
            _restaurantData.Commit();
            return RedirectToAction("Details", new {id = r.Id});
        }
    }
}
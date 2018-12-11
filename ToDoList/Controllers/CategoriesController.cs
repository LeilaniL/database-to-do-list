using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {

        [HttpGet("/categories")]
        public ActionResult Index()
        {
            List<Category> allCategories = Category.GetAll();
            return View(allCategories);
        }

        [HttpGet("/categories/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/categories")]
        public ActionResult Create(string categoryName)
        {
            Category newCategory = new Category(categoryName);
            newCategory.Save();
            List<Category> allCategories = Category.GetAll();
            return View("Index", allCategories);
        }

        [HttpGet("/categories/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(id);
            List<Item> categoryItems = selectedCategory.GetItems();
            List<Item> allItems = Item.GetAll();
            model.Add("category", selectedCategory);
            model.Add("categoryItems", categoryItems);
            model.Add("allItems", allItems);
            return View(model);
        }
        [HttpPost("/categories/{categoryId}/items/new")]
        public ActionResult AddItem(int categoryId, int itemId)
        {
            Category category = Category.Find(categoryId);
            Item item = Item.Find(itemId);
            category.AddItem(item);
            return RedirectToAction("Show", new { id = categoryId });
        }


        // This one creates new Items within a given Category, not new Categories:
        // [HttpPost("/categories/{categoryId}/items")]
        // public ActionResult Create(int categoryId, string itemDescription, DateTime itemDueDate)
        // {
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     Category foundCategory = Category.Find(categoryId);
        //     Item newItem = new Item(itemDescription, categoryId, itemDueDate);
        //     foundCategory.AddItem(newItem);
        //     newItem.Save();
        //     List<Item> categoryItems = foundCategory.GetItems();
        //     model.Add("items", categoryItems);
        //     model.Add("category", foundCategory);
        //     return View("Show", model);
        // }

    }
}
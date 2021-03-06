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
        Console.WriteLine("I am a categories controller");
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }
    
    [HttpGet("/categories/new")]
    public ActionResult New()
    {
        Console.WriteLine("Console Write Line works");
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
      Console.WriteLine(selectedCategory.GetId());
      List<Item> categoryItems = selectedCategory.GetItems();
      Console.WriteLine(categoryItems.Count);
      Console.WriteLine("Show controller works");
      model.Add("category", selectedCategory);
      model.Add("items", categoryItems);
      Console.WriteLine(categoryItems);
      return View(model);
    }
    
    // This one creates new Items within a given Category, not new Categories:
[HttpPost("/categories/{categoryId}/items")]
        public ActionResult Create(int categoryId, string itemDescription, DateTime itemDueDate)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category foundCategory = Category.Find(categoryId);
            Item newItem = new Item(itemDescription, categoryId, itemDueDate);
            foundCategory.AddItem(newItem);
            newItem.Save();
            List<Item> categoryItems = foundCategory.GetItems();
            model.Add("items", categoryItems);
            model.Add("category", foundCategory);
            return View("Show", model);
        }

  }
}
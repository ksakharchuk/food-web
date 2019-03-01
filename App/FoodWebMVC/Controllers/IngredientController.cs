using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodData;

namespace FoodWebMVC.Controllers
{
    public class IngredientController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Список ингредиентов";

            return View();
        }

        public ActionResult GetIngredients(string prefix)
        {
            IEnumerable<Ingredient> ingredients = Ingredient.GetIngredients(prefix);
            
            return PartialView(ingredients);
        }

        [HttpGet]
        public ActionResult EditIngredient(int id)
        {
            Ingredient ingredient;
            
            if (id == 0)
            {
                ingredient = new Ingredient();
                ingredient.ID = 0;
                ingredient.CategoryID = 1;
            }
            else
            {
                ingredient = Ingredient.GetIngredientById(id);
            }

            ViewBag.Ingredient = ingredient;

            ViewBag.Title = (id == 0 ? "Добавление ингредиента" : "Редактирование ингредиента");

            return View();
        }

        [HttpPost]
        public ActionResult EditIngredient(Ingredient ingredient)
        {
            ingredient.Save();
            return RedirectToAction("Index", "Ingredient");
        }

        //[HttpGet]
        //public JsonResult Index(string prefix)
        //{
        //    return Json(Ingredient.GetIngredients(), JsonRequestBehavior.AllowGet);
        //}
    }
}
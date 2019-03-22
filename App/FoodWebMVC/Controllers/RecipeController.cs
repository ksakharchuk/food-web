using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodData;


namespace FoodWebMVC.Controllers
{
    public class RecipeController : Controller
    {
        // GET: Recipe
        public ActionResult Index()
        {
            ViewBag.Title = "Список рецептов";
            return View();
        }


        public ActionResult GetRecipes(string prefix)
        {
            IEnumerable<Recipe> recipes = Recipe.GetRecipes(prefix);

            return PartialView(recipes);
        }

        [HttpGet]
        public ActionResult EditRecipe(int id)
        {
            Recipe recipe;

            if (id == 0)
            {
                recipe = new Recipe
                {
                    ID = 0
                };
            }
            else
            {
                recipe = Recipe.GetRecipeById(id);
            }

            ViewBag.Ingredient = recipe;

            ViewBag.Title = (id == 0 ? "Добавление рецепта" : "Редактирование рецепта");

            return View(recipe);
        }   

        [HttpPost]
        public ActionResult EditRecipe(Recipe recipe)
        {
            recipe.Save();
            return RedirectToAction("Index", "Recipe");
        }

        [HttpGet]
        public JsonResult SearchIngredient(string prefix)
        {
            return Json(Ingredient.GetIngredients(prefix), JsonRequestBehavior.AllowGet);
        }
    }
}
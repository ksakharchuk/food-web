using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support;
using FoodData;

namespace TestingData
{
    class Program
    {
        static void Main(string[] args)
        {

            IEnumerable<Ingredient> ingredients = Ingredient.GetIngredients();

        }
    }
}

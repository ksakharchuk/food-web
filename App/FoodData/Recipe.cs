using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace FoodData
{
    public class Recipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal TotalProteins { get; set; }
        public decimal TotalFats { get; set; }
        public decimal TotalCarbohydrates { get; set; }
        public decimal TotalEnergy { get; set; }
        public IList<RecipeIngredient> Ingredients { get; set; }

        /*
        private static readonly string[] Recipe_ColumnNames = new string[]
        {
            "id",
            "name",
            "total_weight",
            "total_proteins",
            "total_fats",
            "total_carbohydrates",
            "total_energy"
        };
        private static readonly Type[] Recipe_ColumnTypes = new Type[]
        {
            typeof(int),
            typeof(string),
            typeof(float),
            typeof(float),
            typeof(float),
            typeof(float),
            typeof(float)
        };
        */

        public static IEnumerable<Recipe> GetRecipes()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString))
            {
                string readSp = "getRecipesSP";
                return db.Query<Recipe>(readSp, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static IList<Recipe> GetRecipes(string namePattern)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString))
            {
                string readSp = "getRecipesSP";
                return db.Query<Recipe>(readSp, new { namePattern }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static Recipe GetRecipeById(int id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString))
            {
                string readSp = "getRecipeByIdSP";
                var results = db.QueryMultiple(readSp, new { id }, commandType: CommandType.StoredProcedure);
                var recipe = results.ReadSingle<Recipe>();
                var ingredients = results.Read<RecipeIngredient>();
                recipe.Ingredients = ingredients.ToList();
                return recipe;
            }
        }

        public void Save()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ingredient_id", typeof(int));
            dt.Columns.Add("ingredient_weight", typeof(float));

            int j = 0;
            foreach (RecipeIngredient ingredient in Ingredients)
            {
                dt.Rows.Add(dt.NewRow());
                dt.Rows[j][0] = ingredient.IngredientID;
                dt.Rows[j][1] = ingredient.IngredientWeight;
                j++;
            }

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString))
            {
                string saveSp = "saveRecipeSP";
                db.Execute(saveSp, new { recipe_id = ID, recipe_name = Name, recipe_total_weight = TotalWeight, ingredients = dt.AsTableValuedParameter("dbo.TRecipeIngredients") },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("id", typeof(int));
            dt.Rows.Add(dt.NewRow());
            dt.Rows[0][0] = ID;

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ids", SqlDbType.Structured)
            {
                Value = dt
            };
            Global.GlobalDataProvider.GetDataTable("deleteRecipesSP", CommandType.StoredProcedure, prms);
        }
    }
}

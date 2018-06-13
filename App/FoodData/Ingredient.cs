using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using sqlData;
using Dapper;
using Support;
using System.Configuration;

namespace FoodData
{
    public class Ingredient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public int CategoryID { get; set; }
        public decimal Proteins { get; set; }
        public decimal Fats { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Energy { get; set; }

        private static readonly string[] Ingredient_ColumnNames = new string[]
        { 
            "id",
            "barcode",
            "name", 
            "category_id", 
            "proteins", 
            "fats", 
            "carbohydrates", 
            "energy"
        };
        private static readonly Type[] Ingredient_ColumnTypes = new Type[] 
        {
            typeof(int),
            typeof(string),
            typeof(string), 
            typeof(int), 
            typeof(float), 
            typeof(float),
            typeof(float), 
            typeof(float)
        };


        public static DataTable GetIngredientsDT()
        {
            DataTable result;
            result = Global.GlobalDataProvider.GetDataTable("getIngredientsSP", CommandType.StoredProcedure);
            return result;
        }

        public static IEnumerable<Ingredient> GetIngredients()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString))
            {
                string readSp = "getIngredientsSP";
                return db.Query<Ingredient>(readSp, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static Ingredient GetIngredientById(int id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString))
            {
                string readSp = "getIngredientsSP";
                return db.Query<Ingredient>(readSp, new { Id = id }, commandType: CommandType.StoredProcedure).ToList().FirstOrDefault();
            }
        }

        public void Save()
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < Ingredient_ColumnNames.Length; i++)
                dt.Columns.Add(Ingredient_ColumnNames[i], Ingredient_ColumnTypes[i]);

            dt.Rows.Add(dt.NewRow());
            dt.Rows[0][0] = ID;
            dt.Rows[0][1] = Barcode;
            dt.Rows[0][2] = Name;
            dt.Rows[0][3] = CategoryID;
            dt.Rows[0][4] = Proteins;
            dt.Rows[0][5] = Fats;
            dt.Rows[0][6] = Carbohydrates;
            dt.Rows[0][7] = Energy;

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString))
            {
                string saveSp = "saveIngredientsSP";
                db.Execute(saveSp, new { ingredients = dt.AsTableValuedParameter("dbo.TIngredients") },
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
            prms[0] = new SqlParameter("@ids", SqlDbType.Structured);
            prms[0].Value = dt;
            Global.GlobalDataProvider.GetDataTable("deleteIngredientsSP", CommandType.StoredProcedure, prms);
        }

    }
}

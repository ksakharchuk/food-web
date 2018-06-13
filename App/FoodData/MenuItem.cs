using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using sqlData;

namespace FoodData
{
    public class MenuItem
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int IngredientID { get; set; }
        public decimal Weight { get; set; }

        private static readonly string[] MenuItem_ColumnNames = new string[]
        { 
            "id", 
            "menu_id",
            "ingredient_id", 
            "weight"
        };
        private static readonly Type[] MenuItem_ColumnTypes = new Type[] 
        {
            typeof(int), 
            typeof(int), 
            typeof(int), 
            typeof(decimal)
        };


        public static DataTable GetMenuItemsDT(int menuId)
        {
            DataTable result;
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@menu_id", SqlDbType.Int);
            prms[0].Value = menuId;
            result = Global.GlobalDataProvider.GetDataTable("getMenuItemsSP", CommandType.StoredProcedure, prms);
            return result;
        }

        public void Save()
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < MenuItem_ColumnNames.Length; i++)
                dt.Columns.Add(MenuItem_ColumnNames[i], MenuItem_ColumnTypes[i]);

            dt.Rows.Add(dt.NewRow());
            dt.Rows[0][0] = ID;
            dt.Rows[0][1] = MenuID;
            dt.Rows[0][2] = IngredientID;
            dt.Rows[0][3] = Weight;

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@menu_items", SqlDbType.Structured);
            prms[0].Value = dt;
            Global.GlobalDataProvider.ExecuteNonQuery("saveMenuItemsSP", CommandType.StoredProcedure, prms);
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
            Global.GlobalDataProvider.GetDataTable("deleteMenuItemsSP", CommandType.StoredProcedure, prms);
        }
    }
}

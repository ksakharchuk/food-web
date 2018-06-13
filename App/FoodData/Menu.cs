using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using sqlData;

namespace FoodData
{
    public class Menu
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MealPeriodID { get; set; }
        public DateTime ServiceDate { get; set; }

        private static readonly string[] Menu_ColumnNames = new string[]
        { 
            "id", 
            "name", 
            "meal_period_id", 
            "service_date"
        };
        private static readonly Type[] Menu_ColumnTypes = new Type[] 
        {
            typeof(int), 
            typeof(string), 
            typeof(int), 
            typeof(DateTime)
        };


        public static DataTable GetMenusDT()
        {
            DataTable result;
            result = Global.GlobalDataProvider.GetDataTable("getMenusSP", CommandType.StoredProcedure);
            return result;
        }

        public void Save()
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < Menu_ColumnNames.Length; i++)
                dt.Columns.Add(Menu_ColumnNames[i], Menu_ColumnTypes[i]);

            dt.Rows.Add(dt.NewRow());
            dt.Rows[0][0] = ID;
            dt.Rows[0][1] = Name;
            dt.Rows[0][2] = MealPeriodID;
            dt.Rows[0][3] = ServiceDate;

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@menus", SqlDbType.Structured);
            prms[0].Value = dt;
            Global.GlobalDataProvider.ExecuteNonQuery("saveMenusSP", CommandType.StoredProcedure, prms);
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
            Global.GlobalDataProvider.GetDataTable("deleteMenusSP", CommandType.StoredProcedure, prms);
        }
    }
}

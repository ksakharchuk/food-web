using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using sqlData;

namespace FoodData
{
    public class MealPeriod
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public static DataTable GetMealPeriodsDT()
        {
            DataTable result;
            result = Global.GlobalDataProvider.GetDataTable("getMealPeriodsSP", CommandType.StoredProcedure);
            return result;
        }
    }
}

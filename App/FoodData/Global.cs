using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sqlData;

namespace FoodData
{
    class Global
    {
        private static SqlDataProvider _GlobalDataProvider = null;

        public static SqlDataProvider GlobalDataProvider
        {
            get
            {
                if (_GlobalDataProvider != null) return _GlobalDataProvider;
                lock (typeof(Global))
                {
                    return _GlobalDataProvider = new SqlDataProvider();
                }
            }
        }
    }
}

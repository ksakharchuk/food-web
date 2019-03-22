using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DapperParameters;

namespace Parser
{

    class EvrooptItem
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Country { get; set; }
        public string Proteins { get; set; }
        public string Fats { get; set; }
        public string Carbohydrates { get; set; }
        public string Energy { get; set; }
        public string EnergyString { get; set; }
        public int ArticleId { get; set; }
        public string ArticlePrice { get; set; }

        public static void Save(List<EvrooptItem> items)
        {
            DataTable dt = new DataTable();

            var parameters = new DynamicParameters();
            parameters.AddTable("@items", "[stage].[TEvroopItems]", items);
            parameters.Add("source_id", 1, DbType.Int16);

            using (IDbConnection db = new SqlConnection("data source=.;Integrated Security=SSPI;Initial Catalog=food;"))
            {
                string saveSp = "[stage].[saveEvrooptItemsSP]";
                db.Execute(saveSp, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}

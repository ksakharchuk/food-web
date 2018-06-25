using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html;
using AngleSharp.Parser.Html;
using Dapper;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseGipermallArticle("691796");
        }
        static void ParseGipermallArticle(string id)
        {
            string url = "https://gipermall.by/catalog/item_" + id + ".html";
            string html = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                html = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            HtmlParser parser = new HtmlParser();
            //Just get the DOM representation
            var document = parser.Parse(html);

            string name = document.QuerySelector("div.template_1_columns > h1").TextContent;

            var descriptionElements = document.QuerySelectorAll("ul.description > li");
            string barcode = descriptionElements.Where(m => m.TextContent.Contains("Штрих-код:")).FirstOrDefault().GetElementsByTagName("span").FirstOrDefault().TextContent;

            if (!string.IsNullOrEmpty(barcode))
            {
                try
                {
                    string country = descriptionElements.Where(m => m.TextContent.Contains("Страна производства:")).FirstOrDefault().GetElementsByTagName("span").FirstOrDefault().TextContent;
                    string brand = descriptionElements.Where(m => m.TextContent.Contains("Торговая марка:")).FirstOrDefault().GetElementsByTagName("span").FirstOrDefault().TextContent;

                    string proteins = null;
                    string proteinString = document.QuerySelector("tr.property_307 > td.value").TextContent;
                    if (proteinString != null)
                        proteins = proteinString.Substring(0, proteinString.IndexOf(" "));

                    string fats = null;
                    string fatString = document.QuerySelector("tr.property_308 > td.value").TextContent;
                    if (fatString != null)
                        fats = fatString.Substring(0, fatString.IndexOf(" "));

                    string carbohydrates = null;
                    string carbohydratesString = document.QuerySelector("tr.property_317 > td.value").TextContent;
                    if (carbohydratesString != null)
                        carbohydrates = carbohydratesString.Substring(0, carbohydratesString.IndexOf(" "));

                    string energy = null;
                    string energyString = document.QuerySelector("tr.property_313 > td.value").TextContent;
                    if (energyString != null)
                        energy = energyString.Substring(0, energyString.IndexOf(" "));

                    const string sql = @"
                        merge into ingredients_stage as Target
                        using (select 
	                        @barcode AS barcode,
	                        @name AS name,
	                        @brand AS brand,
	                        @country AS country,
	                        @proteins AS proteins,
	                        @fats AS fats,
	                        @carbohydrates AS carbohydrates,
	                        @energy AS energy) as Source
                        on (Target.barcode = Source.barcode)
                        when matched then
                            update set 
		                        Target.name = Source.name,
		                        Target.brand = Source.brand,
		                        Target.country = Source.country,
		                        Target.proteins = Source.proteins,
		                        Target.fats = Source.fats,
		                        Target.carbohydrates = Source.carbohydrates,
		                        Target.energy = Source.energy
                        when not matched by Target then
                            insert (
	                        barcode,
	                        name,
	                        brand,
	                        country,
	                        proteins,
	                        fats,
	                        carbohydrates,
	                        energy) 
	                        values (barcode,
		                        name,
		                        brand,
		                        country,
		                        proteins,
		                        fats,
		                        carbohydrates,
		                        energy);";

                    using (IDbConnection db = new SqlConnection("data source=.;Integrated Security=SSPI;Initial Catalog=food;"))
                    {
                        db.Execute(sql, new { barcode, name, brand, country, proteins, fats, carbohydrates, energy });
                    }
                    //conn.Execute(sql, new { myId = 999, myValue = 123 })
                }
                catch
                {
                    Console.WriteLine("Cant parse product: " + id);
                }
            }
        }
    }
}

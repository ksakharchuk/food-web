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
            ParseGipermallCategory(400000272);
            //ParseGipermallArticle(769284);
        }

        static string GetResponseHtml(string url)
        {
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

            return html;
        }

        static void ParseGipermallCategory(int categoryId)
        {
            string categoryUrl = "https://gipermall.by/catalog/" + categoryId + ".html";

            int iteration = 2;

            string html = GetResponseHtml(categoryUrl);

            HtmlParser parser = new HtmlParser();
            var document = parser.Parse(html);
            var elements = document.QuerySelectorAll("div.products_card");
            while (elements.Count() != 0)
            {
                foreach (var element in elements)
                {
                    var productUrl = element.QuerySelector("a.fancy_ajax").Attributes["href"].Value;
                    ParseGipermallArticle(productUrl);
                }
                iteration++;
                html = GetResponseHtml(categoryUrl + "?lazy_steep=" + iteration);
                parser = new HtmlParser();
                document = parser.Parse(html);
                elements = document.QuerySelectorAll("div.products_card");
            }
            
        }

        static void ParseGipermallArticle(int id)
        {
            string url = "https://gipermall.by/catalog/item_" + id + ".html";
            ParseGipermallArticle(url);
        }

        static void ParseGipermallArticle(string url)
        {
            string html = GetResponseHtml(url);

            HtmlParser parser = new HtmlParser();
            var document = parser.Parse(html);

            string name = document.QuerySelector("div.template_1_columns > h1").TextContent;

            var descriptionElements = document.QuerySelectorAll("ul.description > li");
            string barcode = descriptionElements.FirstOrDefault(m => m.TextContent.Contains("Штрих-код:"))?.GetElementsByTagName("span").FirstOrDefault()?.TextContent;
            bool isGipermallId = Int32.TryParse(descriptionElements.FirstOrDefault(m => m.TextContent.Contains("Артикул:"))?.GetElementsByTagName("span").FirstOrDefault()?.TextContent, out var gipermallId);

            if (!string.IsNullOrEmpty(barcode) && isGipermallId)
            {
                try
                {
                    string country = null;
                    var counrtyElement = descriptionElements.FirstOrDefault(m => m.TextContent.Contains("Страна производства:"));
                    if (counrtyElement != null)
                         country = counrtyElement.GetElementsByTagName("span").FirstOrDefault()?.TextContent;

                    string brand = null;
                    var brandElement = descriptionElements.FirstOrDefault(m => m.TextContent.Contains("Торговая марка:"));
                    if (brandElement != null)
                        brand = brandElement.GetElementsByTagName("span").FirstOrDefault()?.TextContent;

                    string proteins = null;
                    var proteinsElement = document.QuerySelector("tr.property_307 > td.value");
                    string proteinsString = proteinsElement?.TextContent;
                    if (proteinsString != null)
                    {
                        proteinsString = proteinsString.ToLower();
                        proteins = proteinsString.IndexOf("г") >= 0 ? proteinsString.Substring(0, proteinsString.IndexOf("г")).Trim() : proteinsString;
                        proteins = proteins.Replace(',', '.');
                        proteins = Decimal.TryParse(proteins, out decimal prot) ? proteins : null;
                    }

                    string fats = null;
                    var fatsElement = document.QuerySelector("tr.property_308 > td.value");
                    string fatsString = fatsElement?.TextContent;
                    if (fatsString != null)
                    {
                        fatsString = fatsString.ToLower();
                        fats = fatsString.IndexOf("г") >= 0 ? fatsString.Substring(0, fatsString.IndexOf("г")).Trim() : fatsString;
                        fats = fats.Replace(',', '.');
                        fats = Decimal.TryParse(fats, out decimal prot) ? fats : null;
                    }

                    string carbohydrates = null;
                    var carbohydratesElement = document.QuerySelector("tr.property_317 > td.value");
                    string carbohydratesString = carbohydratesElement?.TextContent;
                    if (carbohydratesString != null)
                    {
                        carbohydratesString = carbohydratesString.ToLower();
                        carbohydrates = carbohydratesString.IndexOf("г") >= 0 ? carbohydratesString.Substring(0, carbohydratesString.IndexOf("г")).Trim() : carbohydratesString;
                        carbohydrates = carbohydrates.Replace(',', '.');
                        carbohydrates = Decimal.TryParse(carbohydrates, out decimal prot) ? carbohydrates : null;
                    }

                    string energy = null;
                    var energyElement = document.QuerySelector("tr.property_313 > td.value");
                    string energyString = energyElement?.TextContent;
                    if (energyString != null)
                    {
                        energyString = energyString.ToLower();
                        energy = energyString.IndexOf("ккал") >= 0 ? energyString.Substring(0, energyString.IndexOf("ккал")).Trim() : energyString;
                        energy = energy.Replace(',', '.');
                        energy = Decimal.TryParse(energy, out decimal prot) ? energy : null;
                    }

                    const string sql = @"
                        merge into stage.ingredients as Target
                        using (select 
	                        @barcode AS barcode,
	                        @name AS name,
	                        @brand AS brand,
	                        @country AS country,
	                        @proteins AS proteins,
	                        @fats AS fats,
	                        @carbohydrates AS carbohydrates,
	                        @energy AS energy,
	                        @gipermallId AS gipermall_id) as Source
                        on (Target.barcode = Source.barcode)
                        when matched then
                            update set 
		                        Target.name = Source.name,
		                        Target.brand = Source.brand,
		                        Target.country = Source.country,
		                        Target.proteins = Source.proteins,
		                        Target.fats = Source.fats,
		                        Target.carbohydrates = Source.carbohydrates,
		                        Target.energy = Source.energy,
		                        Target.gipermall_id = Source.gipermall_id
                        when not matched by Target then
                            insert (
	                        barcode,
	                        name,
	                        brand,
	                        country,
	                        proteins,
	                        fats,
	                        carbohydrates,
	                        energy,
                            gipermall_id) 
	                        values (barcode,
		                        name,
		                        brand,
		                        country,
		                        proteins,
		                        fats,
		                        carbohydrates,
		                        energy,
		                        gipermall_id);";

                    using (IDbConnection db = new SqlConnection("data source=.;Integrated Security=SSPI;Initial Catalog=food;"))
                    {
                        db.Execute(sql, new { barcode, name, brand, country, proteins, fats, carbohydrates, energy, gipermallId });
                    }
                    //conn.Execute(sql, new { myId = 999, myValue = 123 })
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant parse product: " + gipermallId + ". Error: " + ex.Message);
                }
            }
        }
    }
}

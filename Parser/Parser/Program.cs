using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Extensions;
using AngleSharp.Html;
using AngleSharp.Parser.Html;
using Dapper;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            //ParseGipermallCategory(400000276);
            //ParseGipermallArticle(36361);

            ParseEdostavkaCategory(400000172);

            Console.Write("Parsing finished. ");
            var name = Console.ReadLine();
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

            var priceElement = document.QuerySelector("div.price_byn > div.price");
            string priceStr = priceElement.Text().Replace("р","").Replace("к", "").TrimEnd(new char[2]{'.',' '});
            string gipermallPrice = null;
            gipermallPrice = Decimal.TryParse(priceStr, out decimal o) ? priceStr : null;

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
                        proteins = Decimal.TryParse(proteins, out o) ? proteins : null;
                    }

                    string fats = null;
                    var fatsElement = document.QuerySelector("tr.property_308 > td.value");
                    string fatsString = fatsElement?.TextContent;
                    if (fatsString != null)
                    {
                        fatsString = fatsString.ToLower();
                        fats = fatsString.IndexOf("г") >= 0 ? fatsString.Substring(0, fatsString.IndexOf("г")).Trim() : fatsString;
                        fats = fats.Replace(',', '.');
                        fats = Decimal.TryParse(fats, out o) ? fats : null;
                    }

                    string carbohydrates = null;
                    var carbohydratesElement = document.QuerySelector("tr.property_317 > td.value");
                    string carbohydratesString = carbohydratesElement?.TextContent;
                    if (carbohydratesString != null)
                    {
                        carbohydratesString = carbohydratesString.ToLower();
                        carbohydrates = carbohydratesString.IndexOf("г") >= 0 ? carbohydratesString.Substring(0, carbohydratesString.IndexOf("г")).Trim() : carbohydratesString;
                        carbohydrates = carbohydrates.Replace(',', '.');
                        carbohydrates = Decimal.TryParse(carbohydrates, out o) ? carbohydrates : null;
                    }

                    string energy = null;
                    var energyElement = document.QuerySelector("tr.property_313 > td.value");
                    string energyString = energyElement?.TextContent;
                    if (energyString != null)
                    {
                        energyString = energyString.ToLower();
                        energy = energyString.IndexOf("ккал") >= 0 ? energyString.Substring(0, energyString.IndexOf("ккал")).Trim() : energyString;
                        energy = energy.Replace(',', '.');
                        energy = Decimal.TryParse(energy, out o) ? energy : null;
                        if (energy == null)
                        {
                            energy = energyString.IndexOf("калл") >= 0 ? energyString.Substring(0, energyString.IndexOf("калл")).Trim() : energyString;
                            energy = energy.Replace(',', '.');
                            energy = Decimal.TryParse(energy, out o) ? energy : null;
                        }

                        if (energy == null)
                        {
                            energy = energyString.IndexOf("ккал") >= 0 ? energyString.Substring(energyString.IndexOf("/") + 1, energyString.IndexOf("ккал") - energyString.IndexOf("/") - 1).Trim() : energyString;
                            energy = energy.Replace(',', '.');
                            energy = Decimal.TryParse(energy, out o) ? energy : null;
                        }
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
	                        @gipermallId AS gipermall_id,
	                        @gipermallPrice AS gipermall_price) as Source
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
		                        Target.gipermall_id = Source.gipermall_id,
		                        Target.gipermall_price = Source.gipermall_price
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
                            gipermall_id,
                            gipermall_price) 
	                        values (barcode,
		                        name,
		                        brand,
		                        country,
		                        proteins,
		                        fats,
		                        carbohydrates,
		                        energy,
		                        gipermall_id,
                                gipermall_price);";

                    using (IDbConnection db = new SqlConnection("data source=.;Integrated Security=SSPI;Initial Catalog=food;"))
                    {
                        db.Execute(sql, new { barcode, name, brand, country, proteins, fats, carbohydrates, energy, gipermallId, gipermallPrice });
                    }
                    //conn.Execute(sql, new { myId = 999, myValue = 123 })
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant parse product: " + gipermallId + ". Error: " + ex.Message);
                }
            }
        }

        static void ParseEdostavkaCategory(int categoryId)
        {
            string categoryUrl = "https://e-dostavka.by/catalog/" + categoryId + ".html";

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
                    ParseEdostavkaArticle(productUrl);
                }
                iteration++;
                html = GetResponseHtml(categoryUrl + "?lazy_steep=" + iteration);
                parser = new HtmlParser();
                document = parser.Parse(html);
                elements = document.QuerySelectorAll("div.products_card");
            }

        }
        static void ParseEdostavkaArticle(string url)
        {
            string html = GetResponseHtml(url);

            HtmlParser parser = new HtmlParser();
            var document = parser.Parse(html);

            string name = document.QuerySelector("div.template_1_columns > h1").TextContent;

            var descriptionElements = document.QuerySelectorAll("ul.description > li");
            string barcode = descriptionElements.FirstOrDefault(m => m.TextContent.Contains("Штрих-код:"))?.GetElementsByTagName("span").FirstOrDefault()?.TextContent;
            bool isEdostavkaId = Int32.TryParse(descriptionElements.FirstOrDefault(m => m.TextContent.Contains("Артикул:"))?.GetElementsByTagName("span").FirstOrDefault()?.TextContent, out var edostavkaId);

            var priceElement = document.QuerySelector("div.price_byn > div.price");
            string priceStr = priceElement.Text().Replace("р", "").Replace("к", "").TrimEnd(new char[2] { '.', ' ' });
            string edostavkaPrice = null;
            edostavkaPrice = Decimal.TryParse(priceStr, out decimal o) ? priceStr : null;

            if (!string.IsNullOrEmpty(barcode) && isEdostavkaId)
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
                        proteins = Decimal.TryParse(proteins, out o) ? proteins : null;
                    }

                    string fats = null;
                    var fatsElement = document.QuerySelector("tr.property_308 > td.value");
                    string fatsString = fatsElement?.TextContent;
                    if (fatsString != null)
                    {
                        fatsString = fatsString.ToLower();
                        fats = fatsString.IndexOf("г") >= 0 ? fatsString.Substring(0, fatsString.IndexOf("г")).Trim() : fatsString;
                        fats = fats.Replace(',', '.');
                        fats = Decimal.TryParse(fats, out o) ? fats : null;
                    }

                    string carbohydrates = null;
                    var carbohydratesElement = document.QuerySelector("tr.property_317 > td.value");
                    string carbohydratesString = carbohydratesElement?.TextContent;
                    if (carbohydratesString != null)
                    {
                        carbohydratesString = carbohydratesString.ToLower();
                        carbohydrates = carbohydratesString.IndexOf("г") >= 0 ? carbohydratesString.Substring(0, carbohydratesString.IndexOf("г")).Trim() : carbohydratesString;
                        carbohydrates = carbohydrates.Replace(',', '.');
                        carbohydrates = Decimal.TryParse(carbohydrates, out o) ? carbohydrates : null;
                    }

                    string energy = null;
                    var energyElement = document.QuerySelector("tr.property_313 > td.value");
                    string energyString = energyElement?.TextContent;
                    if (energyString != null)
                    {
                        energyString = energyString.ToLower();
                        energy = energyString.IndexOf("ккал") >= 0 ? energyString.Substring(0, energyString.IndexOf("ккал")).Trim() : energyString;
                        energy = energy.Replace(',', '.');
                        energy = Decimal.TryParse(energy, out o) ? energy : null;
                        if (energy == null)
                        {
                            energy = energyString.IndexOf("калл") >= 0 ? energyString.Substring(0, energyString.IndexOf("калл")).Trim() : energyString;
                            energy = energy.Replace(',', '.');
                            energy = Decimal.TryParse(energy, out o) ? energy : null;
                        }

                        if (energy == null)
                        {
                            energy = energyString.IndexOf("ккал") >= 0 ? energyString.Substring(energyString.IndexOf("/") + 1, energyString.IndexOf("ккал") - energyString.IndexOf("/") - 1).Trim() : energyString;
                            energy = energy.Replace(',', '.');
                            energy = Decimal.TryParse(energy, out o) ? energy : null;
                        }
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
	                        @edostavkaId AS edostavka_id,
	                        @edostavkaPrice AS edostavka_price) as Source
                        on (Target.barcode = Source.barcode)
                        when matched then
                            update set 
		                        Target.name = ISNULL(Target.name, Source.name),
		                        Target.brand = ISNULL(Target.brand, Source.brand),
		                        Target.country = ISNULL(Target.country, Source.country),
		                        Target.proteins = ISNULL(Target.proteins, Source.proteins),
		                        Target.fats = ISNULL(Target.fats, Source.fats),
		                        Target.carbohydrates = ISNULL(Target.carbohydrates, Source.carbohydrates),
		                        Target.energy = ISNULL(Target.energy, Source.energy),
		                        Target.edostavka_id = Source.edostavka_id,
		                        Target.edostavka_price = Source.edostavka_price
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
                            edostavka_id,
                            edostavka_price) 
	                        values (barcode,
		                        name,
		                        brand,
		                        country,
		                        proteins,
		                        fats,
		                        carbohydrates,
		                        energy,
		                        edostavka_id,
                                edostavka_price);";

                    using (IDbConnection db = new SqlConnection("data source=.;Integrated Security=SSPI;Initial Catalog=food;"))
                    {
                        db.Execute(sql, new { barcode, name, brand, country, proteins, fats, carbohydrates, energy, edostavkaId, edostavkaPrice });
                    }
                    //conn.Execute(sql, new { myId = 999, myValue = 123 })
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant parse product: " + edostavkaId + ". Error: " + ex.Message);
                }
            }
        }

    }
}

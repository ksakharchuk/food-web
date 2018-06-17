using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html;
using AngleSharp.Parser.Html;

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
            //var name = document.QuerySelectorAll("div.description > li");
            var descriptionElement = document.QuerySelectorAll("ul.description > li");
            string barcode = descriptionElement.Where(m => m.TextContent.Contains("Штрих-код:")).FirstOrDefault().GetElementsByTagName("span").FirstOrDefault().TextContent;
            //var productData = document.QuerySelector("ul.description").QuerySelector("";
        }
    }
}

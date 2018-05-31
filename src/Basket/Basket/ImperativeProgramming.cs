using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Basket
{
    public class ImperativeProgramming
    {
        public static int CalculateBasketAmount(List<BasketLineArticle> basketTestBasketLineArticles)
        {
            var amountTotal = 0;
            foreach (var basketLineArticle in basketTestBasketLineArticles)
            {
                // Retrive article from database
                string id = basketLineArticle.Id;
                var article = GetArticleDatabase(id);
                // Calculate amount
                var amount = 0;
                int articlePrice = article.Price;
                switch (article.Category)
                {
                    case "food":
                        amount += articlePrice * 100 + articlePrice * 12;
                        break;
                    case "electronic":
                        amount += articlePrice * 100 + articlePrice * 20 + 4;
                        break;
                    case "desktop":
                        amount += articlePrice * 100 + articlePrice * 20;
                        break;
                }

                amountTotal += amount * basketLineArticle.Number;
            }

            return amountTotal;
        }

        private static ArticleDatabase GetArticleDatabase(string id)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var assemblyDirectory = Path.GetDirectoryName(path);
            var jsonPath = Path.Combine(assemblyDirectory, "article-database.json");
            IList<ArticleDatabase> articleDatabases =
                JsonConvert.DeserializeObject<List<ArticleDatabase>>(File.ReadAllText(jsonPath));
            var article = articleDatabases.First(articleDatabase => { return articleDatabase.Id == id; });
            return article;
        }
    }
}
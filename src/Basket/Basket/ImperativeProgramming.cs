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

                #if DEBUG
                var article = GetArticleDatabaseMock(id);
                #else
                #endif
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

        public static ArticleDatabase GetArticleDatabaseMock(string id)
        {
            switch (id)
            {
                case "1":
                    return new ArticleDatabase
                    {
                        Id = "1",
                        Price = 1,
                        Stock =
                            35,
                        Label = "Banana",
                        Category = "food"
                    };
                case "2":
                    return new ArticleDatabase
                    {
                        Id = "2",
                        Price = 500,
                        Stock = 20,
                        Label = "Fridge electrolux",
                        Category = "electronic"
                    };
                case "3":
                    return new ArticleDatabase
                    {
                        Id = "3",
                        Price = 49,
                        Stock =
                            68,
                        Label = "Chair",
                        Category = "desktop"
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
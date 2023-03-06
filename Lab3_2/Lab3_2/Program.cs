using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        // Шлях до папки з файлами JSON
        string pathToJsonFiles = @"C:\JsonFiles\";

        // Створення списку критеріїв для фільтрації продуктів
        var criteria = new List<Predicate<Product>>();
        criteria.Add(p => p.Price >= 10); // Фільтр за ціною
        criteria.Add(p => p.Category == "Fruit"); // Фільтр за категорією

        // Зчитування файлів JSON зі списками продуктів
        for (int i = 1; i <= 10; i++)
        {
            string filePath = Path.Combine(pathToJsonFiles, i.ToString() + ".json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(json);

                // Фільтрація та відображення продуктів
                Console.WriteLine($"Products in {i}.json:");
                foreach (var product in products)
                {
                    if (IsMatch(product, criteria))
                    {
                        Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
                    }
                }
            }
        }

        Console.ReadLine();
    }

    // Функція, яка застосовує передані критерії фільтрації до продукту
    static bool IsMatch(Product product, List<Predicate<Product>> criteria)
    {
        foreach (var criterion in criteria)
        {
            if (!criterion(product))
            {
                return false;
            }
        }
        return true;
    }
}

// Клас, який представляє продукт
class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public double Price { get; set; }
}
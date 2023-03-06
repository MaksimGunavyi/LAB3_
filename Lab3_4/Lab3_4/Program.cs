using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var filePaths = Directory.GetFiles("path/to/files", "*.txt");

        // Функція для токенізації тексту
        Func<string, IEnumerable<string>> tokenizeText = text =>
        {
            var separators = new[] { ' ', '\n', '\r', '.', ',', ';', '!', '?', '(', ')' };
            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        };

        // Функція для обчислення частоти слів
        Func<IEnumerable<string>, IDictionary<string, int>> calculateWordFrequencies = tokens =>
        {
            var frequencies = new Dictionary<string, int>();
            foreach (var token in tokens)
            {
                if (frequencies.ContainsKey(token))
                {
                    frequencies[token]++;
                }
                else
                {
                    frequencies[token] = 1;
                }
            }
            return frequencies;
        };

        // Делегат типу Action<IDictionary<string, int>> для відображення статистики
        Action<IDictionary<string, int>> displayStatistics = frequencies =>
        {
            foreach (var pair in frequencies.OrderByDescending(p => p.Value))
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        };

        // Читання файлів і обчислення статистики
        foreach (var filePath in filePaths)
        {
            var text = File.ReadAllText(filePath);
            var tokens = tokenizeText(text);
            var frequencies = calculateWordFrequencies(tokens);
            Console.WriteLine($"Statistics for {Path.GetFileName(filePath)}:");
            displayStatistics(frequencies);
            Console.WriteLine();
        }
    }
}
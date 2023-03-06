using System;
using System.IO;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = "transactions.csv";
        string outputFilePath = "result.csv";
        string dateFormat = "MM/dd/yyyy";

        Func<string, DateTime> getDate = line =>
        {
            string[] parts = line.Split(',');
            return DateTime.ParseExact(parts[0], dateFormat, CultureInfo.InvariantCulture);
        };

        Func<string, double> getAmount = line =>
        {
            string[] parts = line.Split(',');
            return double.Parse(parts[1], CultureInfo.InvariantCulture);
        };

        Action<DateTime, double> writeResult = (date, amount) =>
        {
            using (StreamWriter sw = new StreamWriter(outputFilePath, true))
            {
                sw.WriteLine($"{date.ToString(dateFormat)}, {amount.ToString("0.00", CultureInfo.InvariantCulture)}");
            }
        };

        int count = 0;
        DateTime currentDate = DateTime.MinValue;
        double dailyTotal = 0;

        using (StreamReader sr = new StreamReader(inputFilePath))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                DateTime date = getDate(line);
                double amount = getAmount(line);

                if (date != currentDate)
                {
                    if (dailyTotal > 0)
                    {
                        writeResult(currentDate, dailyTotal);
                    }
                    currentDate = date;
                    dailyTotal = 0;
                }

                dailyTotal += amount;
                count++;

                if (count % 10 == 0)
                {
                    writeResult(currentDate, dailyTotal);
                    dailyTotal = 0;
                }
            }
        }

        if (dailyTotal > 0)
        {
            writeResult(currentDate, dailyTotal);
        }
    }
}
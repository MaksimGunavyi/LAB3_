using System;
using System.Drawing;
using System.IO;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFolder = @"C:\input\";
            string outputFolder = @"C:\output\";

            // Список доступних операцій обробки зображень
            Func<Bitmap, Bitmap>[] imageProcessors = new Func<Bitmap, Bitmap>[]
            {
                InvertColors,
                Rotate90Degrees,
                Resize
            };

            // Завантаження зображень з папки вхідного каталогу
            string[] imageFiles = Directory.GetFiles(inputFolder, "*.jpg");

            // Перебір зображень та їх обробка за допомогою вибраних операцій
            foreach (string imageFile in imageFiles)
            {
                Bitmap originalImage = new Bitmap(imageFile);

                Console.WriteLine($"Processing image: {Path.GetFileName(imageFile)}");

                Bitmap processedImage = originalImage;
                foreach (Func<Bitmap, Bitmap> imageProcessor in imageProcessors)
                {
                    processedImage = imageProcessor(processedImage);
                }

                // Збереження обробленого зображення у вихідний каталог
                string outputFile = Path.Combine(outputFolder, Path.GetFileName(imageFile));
                processedImage.Save(outputFile);
            }

            Console.WriteLine("All images processed successfully.");
            Console.ReadKey();
        }

        // Функція, яка інвертує кольори зображення
        static Bitmap InvertColors(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = image.GetPixel(x, y);
                    Color invertedColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                    result.SetPixel(x, y, invertedColor);
                }
            }

            return result;
        }

        // Функція, яка повертає зображення на 90 градусів
        static Bitmap Rotate90Degrees(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Height, image.Width);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    result.SetPixel(y, x, image.GetPixel(x, y));
                }
            }

            result.RotateFlip(RotateFlipType.Rotate90FlipNone);

            return result;
        }

        // Функція, яка змінює розмір зображення до певних розмірів
        static Bitmap Resize(Bitmap image)
        {
            Bitmap result = new Bitmap(640, 480);

            Graphics graphics = Graphics.FromImage(result);
            graphics.DrawImage(image, 0, 0, 640, 480);

            return result;
        }
    }
}
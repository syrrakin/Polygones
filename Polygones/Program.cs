using System;
using System.Collections.Generic;
using System.Linq;

namespace Polygones
{
    class Program
    {
        static void Main()
        {
            //Видимая область
            var leftTopCorner = new Point(0, 100);
            var rigthBottomCorner = new Point(100, 0);
            var polygones = new List<Polygone>();  
            //Заполняем полигонами со случайными координатами и количеством сторон
            for (int i = 1; i <= 10; i++)
            {
                polygones.Add(CreateRandomPolygone(100, 4));
            }
            Console.WriteLine($"Левый верхний угол: {leftTopCorner}");
            Console.WriteLine($"Правый нижний угол: {rigthBottomCorner}");
            Console.WriteLine($"Все полигоны:");
            polygones.ForEach(p => Console.WriteLine(p));
            Console.WriteLine("Видимые полигоны:");
            var solver = new Solver(leftTopCorner.X, rigthBottomCorner.Y, rigthBottomCorner.X, leftTopCorner.Y);            
            solver.FindVisiblePolygones(polygones).ForEach(p => Console.WriteLine(p));
            Console.WriteLine("Нажмите Enter для завершения");
            Console.ReadLine();
        }

        static Polygone CreateRandomPolygone(int coordinateFactor, int maxSides)
        {
            var rand = new Random();
            int GetRandomCoordinate() => rand.Next(-coordinateFactor, coordinateFactor);
            var result = new Polygone();
            var sides = maxSides <= 2 ? 2 : rand.Next(2, maxSides + 1);
            for (int i = 1; i <= sides; i++)
            {
                result.Points.Add(new Point(GetRandomCoordinate(), GetRandomCoordinate()));
            }
            return result;
        }

        
    }
}

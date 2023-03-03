using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Polygones
{
    internal class Program
    {
        private static void Main()
        {
            var stopwatch = Stopwatch.StartNew();
            //Видимая область
            var leftTopCorner = new Point(-50.0, 50.0);
            var rigthBottomCorner = new Point(50.0, -50.0);            
            Console.WriteLine($"{stopwatch.Elapsed} Видимая область: {leftTopCorner} {rigthBottomCorner}");
            //Генерируем случайные полигоны
            var polygoneCount = 10000000;
            var coordinateFactor = 1000;
            var maxPointsCount = 4;            
            Console.WriteLine($"{stopwatch.Elapsed} Рабочая область: [{-coordinateFactor};{-coordinateFactor}] [{coordinateFactor};{coordinateFactor}]");
            var polygones = CreateRandomPolygones(polygoneCount, coordinateFactor, maxPointsCount);            
            Console.WriteLine($"{stopwatch.Elapsed} Сгенерировано полигонов: {polygoneCount}");
            var solver = new Solver(leftTopCorner.X, rigthBottomCorner.Y, rigthBottomCorner.X, leftTopCorner.Y);
            var visiblePolygones = solver.FindVisiblePolygones(polygones);
            Console.WriteLine($"{stopwatch.Elapsed} Видимых полигонов: {visiblePolygones.Count()}");
            Console.WriteLine("Нажмите Enter для завершения");
            Console.ReadLine();
        }

        private static IEnumerable<Polygone> CreateRandomPolygones(int polygoneCount, int coordinateFactor, int maxPointsCount)
        {
            var bag = new ConcurrentBag<Polygone>();
            int seed = Environment.TickCount;
            var threadRandom = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
            Parallel.For(0, polygoneCount, i => {
                bag.Add(CreateRandomPolygone(coordinateFactor, maxPointsCount, threadRandom.Value));
            });
            return bag;
        }

        private static Polygone CreateRandomPolygone(int coordinateFactor, int maxPointsCount, Random random)
        {
            var pointsCount = maxPointsCount <= 2 ? 2 : random.Next(2, maxPointsCount + 1);
            var polygone = new Polygone();
            for (int i = 0; i < pointsCount; i++)
            {
                polygone.Points.Add(new Point(GetRandomCoordinate(coordinateFactor, random), GetRandomCoordinate(coordinateFactor, random)));
            }            
            return polygone;
        }

        private static int GetRandomCoordinate(int coordinateFactor, Random random) => random.Next(-coordinateFactor, coordinateFactor);

    }
}

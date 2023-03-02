using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polygones
{
    internal class Solver
    {
        //Класс реализует алгоритм Коэна - Сазерленда для поиска видимых полигонов
        //Алгоритм упрощен, т.к. не требуется определять точки пересечения, достаточно только установить факт, что
        //одна из сторон полигона входит в видимую область
        //Коды областей
        const int left = 1;
        const int right = 2;
        const int bottom = 4;
        const int top = 8;
        //Координаты видимой области
        double minX;
        double minY;
        double maxX;        
        double maxY;

        public Solver(double minX, double minY, double maxX, double maxY)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
        }

        public List<Polygone> FindVisiblePolygones(List<Polygone> polygones)
        {
            return polygones
                .AsParallel()
                .Where(p =>
                {
                    //Принимаем, что полигон по условию задачи состоит как минимум из одного отрезка и не проверяем вырожденные случаи
                    for (int i = 0; i < p.Points.Count - 1; i++)
                    {
                        if (CheckLine(p.Points[i], p.Points[i + 1]))
                        {
                            return true;
                        }
                    }
                    //Если полигон не является отрезком - проверяем отрезок между первой и последней точкой
                    //Принимаем, что все полигоны замкнутые (в задании не указано обратного)
                    return p.Points.Count >= 2 ? CheckLine(p.Points[0], p.Points[p.Points.Count - 1]) : false;
                })
                .ToList();
        }

        bool CheckLine(Point point1, Point point2)
        {
            var x1 = point1.X;
            var y1 = point1.Y;
            var x2 = point2.X;
            var y2 = point2.Y;
            var code1 = GetCode(x1, y1);
            var code2 = GetCode(x2, y2);

            while(true)
            {
                //Хотя бы одна точка находится внутри видимой области
                if (code1  == 0 || code2 == 0) { return true; }
                //Обе точки находятся с одной стороны видимой области
                if ((code1 & code2) > 0) { return false; }

                //Определяем точку пересечения
                if ((code1 & top) != 0)
                {
                    x1 = x1 + (x2 - x1) * (maxY - y1) / (y2 - y1);
                    //В знаменателе ни при каких условиях не может быть нуль - точки гарантированно находятся в разных регионах по высоте,
                    //соответственно y2 - y1 всегда не равно 0. Поэтому проверку делать не нужно.
                    //Ниже аналогично
                    y1 = maxY;
                }
                else if ((code1 & bottom) != 0)
                {
                    x1 = x1 + (x2 - x1) * (minY - y1) / (y2 - y1);
                    y1 = minY;
                }
                else if ((code1 & right) != 0)
                {
                    y1 = y1 + (y2 - y1) * (maxX - x1) / (x2 - x1);
                    x1 = maxX;
                }
                else if ((code1 & left) != 0)
                {
                    y1 = y1 + (y2 - y1) * (minX - x1) / (x2 - x1);
                    x1 = minX;
                }
                code1 = GetCode(x1, y1);
            }
        }

        int GetCode(double x, double y)
        {
            return (x < minX ? left : 0) + (x > maxX ? right : 0) + (y < minY ? bottom : 0) + (y > maxY ? top : 0);
        }
    }
}

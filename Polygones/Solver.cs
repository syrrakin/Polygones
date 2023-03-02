using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polygones
{
    internal class Solver
    {
        //Класс реализует алгоритм Коэна - Сазерленда для поиска видимых полигонов
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
                if ((code1 | code2) == 0) { return true; }
                //Обе точки находятся с одной стороны видимой области
                if ((code1 & code2) > 0) { return false; }

                //Определяем точку пересечения
                var code_out = code1 != 0 ? code1 : code2;
                var x = 0.0;
                var y = 0.0;
                if ((code_out & top) != 0)
                {
                    x = x1 + (x2 - x1) * (maxY - y1) / (y2 - y1);
                    y = maxY;
                }
                else if ((code_out & bottom) != 0)
                {
                    x = x1 + (x2 - x1) * (minY - y1) / (y2 - y1);
                    y = minY;
                }
                else if ((code_out & right) != 0)
                {
                    y = y1 + (y2 - y1) * (maxX - x1) / (x2 - x1);
                    x = maxX;
                }
                else if ((code_out & left) != 0)
                {
                    y = y1 + (y2 - y1) * (minX - x1) / (x2 - x1);
                    x = minX;
                }

                //Перемещаем один из концов отрезка в точку пересечения
                if (code_out == code1)
                {
                    x1 = x;
                    y1 = y;
                    code1 = GetCode(x1, y1);
                }
                else
                {
                    x2 = x;
                    y2 = y;
                    code2 = GetCode(x2, y2);
                }
            }
        }

        int GetCode(double x, double y)
        {
            return (x < minX ? left : 0) + (x > maxX ? right : 0) + (y < minY ? bottom : 0) + (y > maxY ? top : 0);
        }
    }
}

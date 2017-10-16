using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorService
{
    class DataMaker
    {
        static decimal step = 0.001M;
        static int shingle = 3;

        decimal pos = 0;
        List<decimal> remnand = new List<decimal>();


        public IEnumerable<decimal[]> NormalizePoints(IEnumerable<decimal> points)
        {
            remnand.AddRange(points);

            if (remnand.Count < shingle)
            {
                return null;
            }

            var rez = new List<decimal[]>();

            for (int i = 0; i < remnand.Count - shingle + 1; i++)
            {
                decimal[] point = new decimal[shingle + 1];
                point[shingle] = pos;   // add step to last element for normalize
                pos += step;            // intrease pos
                for (int j = i; j < i + shingle; j++)
                {
                    point[j - i] = remnand[j];
                }
                rez.Add(point);
            }
            remnand.RemoveRange(0, remnand.Count - shingle + 1);
            return rez;
        }
    }
}

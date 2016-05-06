using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotterLib
{
    public static class VectorMath
    {
        public static IEnumerable<float> NormalizeVector(this IEnumerable<float> vector)
        {
            var min = vector.Min();
            var absMin = Math.Abs(min);
            if (min < 0)
            {
                vector = vector.Select(x => x + absMin);
            }

            var max = vector.Max();

            if (max > 0)
            {
                vector = vector.Select(x => x / max);
            }

            return vector;
        }
    }
}

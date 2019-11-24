using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMath
{
    public static class MathExtensions
    {
        /// <summary>
        /// Get the minimum of 4 Singles (floats)
        /// </summary>
        public static Single Min(this Single a, Single b, Single c, Single d)
        {
            Single min = Single.MaxValue;
            if (a < min)
            {
                min = a;
            }
            if (b < min)
            {
                min = b;
            }
            if (c < min)
            {
                min = c;
            }
            if (d < min)
            {
                min = d;
            }

            return min;
        }

        /// <summary>
        /// Get the maximum of 4 Singles (floats)
        /// </summary>
        public static Single Max(this Single a, Single b, Single c, Single d)
        {
            float max = float.MinValue;
            if (a > max)
            {
                max = a;
            }
            if (b > max)
            {
                max = b;
            }
            if (c > max)
            {
                max = c;
            }
            if (d > max)
            {
                max = d;
            }

            return max;
        }

        /// <summary>
        /// lerp between min and max by 'by'
        /// </summary>
        public static Single Lerp(this Single by, Single min, Single max)
        {            
            return min * (1 - by) + max * by;            
        }

        /// <summary>
        /// Float wrapper around System.Math.Sin
        /// </summary>
        public static Single Sin(this Single degrees)
        {
            return (Single) Math.Sin(degrees);
        }

        /// <summary>
        /// Float wrapper around System.Math.Cos
        /// </summary>
        public static Single Cos(this Single degrees)
        {
            return (Single)Math.Cos(degrees);
        }
    }
}

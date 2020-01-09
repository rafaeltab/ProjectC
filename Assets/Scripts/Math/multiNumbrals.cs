using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MyMath
{
    /// <summary>
    /// 3 floats in 1 class
    /// </summary>
    public class float3
    {
        public float3(float x,float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        /// <summary>
        /// Cast to a vector3
        /// </summary>
        /// <param name="input">the float3 to cast</param>
        /// <returns>the casted vector3</returns>
        public static Vector3 ToVector3(float3 input)
        {
            return new Vector3(input.x, input.y, input.z);
        }
    }

    /// <summary>
    /// 4 floats in 1 class
    /// </summary>
    public class float4
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }

        /// <summary>
        /// make a float 4
        /// </summary>
        public float4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// A float wrapper around the System.Math.Cos function
        /// </summary>
        /// <param name="n">The number to apply cos to</param>
        private float Cos(float n)
        {
            return (float)Math.Cos(n);
        }

        /// <summary>
        /// A float wrapper around the System.Math.Sin function
        /// </summary>
        /// <param name="n">The number to apply sin to</param>
        private float Sin(float n)
        {
            return (float)Math.Sin(n);
        }

        /// <summary>
        /// Use the w position to lerp between one and another float4 to get a float3
        /// </summary>
        /// <param name="other">other float4</param>
        /// <returns>result float3</returns>
        public float3 LerpByW(float4 other)
        {
            return LerpByW(this, other);
        }

        /// <summary>
        /// Use the w position to lerp between one and another float4 to get a float3
        /// </summary>
        /// /// <param name="a">the first float4</param>
        /// <param name="b">other float4</param>
        /// <returns>result float3</returns>
        public static float3 LerpByW(float4 a, float4 b)
        {
            float perc = 0.0f.Lerp(a.w, b.w);
            float x = b.x * perc + a.x * (1 - perc);
            float y = b.y * perc + a.y * (1 - perc);
            float z = b.z * perc + a.z * (1 - perc);

            return new float3(x, y, z);
        }

        /// <summary>
        /// - operator, subtract a float from a float4
        /// </summary>
        public static float4 operator -(float4 self, float other)
        {
            return new float4(self.x,self.y,self.z,self.w-other);
        }

        /// <summary>
        /// + operator, add a float to a float4
        /// </summary>
        public static float4 operator +(float4 self, float other)
        {
            return new float4(self.x, self.y, self.z, self.w + other);
        }

        /// <summary>
        /// * operator, add a float4 to a float4
        /// </summary>
        public static float4 operator +(float4 self, float4 other)
        {
            return new float4(self.x+other.x,self.y+other.y,self.z+other.z,self.w+other.w);
        }

        /// <summary>
        /// - operator, subtract a float4 from a float4
        /// </summary>
        public static float4 operator -(float4 self, float4 other)
        {
            return new float4(self.x - other.x, self.y - other.y, self.z - other.z, self.w - other.w);
        }
    }

    /// <summary>
    /// 3 ints in a struct
    /// </summary>
    public struct int3
    {
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
    }

    /// <summary>
    /// 4 ints in a class
    /// </summary>
    public class int4
    {
        /// <summary>
        /// Make an int4
        /// </summary>
        public int4(int a, int b, int c, int d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public int d { get; set; }
    }

    /// <summary>
    /// 6 floats in 1 class
    /// </summary>
    [Serializable]
    public class float6
    {
        /// <summary>
        /// make an empty float6
        /// </summary>
        public float6()
        {

        }

        /// <summary>
        /// Make a flaot6 with values
        /// </summary>
        public float6(float a, float b, float c, float d, float e, float f)
        {
            A = a; B = b; C = c; D = d; E = e; F = f;
        }

        /// <summary>
        /// Change the value of the indth value to val
        /// </summary>
        /// <param name="ind">Ind that defines what value to change</param>
        /// <param name="val">the value to put in</param>
        public void Change(int ind, float val)
        {
            switch (ind)
            {
                case 0:
                    A = val;
                    break;
                case 1:
                    B = val;
                    break;
                case 2:
                    C = val;
                    break;
                case 3:
                    D = val;
                    break;
                case 4:
                    E = val;
                    break;
                case 5:
                    F = val;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Add the value of the indth value to val
        /// </summary>
        /// <param name="ind">Ind that defines what value to add to</param>
        /// <param name="val">the value to add</param>
        public void Add(int ind, float val)
        {
            switch (ind)
            {
                case 0:
                    A += val;
                    break;
                case 1:
                    B += val;
                    break;
                case 2:
                    C += val;
                    break;
                case 3:
                    D += val;
                    break;
                case 4:
                    E += val;
                    break;
                case 5:
                    F += val;
                    break;
                default:
                    break;
            }
        }

        [Range(-1,1)]
        public float A;
        [Range(-1, 1)]
        public float B;
        [Range(-1, 1)]
        public float C;
        [Range(-1, 1)]
        public float D;
        [Range(-1, 1)]
        public float E;
        [Range(-1, 1)]
        public float F;
    }
}
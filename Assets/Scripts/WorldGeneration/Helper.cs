using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Helper
    {
        /// <summary>
        /// get all the non null values from 
        /// </summary>
        /// <param name="possibleNulls">Enumerable with possible null values (Nullable<Type>)</param>
        /// <returns>Non null values</returns>
        public static IEnumerable<T> GetNotNulls<T>(IEnumerable<T?> possibleNulls) where T : struct
        {
            List<T> nonNulls = new List<T>();
            foreach (T? t in possibleNulls)
            {
                if (t.HasValue)
                {
                    nonNulls.Add(t.Value);
                }
            }
            return nonNulls;
        }

        /// <summary>
        /// Make every element in enumerable nullable
        /// </summary>
        /// <param name="nonNullList">Enumerable with non nullable values</param>
        /// <returns>Enumerable with nullable values</returns>
        public static IEnumerable<T?> ToNullableEnumerable<T>(IEnumerable<T> nonNullList) where T : struct
        {
            List<T?> possbileNulls = new List<T?>();
            foreach (T t in nonNullList)
            {
                possbileNulls.Add(t);
            }
            return possbileNulls;
        }

        /// <summary>
        /// Order chunks by distance
        /// </summary>
        /// <param name="unordered">unordered chunks</param>
        /// <returns>ordered chunks</returns>
        public static IOrderedEnumerable<Vector3Int> Order(IEnumerable<Vector3Int> unordered,int size,Transform loadPoint)
        {
            return from chunk in unordered orderby Vector3.Distance(CastInt(chunk * size), loadPoint.position) ascending select chunk;
        }

        /// <summary>
        /// Create a Vector3 from a Vector3Int
        /// </summary>
        /// <param name="input">vector3int</param>
        /// <returns>cast vector3</returns>
        // TODO: move to helper class
        public static Vector3 CastInt(Vector3Int input)
        {
            return new Vector3(input.x, input.y, input.z);
        }

        /// <summary>
        /// Print the contents of an enumerable
        /// </summary>
        public static void PrintEnumerable<T>(IEnumerable<T> enumerable)
        {
            string s = "";
            foreach (T e in enumerable)
            {
                s += $"{e.ToString()}, ";
            }
            UnityEngine.Debug.Log($"Enumerable with size {enumerable.Count()} and contents: " + s);
        }

        /// <summary>
        /// Print the contents of an enumerable
        /// </summary>
        public static void PrintEnumerable<T>(IEnumerable<T> enumerable,string namePrefix)
        {
            string s = "";
            foreach (T e in enumerable)
            {
                s += $"{e.ToString()}, ";
            }
            UnityEngine.Debug.Log($"Enumerable named {namePrefix} with size {enumerable.Count()} and contents: " + s);
        }
    }
}

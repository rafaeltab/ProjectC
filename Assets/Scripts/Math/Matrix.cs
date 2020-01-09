using System;

namespace MyMath
{
    /// <summary>
    /// A template for all matrixes only saves the dimensions
    /// </summary>
    public class Matrix
    {
        public int dimension;
    }

    /// <summary>
    /// a 1 dimensional matrix
    /// </summary>
    public class Matrix1D : Matrix
    {
        public float[] values;
        /// <summary>
        /// Make a 1 dimensional matrix with a size of size
        /// </summary>
        public Matrix1D(int size)
        {
            values = new float[size];
            dimension = 1;
        }

        /// <summary>
        /// Make a 1 dimensional matrix with values
        /// </summary>
        public Matrix1D(float[] values)
        {
            this.values = values;
            dimension = 1;
        }

        /// <summary>
        /// * operator, Multiply a 1D Matrix by a float
        /// </summary>
        /// <param name="a">The 1D matrix</param>
        /// <param name="b">the float</param>
        /// <returns>result 1D Matrix</returns>
        public static Matrix1D operator *(Matrix1D a, float b)
        {
            Matrix1D result = new Matrix1D(a.values.Length);

            for (int i = 0; i < a.values.Length; i++)
            {
                result.values[i] = a.values[i] * b;
            }

            return result;
        }

        /// <summary>
        /// * operator, Multiply a 1D Matrix by a 1D Matrix
        /// </summary>
        /// <param name="a">The first 1D matrix</param>
        /// <param name="b">The second 1D matrix</param>
        /// <returns>result Matrix</returns>
        public static Matrix operator *(Matrix1D a, Matrix1D b)
        {
            if (a.values.Length == b.values.Length)
            {
                Matrix1D result = new Matrix1D(a.values.Length);

                for (int i = 0; i < a.values.Length; i++)
                {
                    result.values[i] = a.values[i] * b.values[i];
                }

                return result;
            }
            else
            {
                Matrix2D result = new Matrix2D(a.values.Length, b.values.Length);

                for (int x = 0; x < a.values.Length; x++)
                {
                    for (int y = 0; y < b.values.Length; y++)
                    {
                        result.values[x, y] = a.values[x] * b.values[y];
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Cast Matrix1D to Matrix2D
        /// </summary>
        public static explicit operator Matrix2D(Matrix1D a)
        {
            float[,] result = new float[a.values.Length, 1];

            for (int i = 0; i < a.values.Length; i++)
            {
                result[i, 0] = a.values[i];
            }

            return new Matrix2D(result);
        }
    }

    /// <summary>
    /// A 2 dimensional matrix
    /// </summary>
    public class Matrix2D : Matrix
    {
        public float[,] values;

        /// <summary>
        /// Make a 2 dimensional matrix with a width and height
        /// </summary>
        public Matrix2D(int width, int height)
        {
            values = new float[width, height];
        }

        /// <summary>
        /// Make a 2 dimensional matrix with values
        /// </summary>
        public Matrix2D(float[,] values)
        {
            this.values = values;
        }

        /// <summary>
        /// Multiply a 2D Matrix by a number
        /// </summary>
        /// <param name="a">The 2D Matrix</param>
        /// <param name="b">The number</param>
        /// <returns>The result 2D Matrix</returns>
        public static Matrix2D operator *(Matrix2D a, float b)
        {
            Matrix2D result = new Matrix2D(a.values);

            for (int x = 0; x < a.values.GetLength(0); x++)
            {
                for (int y = 0; y < a.values.GetLength(1); y++)
                {
                    result.values[x, y] *= b;
                }
            }

            return result;
        }

        /// <summary>
        /// * operator, Multiply 2D matrix by 1D matrix
        /// </summary>
        /// <param name="a">2D Matrix</param>
        /// <param name="b">1D Matrix</param>
        /// <returns>The result 2D Matrix</returns>
        public static Matrix2D operator *(Matrix2D a, Matrix1D b)
        {
            Matrix2D b2D = (Matrix2D)b;
            return Mult(a, b2D);
        }

        /// <summary>
        /// * operator, Multiply 1D Matrix by 2D Matrix
        /// </summary>
        /// <param name="a">The 1D Matrix</param>
        /// <param name="b">The 2D Matrix</param>
        /// <returns>The result 2D Matrix</returns>
        public static Matrix2D operator *(Matrix1D a, Matrix2D b)
        {
            Matrix2D a2D = (Matrix2D)a;
            return Mult(a2D, b);
        }

        /// <summary>
        /// * operator, Multiply 2D Matrix by 2D Matrix
        /// </summary>
        /// <param name="a">2D Matrix a</param>
        /// <param name="b">2D Matrix b</param>
        /// <returns>The result 2D Matrix</returns>
        public static Matrix2D operator *(Matrix2D a, Matrix2D b)
        {
            return Mult(a, b);
        }

        /// <summary>
        /// Multiply 2 2D Matrices
        /// </summary>
        public static Matrix2D Mult(Matrix2D a, Matrix2D b)
        {
            float[,] A = a.values;
            float[,] B = b.values;

            int height = A.GetLength(1);
            int width = B.GetLength(0);

            if (A.GetLength(0) != B.GetLength(1))
            {
                throw new ArgumentException($"Number of columns of matrix A must match number of row of matrix B. Currently a is {A.GetLength(0)} and b is {B.GetLength(1)}");
            }
            float[,] result = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result[x, y] = 0;
                    for (int k = 0; k < A.GetLength(0); k++)
                    {
                        result[x, y] += A[k, y] * B[x, k];
                    }
                }
            }

            return new Matrix2D(result);
        }

        /// <summary>
        /// make a 'good' ToString Method
        /// </summary>
        /// <returns>string representation of the Matrix</returns>
        public override string ToString()
        {
            string s = "[[";
            for (int x = 0; x < values.GetLength(0); x++)
            {
                for (int y = 0; y < values.GetLength(1); y++)
                {
                    s += values[x, y];
                    if (y != values.GetLength(1)-1)
                    {
                        s += ",";
                    }
                    s += "]";
                    if (x != values.GetLength(0) - 1)
                    {
                        s += ",[";
                    }
                }
            }
            s += "]";
            return s;
        }
        
        /// <summary>
        /// Cast a 2D Matrix to 1D Matrix
        /// </summary>
        public static explicit operator Matrix1D(Matrix2D a)
        {
            if (a.values.GetLength(0) == 1)
            {
                float[] result = new float[a.values.GetLength(1)];
                for (int i = 0; i < a.values.GetLength(1); i++)
                {
                    result[i] = a.values[0, i];
                }
                return new Matrix1D(result);
            }
            else if (a.values.GetLength(1) == 1)
            {
                float[] result = new float[a.values.GetLength(0)];
                for (int i = 0; i < a.values.GetLength(0); i++)
                {
                    result[i] = a.values[i, 0];
                }
                return new Matrix1D(result);
            }
            else
            {
                throw new InvalidCastException("Can't cast a Matrix2D with 2+ rows and collumns to a Matrix1D");
            }
        }
    }
}
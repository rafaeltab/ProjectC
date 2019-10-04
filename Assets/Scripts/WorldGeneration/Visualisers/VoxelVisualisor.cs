using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class VoxelVisualisor : IMeshVisualiser
    {
        private float threshold = 0.5f;

        /// <summary>
        /// Constructor, set threshold
        /// </summary>
        /// <param name="thres"></param>
        public VoxelVisualisor(float thres)
        {
            threshold = thres;
        }

        public int sizeOffset()
        {
            return 0;
        }

        /// <summary>
        /// Create a mesh from a float array
        /// </summary>
        /// <param name="values">float array</param>
        /// <returns>generated mesh</returns>
        public Mesh Visualize(float[,,] values, Mesh template, int size)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        if (Enabled(values[x, y, z]))
                        {
                            DoFace(x, y, z, values, vertices, triangles, uvs);
                        }
                    }
                }
            }

            template.Clear();
            template.vertices = vertices.ToArray();
            template.triangles = triangles.ToArray();
            template.RecalculateNormals();

            return template;
        }

        /// <summary>
        /// Generate a single face
        /// </summary>
        private void DoFace(int x, int y, int z, float[,,] values, List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
        {
            Vector3 current = new Vector3(x, y, z);
            #region x
            if (values.GetLength(0) == x + 1)
            {
                GeneratorHelper.CreateFace(GeneratorHelper.drawRight(), vertices, triangles, uvs, current);
            }
            else if (!Enabled(values[x + 1, y, z]))
            {
                GeneratorHelper.CreateFace(GeneratorHelper.drawRight(), vertices, triangles, uvs, current);
            }

            if (x == 0)
            {
                GeneratorHelper.CreateFace(GeneratorHelper.drawLeft(), vertices, triangles, uvs, current);
            }
            else if (!Enabled(values[x - 1, y, z]))
            {
                GeneratorHelper.CreateFace(GeneratorHelper.drawLeft(), vertices, triangles, uvs, current);
            }
            #endregion x

            #region y
            if (values.GetLength(1) == y + 1)
            {

                GeneratorHelper.CreateFace(GeneratorHelper.drawUp(), vertices, triangles, uvs, current);
            }
            else if (!Enabled(values[x, y + 1, z]))
            {

                GeneratorHelper.CreateFace(GeneratorHelper.drawUp(), vertices, triangles, uvs, current);
            }

            if (y == 0)
            {

                GeneratorHelper.CreateFace(GeneratorHelper.drawDown(), vertices, triangles, uvs, current);
            }
            else if (!Enabled(values[x, y - 1, z]))
            {

                GeneratorHelper.CreateFace(GeneratorHelper.drawDown(), vertices, triangles, uvs, current);
            }
            #endregion y

            #region z
            if (values.GetLength(2) == z + 1)
            {
                GeneratorHelper.CreateFace(GeneratorHelper.drawFar(), vertices, triangles, uvs, current);
            }
            else if (!Enabled(values[x, y, z + 1]))
            {
                GeneratorHelper.CreateFace(GeneratorHelper.drawFar(), vertices, triangles, uvs, current);
            }

            if (z == 0)
            {
                GeneratorHelper.CreateFace(GeneratorHelper.drawNear(), vertices, triangles, uvs, current);
            }
            else if (!Enabled(values[x, y, z - 1]))
            {
                GeneratorHelper.CreateFace(GeneratorHelper.drawNear(), vertices, triangles, uvs, current);
            }
            #endregion z
        }

        /// <summary>
        /// Check if a value is enabled
        /// </summary>
        private bool Enabled(float val)
        {
            return val >= threshold;
        }
    }
}

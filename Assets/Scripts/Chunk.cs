using NoiseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Chunk
    {
        public readonly Vector3Int position;
        public readonly float trength;
        private Mesh mesh = new Mesh();
        private readonly int size;
        private readonly float noiseScale;
        private readonly long seed;
        private readonly float threshold;
        private float[,,] vals;

        private bool generated = false;

        /// <summary>
        /// Create a non existing chunk
        /// </summary>
        /// <param name="pos">The Chunks position</param>
        /// <param name="trength">The chunks 4th dimension position</param>
        /// <param name="size">The size of the chunk</param>
        public Chunk(Vector3Int pos,float trength,long seed,float threshold, int size = 16, float noiseScale = 1f)
        {
            position = pos;
            this.trength = trength;
            this.size = size;
            if (noiseScale<=0)
            {
                throw new ArgumentException("noiseScale is out of bounds, it is less then or equal to 0");
            }
            this.noiseScale = noiseScale;
            this.seed = seed;
            this.threshold = threshold;
        }

        /// <summary>
        /// Create a chunk from a saved file. Not implemented
        /// </summary>
        /// <param name="path">The path to the file</param>
        public Chunk(string path)
        {
            //load chunk from saved file
        }

        /// <summary>
        /// Pre-Generate the chunk
        /// </summary>
        public void Generate()
        {
            vals = Gen();
            CreateMesh(vals);                      

            generated = true;
        }

        /// <summary>
        /// Generate the chunk and put it into a gameobject
        /// </summary>
        /// <param name="go">GameObject to put the mesh in</param>
        public void Display(GameObject go)
        {
            MeshFilter filter = go.GetComponent<MeshFilter>();
            if(filter == null){
                filter = go.AddComponent<MeshFilter>();
            }

            if (!generated)
            {
                Generate();
            }
            MeshCollider coll = go.GetComponent<MeshCollider>();
            if(coll != null){
                coll.sharedMesh = mesh;
            }
            filter.mesh = mesh;
            go.SetActive(true);
            go.name = $"Chunk {position.x}, {position.y}, {position.z}";
            go.transform.position = position*size;
        }

        /// <summary>
        /// Create the values array
        /// </summary>
        /// <returns>Float values</returns>
        private float[,,] Gen()
        {
            SimplexNoise sn = new SimplexNoise(seed);
            float[,,] values = new float[size, size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        float sampleX = (x + position.x * size) * noiseScale;
                        float sampleY = (y + position.y * size) * noiseScale;
                        float sampleZ = (z + position.z * size) * noiseScale;

                        float noiseValue = ((float)sn.Evaluate(sampleX, sampleY, sampleZ, trength)) / 2 + 0.5f;
                        values[x, y, z] = noiseValue;
                    }
                }
            }

            return values;
        }

        /// <summary>
        /// Create a mesh from a float array
        /// </summary>
        /// <param name="values">float array</param>
        /// <returns>generated mesh</returns>
        private void CreateMesh(float[,,] values)
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

            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
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
        public bool Enabled(float val)
        {
            return val >= threshold;
        }
    }
}

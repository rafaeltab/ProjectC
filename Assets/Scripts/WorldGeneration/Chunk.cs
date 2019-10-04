using Assets.Scripts.Visualisers;
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
        private int size;
        private readonly float noiseScale;
        private readonly long seed;
        private readonly float threshold;
        private float[,,] vals;
        private IMeshVisualiser mv;

        private bool generated = false;

        /// <summary>
        /// Create a non existing chunk
        /// </summary>
        /// <param name="pos">The Chunks position</param>
        /// <param name="trength">The chunks 4th dimension position</param>
        /// <param name="size">The size of the chunk</param>
        public Chunk(Vector3Int pos,float trength,long seed,float threshold, int size = 16, float noiseScale = 1f)
        {
            mv = new MarchingVisualiser(threshold);
            this.seed = seed+mv.sizeOffset();
            position = pos;
            this.trength = trength;
            this.size = size;
            if (noiseScale<=0)
            {
                throw new ArgumentException("noiseScale is out of bounds, it is less then or equal to 0");
            }
            this.noiseScale = noiseScale;
            
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
            mesh = mv.Visualize(Gen(size),mesh,size);
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
            go.transform.position = position*(size);
        }

        /// <summary>
        /// Create the values array
        /// </summary>
        /// <returns>Float values</returns>
        private float[,,] Gen(int size)
        {
            SimplexNoise sn = new SimplexNoise(seed);
            float[,,] values = new float[size + mv.sizeOffset(), size + mv.sizeOffset(), size + mv.sizeOffset()];
            for (int x = 0; x < size+mv.sizeOffset(); x++)
            {
                for (int y = 0; y < size + mv.sizeOffset(); y++)
                {
                    for (int z = 0; z < size + mv.sizeOffset(); z++)
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
    }
}

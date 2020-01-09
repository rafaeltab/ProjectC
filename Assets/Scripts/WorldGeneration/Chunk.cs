using Assets.Scripts.Visualisers;
using NoiseTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// A chunk of the world
    /// </summary>
    public class Chunk
    {
        public readonly Vector3Int position;
        public readonly float trength;
        private Mesh mesh = new Mesh();
        public int size;
        public readonly float noiseScale;
        public readonly long seed;
        public readonly float threshold;
        public float[,,] vals;
        public IMeshVisualiser mv;
        public bool generated = false;

        /// <summary>
        /// Create a non existing chunk
        /// </summary>
        /// <param name="pos">The Chunks position</param>
        /// <param name="trength">The chunks 4th dimension position</param>
        /// <param name="size">The size of the chunk</param>
        /// <param name="compute">ComputeShader used by the gpu visualizor</param>
        /// <param name="generator">The monobehaviour that called this function</param>
        /// <param name="visualizer">What visualizor to use</param>
        public Chunk(Vector3Int pos,float trength,long seed,float threshold, ComputeShader compute, MonoBehaviour generator,MeshGenModel.Visualizer visualizer, int size = 16, float noiseScale = 1f)
        {
            switch (visualizer)
            {
                case MeshGenModel.Visualizer.VOXEL:
                    mv = new VoxelVisualisor(threshold);
                    break;
                case MeshGenModel.Visualizer.MARCHING:
                    mv = new MarchingVisualiser(threshold);
                    break;
                case MeshGenModel.Visualizer.GPU_MARCHING:
                    mv = new MarchingComputeVisualiser(threshold, compute, generator);
                    break;
                default:
                    break;
            }

            this.seed = seed;
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
        /// Pre-Generate the chunk
        /// </summary>
        public void Generate()
        {
            if (!generated) {
                vals = Gen(size);
            }
            if(mesh.vertices.Count() <= 1){
                mesh = mv.Visualize(vals, mesh, size);
            }
            
            generated = true;
            
        }

        /// <summary>
        /// Make a file name out of all paramaters for a chunk
        /// </summary>
        private static string InfoToFileName(Vector3Int pos, float trength, long seed, float threshold, int size = 16, float noiseScale = 1f)
        {
            return $"{pos.x} {pos.y} {pos.z} {trength} {seed} {threshold} {size} {noiseScale}.chunk";
        }

        /// <summary>
        /// Save the current chunk in a file
        /// </summary>
        public void Save()
        {
            string filename = InfoToFileName(position, trength, seed, threshold, size, noiseScale);

            byte[] serialized = ChunkSerializer.Serialize(this);
            File.WriteAllBytes(@"save\" + filename,serialized);
        }

        /// <summary>
        /// Attempt to load this chunk from a file
        /// </summary>
        public static Chunk TryLoadFromFile(Vector3Int pos, float trength, long seed, float threshold, ComputeShader compute, MonoBehaviour generator, MeshGenModel.Visualizer visualizer, int size = 16, float noiseScale = 1f)
        {
            if(!Directory.Exists(@"save\")){
                Directory.CreateDirectory(@"save\");
            }

            string filename = InfoToFileName(pos, trength, seed, threshold, size, noiseScale);

            if(File.Exists(@"save\" + filename)){
                return ChunkSerializer.Deserialize(File.ReadAllBytes(@"save\" + filename),compute,generator,visualizer);
            }
            else
            {
                return new Chunk(pos,trength,seed,threshold,compute,generator,visualizer,size,noiseScale);
            }
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

            
            Generate();
            
            MeshCollider coll = go.GetComponent<MeshCollider>();
            if(coll != null){
                try
                {
                    coll.sharedMesh = mesh;
                }
                catch (Exception e)
                {
                    Debug.Log(position);
                }
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

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Visualisers
{
    public class MarchingComputeVisualiser : IMeshVisualiser
    {
        private float threshold;
        private ComputeShader compute;
        private MonoBehaviour generator;

        private static ComputeBuffer triangleBuff;
        private static ComputeBuffer floatMapBuff;
        private static ComputeBuffer triCountBuffer;

        /// <summary>
        /// Create all the buffers for the visualize function
        /// </summary>
        /// <param name="size"></param>
        private static void CreateBuffers(int size)
        {
            int fullSize = size * size * size;
            int nrVoxPAxis = size - 1;
            int nrVox = nrVoxPAxis * nrVoxPAxis * nrVoxPAxis;
            int maxTriCount = nrVox * 5;

            if (floatMapBuff == null)
            {
                goto Run;
            }
            else
            {
                if (fullSize != floatMapBuff.count)
                {
                    DisposeBuffers();
                    goto Run;
                }
            }

        Run:
            triangleBuff = new ComputeBuffer(maxTriCount, sizeof(float) * 9, ComputeBufferType.Append);
            triangleBuff.SetCounterValue(0);
            floatMapBuff = new ComputeBuffer(fullSize, sizeof(float) * 4);
            triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        }

        /// <summary>
        /// release all the buffers
        /// </summary>
        private static void DisposeBuffers()
        {
            floatMapBuff.Release();
            floatMapBuff.Dispose();
            floatMapBuff = null;

            triangleBuff.Release();
            triangleBuff.Dispose();
            triangleBuff = null;

            triCountBuffer.Release();
            triCountBuffer.Dispose();
            triCountBuffer = null;
        }

        /// <summary>
        /// Constructor containing the threshold, computeshader and monobehaviour
        /// </summary>
        /// <param name="thres">threshold for the surface level</param>
        /// <param name="generator">The monobehaviour that called this function (indirectly)</param>
        /// <param name="marchingCompute">The compute shader used for generating the mesh</param>
        public MarchingComputeVisualiser(float thres, ComputeShader marchingCompute, MonoBehaviour generator)
        {
            threshold = thres;
            compute = marchingCompute;
            this.generator = generator;
        }

        /// <summary>
        /// Visualize using marching cubes
        /// </summary>
        /// <param name="values">the input values</param>
        /// <param name="template">mesh themplate</param>
        /// <returns>generated mesh</returns>
        public Mesh Visualize(float[,,] values, Mesh template, int size)
        {
            size += 1;
            CreateBuffers(size);

            List<Float4> vals = new List<Float4>();
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        vals.Add(new Float4() { x = x, y = y, z = z, w = values[x, y, z] });
                    }
                }
            }

            SetValues(vals, size);
            Dispatch(size);

            int nrTris = GetCount();
            Tri[] tris = GetTris(nrTris);            

            template = CreateMesh(tris,template);

            DisposeBuffers();

            return template;
        }

        /// <summary>
        /// Put the values into the ComputeShader
        /// </summary>
        /// <param name="vals">float map values</param>
        /// <param name="size">the size</param>
        private void SetValues(List<Float4> vals, int size)
        {
            floatMapBuff.SetData(vals);
            compute.SetBuffer(0, "floatMap", floatMapBuff);

            compute.SetBuffer(0, "triangles", triangleBuff);

            compute.SetInt("size", size);
            compute.SetFloat("surfaceLevel", threshold);
        }
        
        /// <summary>
        /// Start the Compute Shader
        /// </summary>
        /// <param name="size">the size</param>
        private void Dispatch(int size)
        {
            int threadsPAxis = Mathf.CeilToInt((size - 1) / 8f);
            compute.Dispatch(0, threadsPAxis, threadsPAxis, threadsPAxis);
        }

        /// <summary>
        /// Get the count of items inside a Compute Buffer
        /// </summary>
        /// <returns>Count of elements</returns>
        private int GetCount()
        {
            ComputeBuffer.CopyCount(triangleBuff, triCountBuffer, 0);

            int[] triCountArray = { 0 };
            triCountBuffer.GetData(triCountArray);

            return triCountArray[0];
        }

        /// <summary>
        /// Create the mesh object from the triangle array and a template
        /// </summary>
        /// <param name="tris">The triangles</param>
        /// <param name="template">the template</param>
        /// <returns>a unityengine mesh</returns>
        private Mesh CreateMesh(Tri[] tris, Mesh template)
        {
            int nrTris = tris.Length;

            Vector3[] vertices = new Vector3[nrTris * 3];
            int[] triangles = new int[nrTris * 3];

            for (int i = 0; i < nrTris; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    triangles[i * 3 + j] = i * 3 + j;
                    vertices[i * 3 + j] = tris[i][j];
                }
            }

            template.Clear();

            template.vertices = vertices;
            template.triangles = triangles;
            template.RecalculateNormals();

            return template;
        }

        private Tri[] GetTris(int nrTris)
        {
            Tri[] tris = new Tri[nrTris];

            triangleBuff.GetData(tris);
            triangleBuff.SetCounterValue(0);

            return tris;
        }

        /// <summary>
        /// the offset of the size used by Chunk
        /// </summary>
        /// <returns></returns>
        public int sizeOffset()
        {
            return 1;
        }
    }

    /// <summary>
    /// triangle model
    /// </summary>
    struct Tri
    {
#pragma warning disable 649 // disable unassigned variable warning
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Vector3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
        }
    }

    /// <summary>
    /// float4 model
    /// </summary>
    struct Float4
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }
}




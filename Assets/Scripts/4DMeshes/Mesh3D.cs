using MyMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Mesh3D
{
    public float3[] vertices;
    public int3[] triangles;

    /// <summary>
    /// Instantiate a 3D Mesh with triangles and vertices
    /// </summary>
    /// <param name="tris">An array of triangles</param>
    /// <param name="vertices">An array of vertices</param>
    public Mesh3D(int3[] tris, float3[] vertices)
    {
        this.vertices = vertices;
        this.triangles = tris;
    }

    /// <summary>
    /// Create a unity mesh from a 3Dmesh
    /// </summary>
    /// <returns>Unity Mesh</returns>
    public Mesh ToUnityMesh()
    {
        Mesh m = new Mesh();
        m.Clear();
        Vector3[] vert3s = new Vector3[vertices.Length];
        int[] tris = new int[triangles.Length * 3];
        int ind = 0;
        foreach (var item in vertices)
        {
            vert3s[ind] = float3.ToVector3(item);
            ind++;
        }
        ind = 0;
        foreach (var item in triangles)
        {
            tris[ind] = item.a;
            tris[ind + 1] = item.b;
            tris[ind + 2] = item.c;
            ind += 3;
        }
        m.vertices = vert3s;
        m.triangles = tris;

        m.RecalculateNormals();
        return m;

    }
    
}


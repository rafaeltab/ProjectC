using MyMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
class Displayer4D : MonoBehaviour
{
    [Range(-1.5f, 1.5f)]
    public float wPos = 0f;
    public float6 rotation = new float6(0, 0, 0, 0, 0, 0);
    public bool animateRotation = true;
    private float Count = 0;

    Mesh4D mesh = new Mesh4D(Primitives.HYPERCUBE);
    public void Start()
    {
        UpdateMesh();
    }

    /// <summary>
    /// make sure the correct rotation is put in and also change this rotation
    /// </summary>
    public void Update()
    {
        if (animateRotation)
        {

            rotation.A = Count.Sin() / 2 + 0.5f;
            rotation.B = (Count/10).Sin() / 2 + 0.5f;
            rotation.C = (Count / 100).Sin() / 2 + 0.5f;
            rotation.D = (Count / 1000).Sin() / 2 + 0.5f;
            rotation.E = (Count / 10000).Sin() / 2 + 0.5f;
            rotation.F = (Count / 100000).Sin() / 2 + 0.5f;

            Count += 0.1f;
        }

        mesh.SetPos(new MyMath.float4(0, 0, 0, wPos));
        mesh.SetRotation(rotation);
        UpdateMesh();

    }

    /// <summary>
    /// Update the mesh
    /// </summary>
    private void UpdateMesh()
    {        
        Mesh3D slice = mesh.GetSlice();
        GetComponent<MeshFilter>().sharedMesh = slice.ToUnityMesh();
    }
}
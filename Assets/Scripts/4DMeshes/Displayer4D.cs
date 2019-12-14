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

    Mesh4D mesh = new Mesh4D(Primitive.HYPERCUBE);
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
            Animate();
        }

        mesh.SetPos(new MyMath.float4(0, 0, 0, wPos));
        mesh.SetRotation(rotation);
        UpdateMesh();

    }

    /// <summary>
    /// animate the object
    /// </summary>
    public void Animate()
    {
        if (wPos == 0)
        {
            wPos = 0.1f;
        }

        rotation.A = Count.Sin() / 4 + 0.25f;
        rotation.B = (Count / 10).Sin() / 4 + 0.25f;
        rotation.D = (Count / 100).Sin() / 4 + 0.25f;

        if (wPos < 0)
        {
            rotation.A *= -1;
            rotation.B *= -1;
            rotation.D *= -1;
        }

        Count += 0.1f;
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
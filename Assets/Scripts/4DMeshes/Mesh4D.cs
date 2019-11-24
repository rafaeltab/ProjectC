using System.Collections;
using System.Collections.Generic;
using System;
using MyMath;
using UnityEngine;

public class Mesh4D
{
    public float4[] vertices;
    public int4[] cells;

    public float4 position = new float4(0,0,0,0);
    public float6 rotation = new float6(0, 0, 0, 0, 0, 0);

    /// <summary>
    /// create an empty 4d mesh
    /// </summary>
    public Mesh4D()
    {
        vertices = new float4[0];
        cells = new int4[0];
    }

    /// <summary>
    /// Create a primitive mesh
    /// </summary>
    /// <param name="primitive"></param>
    public Mesh4D(Primitives primitive)
    {
        switch (primitive)
        {
            case Primitives.HYPERCUBE:
                PrimitiveHyper();
                break;
        }
    }

    /// <summary>
    /// Create a hyper cube
    /// </summary>
    private void PrimitiveHyper()
    {
        vertices = new float4[10];
        cells = new int4[24]; // will become 24 length

        vertices[0] = new float4(0, 0, 0, 0);//0
        vertices[1] = new float4(0, 0, 1, 0);//1

        vertices[2] = new float4(0, 1, 0, 0);//2
        vertices[3] = new float4(0, 1, 1, 0);//3

        vertices[4] = new float4(1, 0, 0, 0);//4
        vertices[5] = new float4(1, 0, 1, 0);//5

        vertices[6] = new float4(1, 1, 0, 0);//6
        vertices[7] = new float4(1, 1, 1, 0);//7

        vertices[8] = new float4(0.5f, 0.5f, 0.5f, 1);
        vertices[9] = new float4(0.5f, 0.5f, 0.5f, -1f);

        //front
        cells[0] = new int4(2, 6, 0, 8);
        cells[1] = new int4(6, 4, 0, 8);
        //right
        cells[2] = new int4(6, 7, 4, 8);
        cells[3] = new int4(7, 5, 4, 8);
        //left
        cells[4] = new int4(3, 2, 1, 8);
        cells[5] = new int4(2, 0, 1, 8);
        //back
        cells[6] = new int4(7, 3, 5, 8);
        cells[7] = new int4(3, 1, 5, 8);
        //top
        cells[8] = new int4(7, 6, 3, 8);
        cells[9] = new int4(6, 2, 3, 8);
        //bottom
        cells[10] = new int4(1, 0, 5, 8);
        cells[11] = new int4(0, 4, 5, 8);

        //now inside

        //front
        cells[12] = new int4(2, 6, 0, 9);
        cells[13] = new int4(6, 4, 0, 9);
        //righ
        cells[14] = new int4(6, 7, 4, 9);
        cells[15] = new int4(7, 5, 4, 9);
        //left
        cells[16] = new int4(3, 2, 1, 9);
        cells[17] = new int4(2, 0, 1, 9);
        //back
        cells[18] = new int4(7, 3, 5, 9);
        cells[19] = new int4(3, 1, 5, 9);
        //top
        cells[20] = new int4(7, 6, 3, 9);
        cells[21] = new int4(6, 2, 3, 9);
        //bottom
        cells[22] = new int4(1, 0, 5, 9);
        cells[23] = new int4(0, 4, 5, 9);

    }

    /// <summary>
    /// get a 3d slice of the 4d world
    /// </summary>
    /// <returns>3d mesh</returns>
    public Mesh3D GetSlice()
    {
        List<float3> resultVerts = new List<float3>();
        List<int3> resultTris = new List<int3>();

        //get the w = 0 volume

        for (int cell = 0; cell < cells.Length; cell++)
        {
            float4 vertA = vertices[cells[cell].a];
            float4 vertB = vertices[cells[cell].b];
            float4 vertC = vertices[cells[cell].c];
            float4 vertD = vertices[cells[cell].d];

            vertA = FullRotateSingle(vertA);
            vertB = FullRotateSingle(vertB);
            vertC = FullRotateSingle(vertC);
            vertD = FullRotateSingle(vertD);

            vertA += position;
            vertB += position;
            vertC += position;
            vertD += position;

            float minW = float.MaxValue;
            float maxW = float.MinValue;

            minW = vertA.w.Min(vertB.w, vertC.w, vertD.w);
            maxW = vertA.w.Max(vertB.w, vertC.w, vertD.w);

            if (minW < 0 && 0 < maxW)
            {

                int count = SingleSide(vertA, vertB, resultVerts);
                count += SingleSide(vertA, vertC, resultVerts);
                count += SingleSide(vertA, vertD, resultVerts);
                count += SingleSide(vertB, vertC, resultVerts);
                count += SingleSide(vertB, vertD, resultVerts);
                count += SingleSide(vertC, vertD, resultVerts);

                if (count == 3)
                {
                    resultTris.Add(new int3() { a = resultVerts.Count - 3, b = resultVerts.Count - 2, c = resultVerts.Count - 1 });
                }
                else if(count == 4)
                {
                    var a = resultVerts.Count - 4;
                    var b = resultVerts.Count - 3;
                    var c = resultVerts.Count - 2;
                    var d = resultVerts.Count - 1;

                    resultTris.Add(new int3() { a = a, b = b, c = c });
                    resultTris.Add(new int3() { a = c, b = d, c = b });
                    resultVerts.RemoveAt(resultVerts.Count - 1);

                    //TODO: Fix this. Causes weird triangles 
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        resultVerts.RemoveAt(resultVerts.Count - 1);
                    }
                }
            }
            else
            {
                //not cuttung through so skip
                continue;
            }
        }
        return new Mesh3D(resultTris.ToArray(),resultVerts.ToArray());
    }

    int count = 0;

    /// <summary>
    /// Check whether an edge cuts through the W=0 plane
    /// </summary>
    /// <param name="a">Point a that describes the side</param>
    /// <param name="b">Point b that describes the side</param>
    /// <param name="sides">A list of all vertices</param>
    /// <returns>1 if it cuts through 0 if it doesn't</returns>
    private int SingleSide(float4 a, float4 b,List<float3> verts)
    {
        //check wheter or not one is lower then 0 in w
        if (a.w * b.w <= 0)
        {
            count++;
            float3 result = a.LerpByW(b);
            verts.Add(result);
            return 1;
        }
        return 0;
    }

    /// <summary>
    /// Rotate around a single plane
    /// </summary>
    /// <param name="rotationPlane">The plane that stays constant</param>
    /// <param name="degrees">The amount of degrees to rotate by</param>
    public void Rotate(Axis4D rotationPlane, float degrees)
    {
        rotation.Add((int)rotationPlane, degrees);
    }

    /// <summary>
    /// Set the rotation
    /// </summary>
    /// <param name="rotation">The rotation to set</param>
    public void SetRotation(float6 rotation)
    {
        this.rotation = rotation;
    }

    /// <summary>
    /// Fully rotate a single point around the origin
    /// </summary>
    /// <param name="point">The point to rotate</param>
    /// <returns>The rotated point</returns>
    private float4 FullRotateSingle(float4 point)
    {
        var x = point.x;
        var y = point.y;
        var z = point.z;
        var w = point.w;        

        //xy
        Matrix1D matr = new Matrix1D(new float[] { z, w });
        Matrix1D rotated = (Matrix1D) (matr * GetRotationMatrix(rotation.A));
        z = rotated.values[0];
        w = rotated.values[1];

        //xz
        matr = new Matrix1D(new float[] { y, w });
        rotated = (Matrix1D)(matr * GetRotationMatrix(rotation.B));
        y = rotated.values[0];
        w = rotated.values[1];

        //xw
        matr = new Matrix1D(new float[] { y, z });
        rotated = (Matrix1D)(matr * GetRotationMatrix(rotation.C));
        y = rotated.values[0];
        z = rotated.values[1];

        //yz
        matr = new Matrix1D(new float[] { x, w });
        rotated = (Matrix1D)(matr * GetRotationMatrix(rotation.D));
        x = rotated.values[0];
        w = rotated.values[1];

        //yw
        matr = new Matrix1D(new float[] { x, z });
        rotated = (Matrix1D)(matr * GetRotationMatrix(rotation.E));
        x = rotated.values[0];
        z = rotated.values[1];

        //zw
        matr = new Matrix1D(new float[] { x, y });
        rotated = (Matrix1D)(matr * GetRotationMatrix(rotation.F));
        x = rotated.values[0];
        y = rotated.values[1];

        return new float4(x,y,z,w);
    }

    /// <summary>
    /// Get a 2Drotation matrix with degrees filled in
    /// </summary>
    /// <param name="degree">The degrees</param>
    /// <returns>The rotation matrix</returns>
    private static Matrix2D GetRotationMatrix(float degree)
    {
        var sin = degree.Sin();
        var cos = degree.Cos();

        return new Matrix2D(new float[,] { { cos, -sin }, { sin, cos } });
    }

    /// <summary>
    /// Add position to the current position
    /// </summary>
    /// <param name="deltaPos">The position to add</param>
    public void Move(float4 deltaPos)
    {
        position += deltaPos;
    }

    /// <summary>
    /// Set the position
    /// </summary>
    /// <param name="newPos">The position to set to</param>
    public void SetPos(float4 newPos)
    {
        position = newPos;
    }

    public enum Axis4D {xy,xz,xw,yz,yw,zw}
}



public enum Primitives
{
    HYPERCUBE
}

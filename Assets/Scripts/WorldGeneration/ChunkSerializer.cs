using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.MeshGenModel;

/// <summary>
/// A class for serializing and deserializing chunks
/// </summary>
public static class ChunkSerializer
{
    /// <summary>
    /// serialize a chunk into a byte array
    /// </summary>
    public static byte[] Serialize(Chunk chunk)
    {
        List<byte> output = new List<byte>();

        output.AddRange(BitConverter.GetBytes(chunk.trength));

        output.AddRange(BitConverter.GetBytes(chunk.position.x));
        output.AddRange(BitConverter.GetBytes(chunk.position.y));
        output.AddRange(BitConverter.GetBytes(chunk.position.z));

        output.AddRange(BitConverter.GetBytes(chunk.seed));

        output.AddRange(BitConverter.GetBytes(chunk.threshold));

        output.AddRange(BitConverter.GetBytes(chunk.size));

        output.AddRange(BitConverter.GetBytes(chunk.noiseScale));

        output.Add(0x00);

        output.AddRange(BitConverter.GetBytes(chunk.vals.GetLength(0)));
        output.AddRange(BitConverter.GetBytes(chunk.vals.GetLength(1)));
        output.AddRange(BitConverter.GetBytes(chunk.vals.GetLength(2)));

        output.Add(0x00);

        byte[,,][] byteMap = new byte[chunk.vals.GetLength(0), chunk.vals.GetLength(1), chunk.vals.GetLength(2)][]; 
        Parallel.For(0,chunk.vals.GetLength(0),(x)=> {
            Parallel.For(0, chunk.vals.GetLength(1), (y) => {
                Parallel.For(0, chunk.vals.GetLength(2), (z) => {
                    byteMap[x, y, z] = BitConverter.GetBytes(chunk.vals[x, y, z]);
                });
            });
        });

        for (int x = 0; x < chunk.vals.GetLength(0); x++)
        {
            for (int y = 0; y < chunk.vals.GetLength(1); y++)
            {
                for (int z = 0; z < chunk.vals.GetLength(2); z++)
                {
                    output.AddRange(byteMap[x,y,z]);
                }
            }
        }

        return output.ToArray();
    }

    /// <summary>
    /// Deserialize a file into a Chunk using the correct visualizers etc.
    /// </summary>
    public static Chunk Deserialize(byte[] data,ComputeShader compute, MonoBehaviour generator,Visualizer meshVisualizer)
    {
        /////////
        float trength = BitConverter.ToSingle(data, 0);//4
        int xPos = BitConverter.ToInt32(data, 4);//4
        int yPos = BitConverter.ToInt32(data, 8);//4
        int zPos = BitConverter.ToInt32(data, 12);//4

        long seed = BitConverter.ToInt64(data,16);//8
        float threshold = BitConverter.ToSingle(data, 24);//4
        int size = BitConverter.ToInt32(data, 28);//4
        float noiseScale = BitConverter.ToSingle(data, 32);//4
        /////////
        //0x00
        /////////
        
        int length0 = BitConverter.ToInt32(data, 37);//4
        int length1 = BitConverter.ToInt32(data, 41);//4
        int length2 = BitConverter.ToInt32(data, 45);//4

        /////////
        //0x00
        /////////
        int start = 50;
        byte[,,][] valsBytes = new byte[length0, length1, length2][];

        for (int x = 0; x < length0; x++)
        {
            for (int y = 0; y < length1; y++)
            {
                for (int z = 0; z < length2; z++)
                {
                    valsBytes[x,y,z] = new byte[4] { data[start], data[start+1], data[start+2], data[start+3] };
                    start += 4;
                }
            }
        }

        float[,,] floatMap = new float[length0, length1, length2];
        Parallel.For(0, length0, (x) => {
            Parallel.For(0, length1, (y) => {
                Parallel.For(0, length2, (z) => {
                    floatMap[x, y, z] = BitConverter.ToSingle(valsBytes[x,y,z],0);
                });
            });
        });
        Chunk c = new Chunk(new Vector3Int(xPos,yPos,zPos),trength,seed,threshold,compute,generator, meshVisualizer, size,noiseScale);
        c.vals = floatMap;
        c.generated = true;

        return c;
    }
}

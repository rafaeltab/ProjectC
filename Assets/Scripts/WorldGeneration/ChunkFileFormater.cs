using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WorldGeneration
{
    public static class ChunkFileFormater
    {
        public static byte[] Serialize(Chunk c,string folder)
        {
            List<byte> finalBytes = new List<byte>();
            
            finalBytes.AddRange(IntToBytes(c.position.x)); //x location
            finalBytes.AddRange(IntToBytes(c.position.y)); //y location
            finalBytes.AddRange(IntToBytes(c.position.z)); //z location

            finalBytes.AddRange(IntToBytes(c.size)); //size

            finalBytes.AddRange(FloatToBytes(c.trength)); //trength

            for (int x = 0; x < c.size; x++)
            {
                for (int y = 0; y < c.size; y++)
                {
                    for (int z = 0; z < c.size; z++)
                    {
                        finalBytes.AddRange(FloatToBytes(c.vals[x, y, z]));
                    }
                }
            }

            return finalBytes.ToArray();
        }

        public static Chunk DeSerialize(byte[] file,long seed,float threshold, ComputeShader compute, MonoBehaviour generator, MeshGenModel.Visualizer visualizer, float noiseScale = 1f)
        {
            int xLocation = BytesToInt(getSection(file, 0, 4),0);
            int yLocation = BytesToInt(getSection(file, 5, 4),0);
            int zLocation = BytesToInt(getSection(file, 9, 4),0);
            int size = BytesToInt(getSection(file, 13, 4),0);

            float trength = BytesToFloat(getSection(file, 17, 4));

            int startIndex = 21;
            float[,,] values = new float[size, size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        values[x,y,z] = BytesToFloat(getSection(file,startIndex,4));
                        startIndex += 4;
                    }
                }
            }
            Chunk c = new Chunk(new Vector3Int(xLocation,yLocation,zLocation),trength,seed,threshold,compute,generator,visualizer,size,noiseScale);
            c.vals = values;
            c.SetGenerated();
            return c; 
        }

        private static byte[] getSection(byte[] input, int offset, int amount)
        {
            if (input.Length <= offset+amount)
            {
                throw new ArgumentOutOfRangeException("the size of the input is smaller then or equal to offset + amount");
            }

            byte[] ret = new byte[amount];

            for (int i = 0; i < amount; i++)
            {
                ret[i] = input[offset + i];
            }

            return ret;
        }

        private static byte[] IntToBytes(int input)
        {
            byte lolo = (byte)(input & 0xff);
            byte hilo = (byte)((input >> 8) & 0xff);
            byte lohi = (byte)((input >> 16) & 0xff);
            byte hihi = (byte)(input >> 24);

            return new byte[] { lolo, hilo, lohi, hihi };
        }

        private static byte[] FloatToBytes(float input)
        {
            var bytes = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        private static int BytesToInt(byte[] input, int startIndex)
        {
            return input[startIndex] | (input[startIndex + 1] << 8) | (input[startIndex + 2] << 16) | (input[startIndex + 3] << 24);
        }

        private static float BytesToFloat(byte[] input)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(input);
            }
            return BitConverter.ToSingle(input,0);
        }

        /*
         
        Format:

        byte1-4: x location
        byte5-8: y location
        byte9-12: z location
        byte13-16: size
        byte17-20: trength

        rest: values in (4-byte) float format
        */
    }
}

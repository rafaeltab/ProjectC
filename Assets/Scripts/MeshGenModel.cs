using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class MeshGenModel
    {
        public MeshGenModel() { }
        public MeshGenModel(MeshGenModel m)
        {
            autoUpdate = m.autoUpdate;
            sizeX = m.sizeX;
            sizeY = m.sizeY;
            sizeZ = m.sizeZ;
            wPos = m.wPos;
            noiseScale = m.noiseScale;
            threshold = m.threshold;
            created = m.created;
        }

        public Vector3Int chunks;

        public bool autoUpdate = false;
        public int sizeX = 10;
        public int sizeY = 10;
        public int sizeZ = 10;
        [Range(0,10)]
        public float wPos;

        public float noiseScale = 0.45f;
        public float threshold = 0.5f;
        public bool created = false;
    }
}

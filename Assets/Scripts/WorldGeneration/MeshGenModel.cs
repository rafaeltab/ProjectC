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
            size = m.size;
            wPos = m.wPos;
            noiseScale = m.noiseScale;
            threshold = m.threshold;
            template = m.template;
            loadPoint = m.loadPoint;
        }

        public Vector3Int chunks;

        public int size = 10;
        [Range(0,10)]
        public float wPos;

        public float noiseScale = 0.45f;
        public float threshold = 0.5f;
        public int renderDistance = 1;
        public GameObject template;
        public Transform loadPoint;
        public Vector3 loadPointPos;
        public float wPosUpdateRate = 1f;//in seconds
    }
}

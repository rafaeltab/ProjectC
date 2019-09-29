using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class GeneratorHelper
    {
        #region draw methods
        private static readonly Vector3 up = new Vector3() { x = 0, y = 1, z = 0 }; // up
        private static readonly Vector3 rightup = new Vector3() { x = 0, y = 1, z = 1 }; // rightup
        private static readonly Vector3 nearup = new Vector3() { x = 1, y = 1, z = 0 }; // nearup
        private static readonly Vector3 nearrightup = new Vector3() { x = 1, y = 1, z = 1 }; // nearrightup
        private static readonly Vector3 near = new Vector3() { x = 1, y = 0, z = 0 }; // near
        private static readonly Vector3 nearright = new Vector3() { x = 1, y = 0, z = 1 }; // nearright
        private static readonly Vector3 basis = new Vector3() { x = 0, y = 0, z = 0 }; // base
        private static readonly Vector3 right = new Vector3() { x = 0, y = 0, z = 1 }; // right

        public static Vector3[] drawUp()
        {
            return new Vector3[] { up, rightup, nearup, nearrightup };
        }

        public static Vector3[] drawDown()
        {
            return new Vector3[] { near, nearright, basis, right };
        }

        public static Vector3[] drawLeft()
        {
            return new Vector3[] { basis, right, up, rightup };
        }

        public static Vector3[] drawRight()
        {
            return new Vector3[] { nearup, nearrightup, near, nearright };
        }

        public static Vector3[] drawNear()
        {
            return new Vector3[] { basis, up, near, nearup };
        }

        public static Vector3[] drawFar()
        {
            return new Vector3[] { nearright, nearrightup, right, rightup };
        }

        public static void CreateFace(Vector3[] verticesTBA, List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, Vector3 pos)
        {
            verticesTBA[0] = Add(verticesTBA[0], pos);
            verticesTBA[1] = Add(verticesTBA[1], pos);
            verticesTBA[2] = Add(verticesTBA[2], pos);
            verticesTBA[3] = Add(verticesTBA[3], pos);

            int b = vertices.Count;

            foreach (Vector3 v in verticesTBA)
            {
                vertices.Add(v);
                uvs.Add(new Vector2(0, 1));
            }

            triangles.Add(b);
            triangles.Add(b + 1);
            triangles.Add(b + 2);
            triangles.Add(b + 2);
            triangles.Add(b + 1);
            triangles.Add(b + 3);
        }
        #endregion draw methods

        public static Vector3 Add(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
    }
}

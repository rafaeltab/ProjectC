using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// An interface for all mesh visualizers to use so we can easily exchange which we use
    /// </summary>
    public interface IMeshVisualiser
    {
        /// <summary>
        /// Function for generating a mesh from a 3D float array
        /// </summary>
        /// <param name="values"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        Mesh Visualize(float[,,] values, Mesh template,int size);
        int sizeOffset();
    }
}

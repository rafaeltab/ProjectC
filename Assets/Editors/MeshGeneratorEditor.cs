using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
[CanEditMultipleObjects]
public class MeshGeneratorEditor : Editor
{
    MeshGenerator meshGen;

    /// <summary>
    /// function that runs when the class is loaded
    /// </summary>
    void OnEnable()
    {
        meshGen = (MeshGenerator)target;
    }

    /// <summary>
    /// Function that is called every Inspector update
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (meshGen.gen)
        {
            GUILayout.Label("Genning");
        }

        GUILayout.Label($"Elapsed time: {meshGen.time}");
    }
}

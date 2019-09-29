using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
[CanEditMultipleObjects]
public class MeshGeneratorEditor : Editor
{
    MeshGenerator meshGen;

    void OnEnable()
    {
        meshGen = (MeshGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Hey"))
        {
            meshGen.StopCurrentGen();
        }

        if (meshGen.gen)
        {
            GUILayout.Label("Genning");
        }
    }
}

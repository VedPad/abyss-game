using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(createPlaneScript))]
public class createPlaneInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        createPlaneScript _createPlaneScript = (createPlaneScript) target;
        if (GUILayout.Button("Generate Mesh"))
        {
            _createPlaneScript.GenerateMesh();
        }
    }
}

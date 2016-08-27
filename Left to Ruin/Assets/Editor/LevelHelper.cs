// Date   : 27.08.2016 09:10
// Project: LD36
// Author : bradur

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Level))]
public class LevelHelper : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Level level = (Level)target;
        if(GUILayout.Button("Map file..."))
        {
            level.SetMapPath(EditorUtility.OpenFilePanel("Set map file path", "", ""));
        }
    }
}

using UnityEngine;
using UnityEditor;
using FS.Scripts;

[CustomEditor(typeof(FS_Annotation))]
public class FS_AnnotationEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FS_Annotation annotation = (FS_Annotation)target; // store new reference

        GUILayout.BeginHorizontal();

        if (GUILayout.Button(new GUIContent("Go", "Launch URL"), GUILayout.MaxWidth(40)))
        {
            Application.OpenURL(annotation.URL);
        }


        annotation.URL = EditorGUILayout.TextField(annotation.URL);

        GUILayout.EndHorizontal();

    }

}
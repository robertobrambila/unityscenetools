using UnityEngine;
using UnityEditor;
using FS.Scripts;

[CustomEditor(typeof(FS_Annotation))]
public class FS_AnnotationEditor : Editor
{
    public string URL = "https://";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button(new GUIContent("Go", "Launch URL"), GUILayout.MaxWidth(40)))
        {
            validateURL(URL);
            launchURL(URL);
        }

        URL = GUILayout.TextField(URL);

        GUILayout.EndHorizontal();

    }

    private void validateURL(string _URL){
        Debug.Log("Validated URL " + _URL);
    }

    private void launchURL(string _URL){
        Debug.Log("Launched URL " + _URL);
    }

}
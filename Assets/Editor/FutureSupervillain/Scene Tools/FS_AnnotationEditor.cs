using UnityEngine;
using UnityEditor;
using FS.Scripts;

[CustomEditor(typeof(FS_Annotation))]
public class FS_AnnotationEditor : Editor
{

    void Awake()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FS_Annotation annotation = (FS_Annotation)target; // store new target reference

        #region Annotation Text Area
        GUILayout.BeginHorizontal();

        // ScrollView controls the TextArea element itself, not the content that's inside of the text area.
        // The distinction is important because setting a height property to the TextArea element can leave a view
        // that isn't scrollable even though the content inside does cover many more lines. Setting ExpandHeight 
        // allows the actual element to resize along with the content and enables the scrolling we're expecting.
        annotation.scrollPos = EditorGUILayout.BeginScrollView(annotation.scrollPos, GUILayout.Height(100));
        EditorStyles.textArea.wordWrap = true;
        annotation.annotationTextArea = EditorGUILayout.TextArea(annotation.annotationTextArea, EditorStyles.textArea, GUILayout.ExpandHeight(true));


        EditorGUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        #endregion

        #region URL Launcher
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(new GUIContent("Go", "Launch URL"), GUILayout.MaxWidth(40)))
        {
            Application.OpenURL(annotation.URL);
        }


        annotation.URL = EditorGUILayout.TextField(annotation.URL);

        GUILayout.EndHorizontal();
        #endregion 

    }

}
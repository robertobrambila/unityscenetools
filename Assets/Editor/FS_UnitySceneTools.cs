﻿using UnityEngine;
using UnityEditor;

// To-do
// * Add undo to Solo/Unsolo
// * Add modifier keys
// * Add image states to buttons

public class FS_UnitySceneTools : EditorWindow, IHasCustomMenu
{
    GUIStyle FSUSTStyle = new GUIStyle();

    static int buttons = 5;
    static float buttonWidth = 40f;
    static float buttonHeight = 40f;
    static float buttonPadding = 4f;

    Texture newEmptyGameObjectTexture;
    Texture resetPSRTexture;
    Texture dropToGroundTexture;
    Texture soloObjectsTexture;
    Texture unsoloObjectsTexture;

    GUIContent newEmptyGameObjectContent = new GUIContent();
    GUIContent resetPSRTextureContent = new GUIContent();
    GUIContent dropToGroundContent = new GUIContent();
    GUIContent soloObjectsContent = new GUIContent();
    GUIContent unsoloObjectsContent = new GUIContent();


    bool verticalLayoutDirection = false;

    #region about context menu
    string prodName = "F$ Scene Tools";
    string verNumber = "v0.1.0";
    string url = "https://www.futuresupervillain.com";

    void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
    {
        GUIContent verticalContent = new GUIContent("Toggle Layout Direction");
        menu.AddItem(verticalContent, false, verticalContextMenu);

        GUIContent aboutContent = new GUIContent("About...");
        menu.AddItem(aboutContent, false, aboutContextMenu);
    }
 
     private void verticalContextMenu()
    {
        verticalLayoutDirection = !verticalLayoutDirection;
        if (verticalLayoutDirection)
        {
            Debug.Log("Layout Direction Set To Vertical.");
        } else
        {
            Debug.Log("Layout Direction Set To Horizontal.");
        }
    }

    private void aboutContextMenu()
    {
        Debug.LogFormat("{0} {1}\n{2}", prodName,verNumber,url);
    }
    #endregion


    [MenuItem("Window/FutureSupervillain/Scene Tools")]
    private static void ShowWindow() {
        var window = GetWindow<FS_UnitySceneTools>();
        window.titleContent = new GUIContent("F$ Scene Tools");
        window.minSize = new Vector2( (buttons * buttonWidth) + buttonPadding, buttonHeight + (buttonPadding * 2f));
        window.Show();
    }

    void Awake()
    {
        newEmptyGameObjectTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        newEmptyGameObjectContent.image = newEmptyGameObjectTexture;
        newEmptyGameObjectContent.tooltip = "New Empty Game Object";
        

        resetPSRTextureContent.tooltip = "Reset PSR";
        resetPSRTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        resetPSRTextureContent.image = resetPSRTexture;

        dropToGroundContent.tooltip = "Drop To Ground";
        dropToGroundTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        dropToGroundContent.image = dropToGroundTexture;

        soloObjectsContent.tooltip = "Solo Selected Objects";
        soloObjectsTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        soloObjectsContent.image = soloObjectsTexture;

        unsoloObjectsContent.tooltip = "Unsolo All";
        unsoloObjectsTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        unsoloObjectsContent.image = unsoloObjectsTexture;

    }

    private void OnGUI() {
        if (verticalLayoutDirection)
        {
            GUILayout.BeginArea(new Rect(buttonPadding, buttonPadding, buttonHeight + buttonPadding, (buttons * buttonWidth)));
            GUILayout.BeginVertical();
        } else {
            GUILayout.BeginArea(new Rect(buttonPadding, buttonPadding, (buttons * buttonWidth), buttonHeight + buttonPadding));
            GUILayout.BeginHorizontal();
        }

        #region New Empty Game Object
        GUI.skin.button.normal.background = (Texture2D)newEmptyGameObjectContent.image;
        if (GUILayout.Button(newEmptyGameObjectContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            GameObject go = new GameObject("GameObject");
            go.transform.position = Vector3.zero;
            Undo.RegisterCreatedObjectUndo(go, "Create New Empty GameObject");
        }
        #endregion

        #region Reset PSR
        if (GUILayout.Button(resetPSRTextureContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Reset PSR");
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.transform.localEulerAngles = Vector3.zero;  
                PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
            }
        }
        #endregion

        #region Drop To Ground
        if (GUILayout.Button(dropToGroundContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Drop To Ground");
                if (obj.GetComponent<Renderer>())
                {
                    float obj_minY = obj.GetComponent<Renderer>().bounds.min.y;
                    obj.transform.localPosition = new Vector3(obj.transform.position.x, 
                                                            obj.transform.position.y - obj_minY,
                                                            obj.transform.position.z);
                } else
                {
                    obj.transform.localPosition = new Vector3(obj.transform.position.x, 
                                                            0,
                                                            obj.transform.position.z);
                }
                PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
            }
        }
        #endregion

        #region Solo
        if (GUILayout.Button(soloObjectsContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            var svm = SceneVisibilityManager.instance;
            foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (!Selection.Contains (obj)) // only apply to non-selected objects
                {
                obj.hideFlags = HideFlags.HideInHierarchy;
                if (obj.GetComponent<Renderer>()) obj.GetComponent<Renderer>().enabled = false; // workaround for the potential bug?

                // used to manually refresh Hierarchy Window
                EditorApplication.RepaintHierarchyWindow ();
                EditorApplication.DirtyHierarchyWindowSorting();

                // Debug.Log("Isolate: " + obj.name + " : " + obj.hideFlags);
                }
            }

            svm.Isolate(Selection.gameObjects, true); // add modifier for false (don't include children)
        }
        
        if (GUILayout.Button(unsoloObjectsContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            var svm = SceneVisibilityManager.instance;
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                obj.hideFlags = HideFlags.None;
                if (obj.GetComponent<Renderer>()) obj.GetComponent<Renderer>().enabled = true;// workaround for the potential bug?
                // Debug.Log("Show: " + obj.name + " : " + obj.hideFlags);
            }
            svm.ExitIsolation();
        }
        #endregion

        if (verticalLayoutDirection)
        {
            GUILayout.EndVertical();
        } else {
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }
}
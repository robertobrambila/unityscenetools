using UnityEngine;
using UnityEditor;

// To-do
// * Add undo
// * Add modifier keys
// * Add image states to buttons

public class FS_UnitySceneTools : EditorWindow, IHasCustomMenu
{
    GUIStyle FSUSTStyle = new GUIStyle();

    static int buttons = 5;
    static float buttonWidth = 40f;
    static float buttonHeight = 32f;
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
        GUIContent content = new GUIContent("About...");
        menu.AddItem(content, false, aboutContextMenu);
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
        // newEmptyGameObject.text = "N";
        newEmptyGameObjectTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        newEmptyGameObjectContent.image = newEmptyGameObjectTexture;
        newEmptyGameObjectContent.tooltip = "New Empty Game Object";
        

        // resetPSRTextureContent.text = "PSR";
        resetPSRTextureContent.tooltip = "Reset PSR";
        resetPSRTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        resetPSRTextureContent.image = resetPSRTexture;

        // dropToGroundContent.text = "D";
        dropToGroundContent.tooltip = "Drop To Ground";
        dropToGroundTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        dropToGroundContent.image = dropToGroundTexture;

        // soloObjectsContent.text = "S";
        soloObjectsContent.tooltip = "Solo Selected Objects";
        soloObjectsTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        soloObjectsContent.image = soloObjectsTexture;

        // unsoloObjectsContent.text = "U";
        unsoloObjectsContent.tooltip = "Unsolo All";
        unsoloObjectsTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityeditortools/default_40x32.png", typeof(Texture));
        unsoloObjectsContent.image = unsoloObjectsTexture;

    }

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(buttonPadding, buttonPadding, (buttons * buttonWidth), buttonHeight + buttonPadding));
        if (verticalLayoutDirection)
        {
            GUILayout.BeginVertical();
        } else {
            GUILayout.BeginHorizontal();
        }

        #region New Empty Game Object
        GUI.skin.button.normal.background = (Texture2D)newEmptyGameObjectContent.image;
        if (GUILayout.Button(newEmptyGameObjectContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            GameObject go = new GameObject("GameObject");
            go.transform.position = Vector3.zero;
        }
        #endregion

        #region Reset PSR
        if (GUILayout.Button(resetPSRTextureContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.transform.localEulerAngles = Vector3.zero;   
            }
        }
        #endregion

        #region Drop To Ground
        if (GUILayout.Button(dropToGroundContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
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
                // Debug.Log("Show: " + obj.name + " : " + obj.hideFlags);
            }
            svm.ShowAll();
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
using UnityEngine;
using UnityEditor;

// To-do
// * Add undo
// * Add modifier keys
// * Add image states to buttons

public class FS_UnityEditorTools : EditorWindow
{
    GUIStyle FSUETStyle = new GUIStyle();

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


    [MenuItem("Window/FutureSupervillain/Editor Tools")]
    private static void ShowWindow() {
        var window = GetWindow<FS_UnityEditorTools>();
        window.titleContent = new GUIContent("F$ Editor Tools");
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
        if (GUILayout.Button(newEmptyGameObjectContent, FSUETStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            GameObject go = new GameObject("GameObject");
            go.transform.position = Vector3.zero;
        }
        #endregion

        #region Reset PSR
        if (GUILayout.Button(resetPSRTextureContent, FSUETStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
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
        if (GUILayout.Button(dropToGroundContent, FSUETStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
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
        if (GUILayout.Button(soloObjectsContent, FSUETStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
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
        
        if (GUILayout.Button(unsoloObjectsContent, FSUETStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
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
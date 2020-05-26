using UnityEngine;
using UnityEditor;

// To-do
// * Add image states to buttons

namespace FS.Editor
{
    public class FS_UnitySceneTools : EditorWindow, IHasCustomMenu
    {
        GUIStyle FSUSTStyle = new GUIStyle();

        static int buttons = 6;
        static float buttonWidth = 40f;
        static float buttonHeight = 40f;
        static float buttonPadding = 4f;

        Texture normalTexture, highlightedTexture, pressedTexture, disabledTexture;
        Texture newEmptyGameObjectTexture;
        Texture resetPSRTexture;
        Texture unparentTexture;
        Texture dropToGroundTexture;
        Texture soloObjectsTexture;
        Texture unsoloObjectsTexture;


        GUIContent newEmptyGameObjectContent = new GUIContent();
        GUIContent resetPSRContent = new GUIContent();
        GUIContent unparentContent = new GUIContent();
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
                var window = GetWindow<FS_UnitySceneTools>();
                window.minSize = new Vector2(buttonHeight + (buttonPadding * 2f),  (buttons * buttonWidth) + buttonPadding);

                Debug.Log("Layout Direction Set To Vertical.");
            } else
            {
                var window = GetWindow<FS_UnitySceneTools>();
                window.minSize = new Vector2( (buttons * buttonWidth) + buttonPadding, buttonHeight + (buttonPadding * 2f));

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
            window.wantsMouseMove = true; // necessary in order for hover image states to work correctly / without delay
            window.titleContent = new GUIContent("Scene Tools");
            window.minSize = new Vector2( (buttons * buttonWidth) + buttonPadding, buttonHeight + (buttonPadding * 2f));
            window.Show();
        }

        void Awake()
        {
            
            normalTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/normal_40x40.png", typeof(Texture));
            pressedTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/pressed_40x40.png", typeof(Texture));
            highlightedTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/highlighted_40x40.png", typeof(Texture));
            disabledTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/disabled_40x40.png", typeof(Texture));

            FSUSTStyle.normal.background = (Texture2D)normalTexture;
            // FSUSTStyle.onNormal.background = (Texture2D)normalTexture;
            FSUSTStyle.hover.background = (Texture2D)highlightedTexture;
            // FSUSTStyle.onHover.background = (Texture2D)highlightedTexture;
            FSUSTStyle.active.background = (Texture2D)pressedTexture;
            // FSUSTStyle.onActive.background = (Texture2D)pressedTexture;


            newEmptyGameObjectTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/newgameobject_icon_40x40.png", typeof(Texture));
            newEmptyGameObjectContent.image = newEmptyGameObjectTexture;
            newEmptyGameObjectContent.tooltip = "New Game Object";
            

            resetPSRContent.tooltip = "Reset PSR";
            resetPSRTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/resetpsr_icon_40x40.png", typeof(Texture));
            resetPSRContent.image = resetPSRTexture;

            unparentContent.tooltip = "Unparent";
            unparentTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/unparent_icon_40x40.png", typeof(Texture));
            unparentContent.image = unparentTexture;

            dropToGroundContent.tooltip = "Drop To Ground";
            dropToGroundTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/droptoground_icon_40x40.png", typeof(Texture));
            dropToGroundContent.image = dropToGroundTexture;

            soloObjectsContent.tooltip = "Solo Objects";
            soloObjectsTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/solo_icon_40x40.png", typeof(Texture));
            soloObjectsContent.image = soloObjectsTexture;

            unsoloObjectsContent.tooltip = "Unsolo All";
            unsoloObjectsTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/unsolo_icon_40x40.png", typeof(Texture));
            unsoloObjectsContent.image = unsoloObjectsTexture;

        }

        private void OnGUI() {
            // force repaint window on mouse move so hover image states work correctly
            if (Event.current.type == EventType.MouseMove) Repaint();

            if (verticalLayoutDirection)
            {
                GUILayout.BeginArea(new Rect(buttonPadding, buttonPadding, buttonHeight + buttonPadding, (buttons * buttonWidth)));
                GUILayout.BeginVertical();
            } else {
                GUILayout.BeginArea(new Rect(buttonPadding, buttonPadding, (buttons * buttonWidth), buttonHeight + buttonPadding));
                GUILayout.BeginHorizontal();
            }

            #region New Empty Game Object
            if (GUILayout.Button(newEmptyGameObjectContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                if (Event.current.shift && (Selection.activeGameObject != null) ) // holding (SHIFT) insert as local child for each object in selection
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        GameObject go = new GameObject("GameObject");
                        Undo.RegisterCreatedObjectUndo(go, "Create New Child GameObject");
                        go.transform.SetParent(obj.transform, false);

                    }
                }
                else if (Event.current.alt && (Selection.activeGameObject != null) )// holding (ALT) insert as local parent for each object in selection
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        GameObject go = new GameObject("GameObject");
                        Undo.RegisterCreatedObjectUndo(go, "Create New Parent GameObject");

                        go.transform.SetParent(obj.transform.parent); // nest the GO in case the selection is a child
                        Undo.SetTransformParent(obj.transform,go.transform, "Set New Parent");

                    }
                }
                else if (Event.current.control && (Selection.activeGameObject != null) )// holding (control) insert as local parent to entire selection
                {
                    GameObject go = new GameObject("GameObject");
                    Undo.RegisterCreatedObjectUndo(go, "Create New Parent GameObject");
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        go.transform.SetParent(obj.transform.parent); // nest the GO in case the selection is a child
                        Undo.SetTransformParent(obj.transform,go.transform, "Set New Parent");
                    }
                }
                else
                {
                    GameObject go = new GameObject("GameObject");
                    go.transform.position = Vector3.zero;
                    Undo.RegisterCreatedObjectUndo(go, "Create New GameObject");
                }
            }
            #endregion

            #region Reset PSR
            if (GUILayout.Button(resetPSRContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {   
                if (Event.current.shift) // holding (SHIFT) reset P
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        Undo.RecordObject(obj.transform, "Reset P"); // save state for undo

                        obj.transform.localPosition = Vector3.zero;

                        PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
                    }
                }
                else if (Event.current.alt) // holding (ALT) reset S
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        Undo.RecordObject(obj.transform, "Reset S"); // save state for undo

                        obj.transform.localScale = Vector3.one;

                        PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
                    }
                }
                else if (Event.current.control) // holding (CONTROL) reset R
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        Undo.RecordObject(obj.transform, "Reset R"); // save state for undo

                        obj.transform.localEulerAngles = Vector3.zero;  

                        PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
                    }
                }
                else // reset PSR
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        Undo.RecordObject(obj.transform, "Reset PSR"); // save state for undo

                        obj.transform.localPosition = Vector3.zero;
                        obj.transform.localScale = Vector3.one;
                        obj.transform.localEulerAngles = Vector3.zero;  

                        PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
                    }
                }
            }
            #endregion

            #region Unparent
            if (GUILayout.Button(unparentContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    Undo.SetTransformParent(obj.transform, null, "Unparent");
                }
            }
            #endregion

            #region Drop To Ground
            if (GUILayout.Button(dropToGroundContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    Undo.RecordObject(obj.transform, "Drop To Ground"); // save state for undo
                    if (obj.GetComponent<Renderer>())
                    {
                        float obj_minY = obj.GetComponent<Renderer>().bounds.min.y;
                        obj.transform.position = new Vector3(obj.transform.position.x, 
                                                                obj.transform.position.y - obj_minY,
                                                                obj.transform.position.z);
                    } else
                    {
                        obj.transform.position = new Vector3(obj.transform.position.x, 
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
                        Undo.RegisterFullObjectHierarchyUndo(obj, "Solo Selection"); // save state for undo
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
                    Undo.RegisterFullObjectHierarchyUndo(obj, "Unsolo All"); // save state for undo
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
}
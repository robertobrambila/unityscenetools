using UnityEngine;
using UnityEditor;

namespace FS.Editor
{
    public class FS_UnitySceneTools : EditorWindow, IHasCustomMenu
    {
        GUIStyle FSUSTStyle = new GUIStyle();

        static int buttons = 6;
        static float buttonWidth = 40f;
        static float buttonHeight = 40f;
        static float buttonPadding = 4f;

        Texture normalTexture, highlightedTexture, pressedTexture;
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

            FSUSTStyle.normal.background = (Texture2D)normalTexture;
            FSUSTStyle.hover.background = (Texture2D)highlightedTexture;
            FSUSTStyle.active.background = (Texture2D)pressedTexture;


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
            forceRepaint();

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
                        Undo.RecordObject(obj.transform, "Reset P");

                        obj.transform.localPosition = Vector3.zero;

                        PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
                    }
                }
                else if (Event.current.alt) // holding (ALT) reset S
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        Undo.RecordObject(obj.transform, "Reset S"); 

                        obj.transform.localScale = Vector3.one;

                        PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
                    }
                }
                else if (Event.current.control) // holding (CONTROL) reset R
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        Undo.RecordObject(obj.transform, "Reset R"); 

                        obj.transform.localEulerAngles = Vector3.zero;  

                        PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
                    }
                }
                else // reset PSR
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
                    Undo.RecordObject(obj.transform, "Drop To Ground");
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
                foreach (GameObject obj in FindObjectsOfType<GameObject>())
                {
                    if (!Selection.Contains (obj)) // only apply to non-selected objects
                    {
                        if (!Selection.activeTransform.IsChildOf(obj.transform)) // don't apply to parent of selection else hierarchy will appear empty
                        {
                            Undo.RegisterFullObjectHierarchyUndo(obj, "Solo Selection");
                            obj.hideFlags = HideFlags.HideInHierarchy;
                            if (obj.GetComponent<Renderer>()) obj.GetComponent<Renderer>().enabled = false;

                            // used to manually refresh Hierarchy Window
                            EditorApplication.RepaintHierarchyWindow ();
                            EditorApplication.DirtyHierarchyWindowSorting();

                        }
                    }
                }

                svm.Isolate(Selection.gameObjects, true);
            }
            
            if (GUILayout.Button(unsoloObjectsContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                var svm = SceneVisibilityManager.instance;
                foreach (GameObject obj in FindObjectsOfType<GameObject>())
                {
                    Undo.RegisterFullObjectHierarchyUndo(obj, "Unsolo All");
                    obj.hideFlags = HideFlags.None;
                    if (obj.GetComponent<Renderer>()) obj.GetComponent<Renderer>().enabled = true;
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

        // force repaint window on mouse move so hover image states work correctly
        // if we don't call this ongui, the highlight state gets a delay after relaunching unity or exiting Play mode
        private void forceRepaint()
        {
            var window = GetWindow<FS_UnitySceneTools>();
            if (Event.current.type == EventType.MouseMove) Repaint();
        }
    }
}
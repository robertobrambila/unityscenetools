using UnityEngine;
using UnityEditor;
using FS.Editor;

namespace FS.Editor
{
    public class FS_UnitySceneTools : EditorWindow, IHasCustomMenu
    {
        GUIStyle FSUSTStyle = new GUIStyle();

        static int buttons = 7;
        static float buttonWidth = 40f;
        static float buttonHeight = 40f;
        static float buttonPadding = 4f;

        Texture normalTexture, hoverTexture, activeTexture;
        Texture newEmptyGameObjectTexture;
        Texture resetPRSTexture;
        Texture annotationTexture;
        Texture unparentTexture;
        Texture dropTexture;
        Texture soloObjectsTexture;
        Texture unsoloObjectsTexture;


        GUIContent newEmptyGameObjectContent = new GUIContent();
        GUIContent resetPRSContent = new GUIContent();
        GUIContent annotationContent = new GUIContent();
        GUIContent unparentContent = new GUIContent();
        GUIContent dropContent = new GUIContent();
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
            activeTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/active_40x40.png", typeof(Texture));
            hoverTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/hover_40x40.png", typeof(Texture));

            FSUSTStyle.normal.background = (Texture2D)normalTexture;
            FSUSTStyle.hover.background = (Texture2D)hoverTexture;
            FSUSTStyle.active.background = (Texture2D)activeTexture;


            newEmptyGameObjectContent.tooltip = "New Game Object";
            newEmptyGameObjectTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/newgameobject_icon_40x40.png", typeof(Texture));
            newEmptyGameObjectContent.image = newEmptyGameObjectTexture;
            

            resetPRSContent.tooltip = "Reset PRS";
            resetPRSTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/resetPRS_icon_40x40.png", typeof(Texture));
            resetPRSContent.image = resetPRSTexture;

            annotationContent.tooltip = "Annotation";
            annotationTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/resetPRS_icon_40x40.png", typeof(Texture));
            annotationContent.image = annotationTexture;

            unparentContent.tooltip = "Unparent";
            unparentTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/unparent_icon_40x40.png", typeof(Texture));
            unparentContent.image = unparentTexture;

            dropContent.tooltip = "Drop";
            dropTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/drop_icon_40x40.png", typeof(Texture));
            dropContent.image = dropTexture;

            soloObjectsContent.tooltip = "Solo Objects";
            soloObjectsTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/solo_icon_40x40.png", typeof(Texture));
            soloObjectsContent.image = soloObjectsTexture;

            unsoloObjectsContent.tooltip = "Unsolo All";
            unsoloObjectsTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/fs_unityscenetools/unsolo_icon_40x40.png", typeof(Texture));
            unsoloObjectsContent.image = unsoloObjectsTexture;

        }

        public void Update()
        {   
            // There doesn't seem like a cleaner/more efficient way of
            // keeping a responsive UI (mainly for the hover state of buttons).
            // Considering moving this to the OnInspectorUpdate method,
            // since that refreshes at 10f/s, though that makes it "feel" slow.
            Repaint();
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
            if (GUILayout.Button(newEmptyGameObjectContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                if (Event.current.shift && (Selection.activeGameObject != null) ) // holding (SHIFT) 
                {
                    FS_NewGameObject.createAsChild();
                }
                else if (Event.current.alt && (Selection.activeGameObject != null) )// holding (ALT)
                {
                    FS_NewGameObject.createAsParents();
                }
                else if (Event.current.control && (Selection.activeGameObject != null) )// holding (CTRL)
                {
                    FS_NewGameObject.createAsParent();
                }
                else 
                {
                    FS_NewGameObject.create();
                }
            }
            #endregion

            #region Reset PRS
            if (GUILayout.Button(resetPRSContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {   
                if (Event.current.shift) // holding (SHIFT)
                {
                    FS_ResetPRS.resetP();
                }
                else if (Event.current.alt) // holding (ALT)
                {
                    FS_ResetPRS.resetR();
                }
                else if (Event.current.control) // holding (CTRL)
                {
                    FS_ResetPRS.resetS();
                }
                else 
                {
                    FS_ResetPRS.reset();
                }
            }
            #endregion

            #region Annotation
            if (GUILayout.Button(annotationContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {   
                if (Event.current.shift) // holding (SHIFT)
                {
                    FS_Annotation.newSelection();
                }
                else 
                {
                    FS_Annotation.newEmpty();
                }
            }
            #endregion

            #region Unparent
            if (GUILayout.Button(unparentContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                if (Event.current.shift) // holding (SHIFT)
                {
                    FS_Unparent.toHierarchy();
                }
                else
                {
                    FS_Unparent.toRoot();
                }
            }
            #endregion

            #region Drop
            if (GUILayout.Button(dropContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                FS_Drop.toGround();
            }
            #endregion

            #region Solo / Unsolo
            if (GUILayout.Button(soloObjectsContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                FS_Solo.selection();
            }
            
            if (GUILayout.Button(unsoloObjectsContent, FSUSTStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                FS_Unsolo.all();
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

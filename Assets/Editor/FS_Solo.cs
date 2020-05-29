using UnityEngine;
using UnityEditor;

namespace FS.Editor
{
    public class FS_Solo
    {
        // isolate selection in scene and hierarchy
        public static void selection()
        {
            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
            {
                if (!Selection.Contains (obj)) // don't include selection
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
            
            SceneVisibilityManager.instance.Isolate(Selection.gameObjects, true);
        }
    }
}
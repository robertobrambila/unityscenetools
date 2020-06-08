using UnityEngine;
using UnityEditor;

namespace FS.Editor
{
    public class FS_Unsolo
    {
        public static void all()
        {
            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
            {
                Undo.RegisterFullObjectHierarchyUndo(obj, "Unsolo All");
                obj.hideFlags = HideFlags.None;
                if (obj.GetComponent<Renderer>()) obj.GetComponent<Renderer>().enabled = true;
            }
            
            SceneVisibilityManager.instance.ExitIsolation();
        }
    }
}
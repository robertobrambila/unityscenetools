using UnityEngine;
using UnityEditor;

namespace FS.Editor
{
    public class FS_Unparent
    {
        // unparent to scene root
        public static void toRoot()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.SetTransformParent(obj.transform, null, "Unparent To Root");
            }
        }

        // unparent but remain in any existing nested hierarchy
        public static void toHierarchy()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                if (obj.transform.parent.parent != null) Undo.SetTransformParent(obj.transform, obj.transform.parent.parent, "Unparent Nested");
            }
        }
    }
}
using UnityEngine;
using UnityEditor;

namespace FS.Editor
{
    public class FS_Annotation
    {
        // 
        public static void newEmpty()
        {
            var go = FS_NewGameObject.create();

        }

        // 
        public static void newSelection()
        {

            foreach (GameObject obj in Selection.gameObjects)
            {
            //     Undo.RecordObject(obj.transform, "Reset PRS");

            //     obj.transform.localPosition = Vector3.zero;
            //     obj.transform.localScale = Vector3.one;
            //     obj.transform.localEulerAngles = Vector3.zero;  

            //     PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
            }

        }

    }
}
using UnityEngine;
using UnityEditor;

namespace FS.Editor
{
    public class FS_ResetPRS
    {
        // resets P, R, S for selected gameobject(s) to zero
        public static void reset()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Reset PRS");

                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.transform.localEulerAngles = Vector3.zero;  

                PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
            }
        }

        // resets Position for selected gameobject(s) to zero
        public static void resetP()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Reset P");

                obj.transform.localPosition = Vector3.zero;

                PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
            }
        }

        // resets Rotation for selected gameobject(s) to zero
        public static void resetR()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Reset R"); 

                obj.transform.localEulerAngles = Vector3.zero;  

                PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
            }
        }

        // resets Scale for selected gameobject(s) to zero
        public static void resetS()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Reset S"); 

                obj.transform.localScale = Vector3.one;

                PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
            }
        }
    }
}
using UnityEngine;
using UnityEditor;
using FS.Scripts;

namespace FS.Editor
{
    public class FS_NewAnnotation
    {
        // create new annotation gameobject
        public static void create()
        {
            var annotationGO = FS_NewGameObject.create("Annotation");
            annotationGO.AddComponent<FS_Annotation>();
        }

        // add new annotation to each selected gameobject
        public static void add()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.AddComponent<FS_Annotation>(obj);
            }
        }

    }
}
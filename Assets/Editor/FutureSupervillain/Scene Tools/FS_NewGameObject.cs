using UnityEngine;
using UnityEditor;

namespace FS.Editor
{
    public class FS_NewGameObject
    {
        // create & return new gameobject at world zero
        public static GameObject create()
        {
            GameObject go = new GameObject("GameObject");
            go.transform.position = Vector3.zero;
            Undo.RegisterCreatedObjectUndo(go, "Create New GameObject");

            return go;
        }

        // create & return new gameobject at world zero with a custom name
        public static GameObject create(string _name)
        {
            GameObject go = new GameObject(_name);
            go.transform.position = Vector3.zero;
            Undo.RegisterCreatedObjectUndo(go, "Create New " + _name);

            return go;
        }

        // create new gameobject as local child for each object in selection
        public static void createAsChild()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                GameObject go = new GameObject("GameObject");
                Undo.RegisterCreatedObjectUndo(go, "Create New Child GameObject");
                go.transform.SetParent(obj.transform, false);
            }
        }

        //  create new gameobject as local parent to entire selection
        public static void createAsParent()
        {
            GameObject go = new GameObject("GameObject");
            Undo.RegisterCreatedObjectUndo(go, "Create New Parent GameObject");
            foreach (GameObject obj in Selection.gameObjects)
            {
                go.transform.SetParent(obj.transform.parent); // preserves any nested hierarchy
                Undo.SetTransformParent(obj.transform,go.transform, "Set New Parent");
            }
        }

        // create new gameobject as local parent for each object in selection
        public static void createAsParents()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                GameObject go = new GameObject("GameObject");
                Undo.RegisterCreatedObjectUndo(go, "Create New Parent GameObjects");

                go.transform.SetParent(obj.transform.parent); // preserves any nested hierarchy
                Undo.SetTransformParent(obj.transform,go.transform, "Set New Parent");
            }
        }

    }
}
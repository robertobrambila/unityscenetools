using UnityEngine;
using UnityEditor;

namespace FS.Editor
{
    public class FS_Drop
    {
        public static void toGround()
        {
            foreach (GameObject obj in Selection.gameObjects)
                {
                    Undo.RecordObject(obj.transform, "Drop To Ground");
                    
                    // if obj has mesh, get lowest point and use that to set on ground
                    if (obj.GetComponent<Renderer>()) 
                    {
                        float obj_minY = obj.GetComponent<Renderer>().bounds.min.y;
                        obj.transform.position = new Vector3(obj.transform.position.x, 
                                                                obj.transform.position.y - obj_minY,
                                                                obj.transform.position.z);
                    } else // set Y = 0
                    {
                        obj.transform.position = new Vector3(obj.transform.position.x, 
                                                                0,
                                                                obj.transform.position.z);
                    }
                    PrefabUtility.RecordPrefabInstancePropertyModifications(obj.transform);
                }
        }
    }
}
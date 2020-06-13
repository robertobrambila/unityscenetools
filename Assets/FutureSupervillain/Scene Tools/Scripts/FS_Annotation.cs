using UnityEngine;

namespace FS.Scripts {
    public class FS_Annotation : MonoBehaviour
    {
        [HideInInspector]
        public string annotationTextArea = "";

        [HideInInspector]
        public Vector2 scrollPos;

        [HideInInspector]
        public string URL ="https://";
    }
}
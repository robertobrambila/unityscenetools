﻿using UnityEngine;

namespace FS.Scripts {
    public class FS_Annotation : MonoBehaviour
    {
        [TextArea]
        public string Annotation = "";

        [HideInInspector]
        public string URL ="https://";
    }
}
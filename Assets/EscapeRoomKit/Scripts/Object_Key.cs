using UnityEngine;

namespace EscapeRoomKit
{
    public class Object_Key : Object_Item
    {
        [Header("Key Setting")]
        [SerializeField] bool isReusable;

        public bool IsKeyReusable() { return isReusable; }

    }
}

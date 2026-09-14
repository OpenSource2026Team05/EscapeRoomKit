using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomKit
{
    public class Object_Cl_Locked : MonoBehaviour
    {
        [Header("Unlock Events")]
        public UnityEvent UnlockEvent;

        public int num;
        public int count = 0;

        public void SetCount()
        {
            count++;
            if (count == num) UnlockEvent?.Invoke();
        }
    }
}

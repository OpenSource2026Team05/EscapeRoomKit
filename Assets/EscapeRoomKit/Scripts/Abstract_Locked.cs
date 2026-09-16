using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomKit
{
    public abstract class ALocked: MonoBehaviour
    {
        protected bool isLocked = true;

        [Header("Unlock Event")]
        public UnityEvent UnlockEvent;

        public abstract bool TryUnlock(IPlayer player);

        public void Unlock()
        {
            isLocked = false;

            UnlockEvent?.Invoke();
        }
        public bool IsLocked() { return isLocked; }
    }
}

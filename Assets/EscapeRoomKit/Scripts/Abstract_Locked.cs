using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomKit
{
    public abstract class ALocked : MonoBehaviour, IRayInteractable
    {
        protected bool isLocked = true;

        [Header("Player")]
        [SerializeField] protected GameObject player;

        [Header("Unlock Event")]
        public UnityEvent UnlockEvent;

        public void OnRayEnter() { }
        public void OnRayStay() { }
        public void OnRayExit() { }
        public void OnRayClick() => CheckLock();

        public abstract bool CheckLock();

        public void UnLock()
        {
            isLocked = false;

            UnlockEvent?.Invoke();
        }
        public bool IsLocked() { return isLocked; }
    }
}

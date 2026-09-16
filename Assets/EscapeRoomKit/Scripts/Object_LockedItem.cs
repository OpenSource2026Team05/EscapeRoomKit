using UnityEngine;

namespace EscapeRoomKit
{
    public class Object_LockedItem : MonoBehaviour, IRayPlayer
    {
        [Header("잠김")]
        public bool isLocked;
        public ALocked locked_type;

        [Header("Player")]
        IPlayer player;

        public void OnRayClick()
        {
            if (isLocked && locked_type.TryUnlock(player))
            {
                isLocked = false;
            }
        }

        public void AllocPlayer(IPlayer player) { this.player = player; }

        public void ClearPlayer() { this.player = null; }
    }
}

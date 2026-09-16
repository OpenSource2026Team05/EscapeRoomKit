using UnityEngine;

namespace EscapeRoomKit
{
    public interface IRayInteractable
    {
        void OnRayClick();
    }

    public interface IRayEnterExit: IRayInteractable
    {
        void OnRayEnter();
        void OnRayExit();
    }

    public interface IRayPlayer: IRayInteractable
    {
        void AllocPlayer(IPlayer player);

        void ClearPlayer();
    }
}

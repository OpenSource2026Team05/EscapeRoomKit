using UnityEngine;

namespace EscapeRoomKit
{

    public class Player_Pivot_Footstep : MonoBehaviour
    {
        [Header("Player_Move")]
        public Player_Move player;

        public void PlayFootstepSound()
        {
            player.PlayFootStepSound();
        }
    }
}

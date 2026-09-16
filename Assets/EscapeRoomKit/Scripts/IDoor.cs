using UnityEngine;

namespace EscapeRoomKit
{
    public abstract class Door : MonoBehaviour
    {
        [Header("Animation")]
        public Animator animator;

        [Header("사운드")]
        public AudioClip audio_open;

        protected bool isOpen = false;

        protected Play_Audio audio_player;

        public void OpenDoor()
        {
            if (animator == null || isOpen) return;

            isOpen = true;

            animator.SetInteger("Open", 1);

            audio_player?.PlayAudio(audio_open);
        }

        public void CloseDoor()
        {
            if (animator == null || !isOpen) return;

            isOpen = false;

            animator.SetInteger("Open", 0);

            audio_player?.PlayAudio(audio_open);
        }
    }
}

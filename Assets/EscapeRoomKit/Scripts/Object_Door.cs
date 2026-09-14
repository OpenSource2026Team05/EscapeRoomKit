using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace EscapeRoomKit
{
    public class Object_Door : MonoBehaviour, IRayInteractable
    {
        [Header("Animation")]
        public Animator animator;

        [Header("잠김")]
        public bool isLocked;

        [Header("사운드")]
        public AudioClip audio_open;
        public AudioClip audio_locked;

        bool isOpen = false;

        private Play_Audio audio_player;

        void Start()
        {
            audio_player = GetComponent<Play_Audio>();
        }

        public void OnRayEnter() { }
        public void OnRayStay() { }
        public void OnRayExit() { }
        public void OnRayClick()
        {
            if (isOpen) CloseDoor();
            else OpenDoor();
        }

        public void UnlockDoor()
        {
            isLocked = false;
            Debug.Log("비밀번호 일치! 문이 열립니다.");
            OpenDoor();
        }

        public void OpenDoor()
        {
            if (animator == null || isOpen) return;
            if (isLocked)
            {
                Object_KeyLocked keylocked = GetComponent<Object_KeyLocked>();
                if (keylocked != null)
                {
                    keylocked.OnInteract();
                    return;
                }

                audio_player?.PlayAudio(audio_locked);
                return;
            }

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

using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace EscapeRoomKit
{
    public class Object_Door_Locked : Door, IRayPlayer
    {
        [Header("잠김")]
        public bool isLocked;
        public AudioClip audio_locked;
        [SerializeReference] public ALocked locked_type;


        [Header("Player")]
        protected IPlayer player;

        public void OnRayClick()
        {
            if (isLocked) CheckDoor();

            else if (isOpen) CloseDoor();
            else OpenDoor();
        }

        public void AllocPlayer(IPlayer player) { this.player = player; }

        public void ClearPlayer() { this.player = null; }

        public void CheckDoor()
        {
            if (locked_type.TryUnlock(player))
            {
                OpenDoor();

                isLocked = false;
            }
            else
            {
                audio_player.PlayAudio(audio_locked);
            }
        }

        public void UnlockDoor()
        {
            isLocked = false;
            Debug.Log("비밀번호 일치! 문이 열립니다.");
            OpenDoor();
        }
        
    }
}

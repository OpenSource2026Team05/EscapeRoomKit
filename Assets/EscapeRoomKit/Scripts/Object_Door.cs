using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace EscapeRoomKit
{
    public class Object_Door_Locked : Door
    {
        [Header("잠김")]
        public bool isLocked;
        public AudioClip audio_locked;
        public ALocked locked_type;

        public override void OnRayClick()
        {
            if (isLocked) CheckDoor();

            else if (isOpen) CloseDoor();
            else OpenDoor();
        }

        public void CheckDoor()
        {
            if (locked_type.CheckLock())
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

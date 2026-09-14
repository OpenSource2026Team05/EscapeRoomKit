using TMPro;
using UnityEngine;
using System.Collections;

namespace EscapeRoomKit
{
    public class Object_Item : MonoBehaviour, IRayInteractable
    {

        [Header("Player Character")]
        public GameObject player;

        [Header("사운드")]
        public AudioClip[] audio_grap;

        private Play_Audio audio_player;

        void Start()
        {
            audio_player = GetComponent<Play_Audio>();
        }

        public void UseItem(GameObject target) { }

        public void OnRayEnter() { }
        public void OnRayStay() { }
        public void OnRayExit() { }
        public virtual void OnRayClick() => OnGrab();

        public void OnGrab()
        {
            player.GetComponent<Player_Grab>().Grab(this);

            if (audio_player != null)
            {
                int random_id = Random.Range(0, audio_grap.Length);
                audio_player.PlayAudio(audio_grap[random_id]);
            }
        }
    }
}

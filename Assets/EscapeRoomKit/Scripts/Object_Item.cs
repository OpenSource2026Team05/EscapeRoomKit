using TMPro;
using UnityEngine;
using System.Collections;

namespace EscapeRoomKit
{
    public class Object_Item : MonoBehaviour, IRayPlayer
    {

        [Header("Player Character")]
        protected IPlayer player;

        [Header("사운드")]
        public AudioClip[] audio_grap;

        private Play_Audio audio_player;

        void Start()
        {
            audio_player = GetComponent<Play_Audio>();
        }

        public void UseItem(GameObject target) { }

        public void AllocPlayer(IPlayer player) { this.player = player; }

        public void ClearPlayer() { this.player = null; }

        public virtual void OnRayClick() => OnGrab();

        public void OnGrab()
        {
            player.Grab(this.gameObject);

            if (audio_player != null)
            {
                int random_id = Random.Range(0, audio_grap.Length);
                audio_player.PlayAudio(audio_grap[random_id]);
            }
        }
    }
}

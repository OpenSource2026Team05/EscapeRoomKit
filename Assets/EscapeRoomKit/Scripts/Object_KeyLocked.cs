using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace EscapeRoomKit
{
    public class Object_KeyLocked : MonoBehaviour, IRayInteractable
    {
        private bool isLocked = true;

        [Header("Player")]
        [SerializeField] GameObject player;

        [Header("해제 이벤트")]
        public UnityEvent UnlockEvent;

        [Header("Key Object")]
        [SerializeField] List<GameObject> key;

        public void OnRayEnter() { }
        public void OnRayStay() { }
        public void OnRayExit() { }
        public void OnRayClick() => OnInteract();


        public void OnInteract()
        {
            if (isLocked)
            {
                GameObject player_grabbing = player.GetComponent<Player_Grab>().GetGrabbing();
                if (player_grabbing != null)
                {
                    CheckKey(player_grabbing);
                }
                else
                {
                    Debug.Log("문이 잠겨있습니다. 열쇠가 필요합니다.");
                }
            }
        }

        public void CheckKey(GameObject grabbing)
        {
            Object_Key key_grabbing = grabbing.GetComponent<Object_Key>();
            if (key_grabbing == null) return;

            foreach(GameObject k in key)
            {
                if(k == grabbing)
                {
                    UseKey(key_grabbing);
                    return;
                }
            }
            Debug.Log("문이 잠겨있습니다. 열쇠가 필요합니다.");
        }

        public void UseKey(Object_Key k)
        {
            if (!k.IsKeyReusable())
            {
                player.GetComponent<Player_Grab>().RemoveGrabbing();
                k.gameObject.SetActive(false);
            }

            Debug.Log("열쇠로 문을 열었습니다.");
            UnlockEvent?.Invoke();
        }
    }
}

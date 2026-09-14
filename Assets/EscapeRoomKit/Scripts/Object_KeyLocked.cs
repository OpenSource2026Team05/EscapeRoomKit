using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace EscapeRoomKit
{
    public class Object_KeyLocked : ALocked
    {
        [Header("Key Object")]
        [SerializeField] List<GameObject> key;

        public override bool CheckLock()
        {
            if (isLocked)
            {
                GameObject player_grabbing = player.GetComponent<Player_Grab>().GetGrabbing();
                if (player_grabbing != null)
                {
                    return CheckKey(player_grabbing);
                }
                else
                {
                    return false;
                }
            }

            else return true;
        }

        public bool CheckKey(GameObject grabbing)
        {
            Object_Key key_grabbing = grabbing.GetComponent<Object_Key>();
            if (key_grabbing == null) return false;

            foreach(GameObject k in key)
            {
                if(k == grabbing)
                {
                    UseKey(key_grabbing);
                    return true;
                }
            }

            return false;
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

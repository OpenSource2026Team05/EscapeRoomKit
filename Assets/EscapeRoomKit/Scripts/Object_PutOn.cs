using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace EscapeRoomKit
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Object_PutOn : MonoBehaviour, IRayInteractable
    {
        [Header("Player")]
        [SerializeField] GameObject player;

        [Header("투명 머터리얼")]
        [SerializeField] Material mat_trans;

        [Header("열쇠")]
        [SerializeField] List<GameObject> key;

        [Header("사운드")]
        public AudioClip audio_puton;

        private Play_Audio audio_player;
        private MeshRenderer mesh;

        protected GameObject current;

        void Start()
        {
            current = null;

            audio_player = GetComponent<Play_Audio>();
            mesh = GetComponent<MeshRenderer>();
            mesh.enabled = false;
        }

        public void OnRayEnter() => OnEnter();
        public void OnRayStay() { }
        public void OnRayExit() => OnExit();
        public void OnRayClick() => OnInteract();

        public virtual void OnPutDown() { }

        public virtual void OnPickUp() { }

        public void OnInteract()
        {
            GameObject player_grabbing = player.GetComponent<Player_Grab>().GetGrabbing();

            if (current == null && player_grabbing != null)
            {
                if (HasKey(player_grabbing)) PutDown();
            }
            else if (current!=null && player_grabbing == null)
            {
                if(current.GetComponent<Object_Key>().IsKeyReusable()) PickUp();
            }
        }

        public bool HasKey(GameObject grabbing)
        {
            Object_Key key_grabbing = grabbing.GetComponent<Object_Key>();
            if (key_grabbing == null) return false;

            foreach (GameObject k in key)
            {
                if (k == grabbing)
                {
                    return true;
                }
            }
            return false;
        }


        void PutDown()
        {
            mesh.enabled = false;

            StartCoroutine(AfterPutDown(0.5f, player.GetComponent<Player_Grab>().GetGrabbing()));

            player.GetComponent<Player_Grab>().PutDown(this.transform);
        }

        void PickUp()
        {
            Object_Item grab = current.GetComponent<Object_Item>();
            player.GetComponent<Player_Grab>().Grab(grab);

            StartCoroutine(AfterPickUp(0.5f));
        }

        public void OnEnter()
        {
            GameObject player_grabbing = player.GetComponent<Player_Grab>().GetGrabbing();

            if (current == null && player_grabbing != null)
            {
                if (HasKey(player_grabbing))
                {
                    mesh.material = mat_trans;
                    mesh.enabled = true;
                }
            }
        }

        public void OnExit()
        {
            mesh.enabled = false;
        }

        IEnumerator AfterPutDown(float delay, GameObject current)
        {
            yield return new WaitForSeconds(delay);

            audio_player?.PlayAudio(audio_puton);

            this.current = current;
            OnPutDown();
        }

        IEnumerator AfterPickUp(float delay)
        {
            yield return new WaitForSeconds(delay);

            mesh.material = mat_trans;
            mesh.enabled = true;

            OnPickUp();
            current = null;
        }
    }
}

using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EscapeRoomKit
{
    public class Object_FixCamera : MonoBehaviour, IRayInteractable
    {
        [Header("Fix Position")]
        public Transform fixPoint;

        public Scene_UI_Manager SceneUI;

        [Header("Player")]
        public Player_FixCamera player;


        public void OnRayEnter() { }
        public void OnRayStay() { }
        public void OnRayExit() { }
        public void OnRayClick() => FixCamera();

        public virtual void OnFixed() { }

        public virtual void OnUnFixed() { }

        public void FixCamera()
        {
            if (fixPoint == null || player.isPlayerFix()) return;

            player.FixCamera(fixPoint.position, fixPoint.forward, this.gameObject);

            OnFixed();
        }

        public void UnFixCamera()
        {
            OnUnFixed();
        }
    }
}

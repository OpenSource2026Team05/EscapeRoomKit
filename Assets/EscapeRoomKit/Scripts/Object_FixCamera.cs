using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EscapeRoomKit
{
    public abstract class Object_FixCamera : MonoBehaviour, IRayPlayer
    {
        [Header("Fix Position")]
        public Transform fixPoint;

        public Scene_UI_Manager SceneUI;

        [Header("Player")]
        protected IPlayer player;

        public void OnRayClick() => FixCamera();

        public void AllocPlayer(IPlayer player) { this.player = player; }

        public void ClearPlayer() {  this.player = null; }

        public abstract void OnFixed();

        public abstract void OnUnFixed();

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

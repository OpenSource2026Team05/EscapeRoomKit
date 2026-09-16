using UnityEngine;

namespace EscapeRoomKit
{
    public class PlayerFacade: MonoBehaviour, IPlayer
    {
        [Header("Player Components")]
        [SerializeField] Player_Move move;
        [SerializeField] Player_Interaction interact;
        [SerializeField] Player_Grab grab;
        [SerializeField] Player_FixCamera fixCamera;

        public void Grab(GameObject obj) { grab.Grab(obj); }

        public void Release() { grab.Release(); }

        public GameObject GetGrabbing() { return grab.GetGrabbing(); }

        public void RemoveGrabbing() {  grab.RemoveGrabbing(); }

        public GameObject PutDown(Transform targetPos) {  return grab.PutDown(targetPos); }

        public void SetMoveLock(bool locked) { move.SetMoveLock(locked); }

        public void DisableInteract() { interact.DisableInteract(); }

        public void EnableInteract() { interact.EnableInteract(); }


        public bool isPlayerFix()
        {
            return fixCamera.isPlayerFix();
        }

        public void FixCamera(Vector3 targetPos, Vector3 targetRot, GameObject fixObject)
        {
            fixCamera.FixCamera(targetPos, targetRot, fixObject);
        }

        public void UnFixCamera() {  fixCamera.UnFixCamera(); }
    }
}

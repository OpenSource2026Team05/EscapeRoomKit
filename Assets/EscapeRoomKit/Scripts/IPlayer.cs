using UnityEngine;

namespace EscapeRoomKit
{
    public interface IPlayer
    {
        void Grab(GameObject grab);

        void Release();

        GameObject GetGrabbing();

        void RemoveGrabbing();

        GameObject PutDown(Transform targetPos);

        void SetMoveLock(bool locked);

        void DisableInteract();

        void EnableInteract();

        bool isPlayerFix();

        void FixCamera(Vector3 targetPos, Vector3 targetRot, GameObject fixObject);

        void UnFixCamera();
    }
}

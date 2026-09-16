using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRoomKit
{
    public class Player_FixCamera : MonoBehaviour
    {
        [Header("Camera")]
        public Transform CameraPivot;
        public float targetTime;

        [Header("Player Hand")]
        public GameObject hand;

        [Header("UI Settings")]
        public Scene_UI_Manager SceneUI;

        [Header("Fix Object")]
        public Object_FixCamera fixObject;

        Vector3 originalPos;
        Vector3 originalRot;

        Vector3 targetPos;
        Vector3 targetRot;

        InputAction exit;

        void Start()
        {
            fixObject = null;
        }

        public bool isPlayerFix()
        {
            return (fixObject != null);
        }


        public void FixCamera(Vector3 targetPos, Vector3 targetRot, GameObject fixObject)
        {
            if (this.fixObject != null) return;

            SceneUI.SetActiveCursor(false);
            SceneUI.UnlockPointer();

            this.targetPos = targetPos;
            this.targetRot = targetRot;

            StartCoroutine(MoveToFixPosition());

            this.fixObject = fixObject.GetComponent<Object_FixCamera>();
        }

        public void UnFixCamera()
        {
            if (fixObject == null) return;

            SceneUI.SetActiveCursor(true);
            SceneUI.LockPointer();

            fixObject.OnUnFixed();

            fixObject = null;

            StartCoroutine(MoveToOriginalPosition());
        }

        void EnableExit()
        {
            if (exit == null) exit = InputSystem.actions.FindAction("Return");
            exit.performed -= ctx => Exit();
            exit.performed += ctx => Exit();
            exit.Enable();
        }

        void DisableExit()
        {
            if (exit == null) exit = InputSystem.actions.FindAction("Return");
            exit.performed -= ctx => Exit();
            exit.Disable();
        }

        void Exit()
        {
            UnFixCamera();
        }

        IEnumerator MoveToFixPosition()
        {
            GetComponent<IPlayer>().SetMoveLock(true);

            GetComponent<IPlayer>().DisableInteract();

            if (hand != null) hand.GetComponent<Collider>().isTrigger = true;

            originalPos = CameraPivot.position;
            originalRot = CameraPivot.forward;

            float t = 0f;

            while (t < targetTime)
            {
                t += Time.deltaTime;
                float tp = t / targetTime;

                CameraPivot.position = Vector3.Lerp(originalPos, targetPos, tp);

                CameraPivot.forward = Vector3.Lerp(originalRot, targetRot, tp);

                yield return null;
            }

            EnableExit();
        }

        IEnumerator MoveToOriginalPosition()
        {
            DisableExit();

            float t = 0f;

            while (t < targetTime)
            {
                t += Time.deltaTime;
                float tp = t / targetTime;

                CameraPivot.position = Vector3.Lerp(targetPos, originalPos, tp);

                CameraPivot.forward = Vector3.Lerp(targetRot, originalRot, tp);

                yield return null;
            }

            GetComponent<IPlayer>().SetMoveLock(false);

            GetComponent<IPlayer>().EnableInteract();

            if (hand != null) hand.GetComponent<Collider>().isTrigger = false;
        }
    }
}

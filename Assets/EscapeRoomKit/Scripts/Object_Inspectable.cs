using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRoomKit
{
    public class Object_Inspectable : Object_Item
    {
        [Header("Inspect Settings")]
        public float targetTime = 0.5f;
        public float inspectDistance = 1f;
        public bool isGrabbable;

        [Header("UI Settings")]
        public bool usePanel;
        [TextArea(3, 8)]
        public string disc;
        public Scene_UI_Manager SceneUI;

        [Header("Animation")]
        public Animator animator;

        [Header("Drag Setting")]
        public float rotateSpeed = 0.3f;

        InputAction examine;
        InputAction exit;

        private Vector3 originalPosition;
        private Quaternion originalRotation;

        private Collider col;
        private Rigidbody rigid;

        protected bool isInspecting = false;
        private bool isDragRotateEnabled = false;

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;

            col = GetComponent<Collider>();
            rigid = GetComponent<Rigidbody>();

            examine = InputSystem.actions.FindAction("Examine");
            examine.Disable();

            exit = InputSystem.actions.FindAction("Return");
            exit.performed -= ctx => Exit();
            exit.Disable();
        }

        private void Update()
        {
            if (!isDragRotateEnabled) return;

            if (examine.WasPressedThisFrame())
            {
                Examine();
            }
        }
        public override void OnRayClick() => OnInspect();

        public void OnInspect()
        {
            if (!isInspecting)
            {
                StartCoroutine(MoveToInspectPosition());
                if(animator!=null) animator.SetTrigger("On");
            }
        }

        void Examine()
        {
            Vector2 ExamineInput = examine.ReadValue<Vector2>();
            float deltaX = ExamineInput.x;
            float deltaY = ExamineInput.y;

            transform.Rotate(Vector3.up, -deltaX * rotateSpeed * 100f * Time.deltaTime, Space.World);
            transform.Rotate(mainCamera.transform.right, deltaY * rotateSpeed * 100f * Time.deltaTime, Space.World);
        }

        public void Exit()
        {
            if (!isInspecting) return;

            if (animator != null) animator.SetTrigger("Off");

            isDragRotateEnabled = false;
            examine.Disable();
            exit.performed -= ctx => Exit();
            exit.Disable();

            if (usePanel)
            {
                SceneUI.SetActivePanel(2, false);
            }

            SceneUI.SetActiveCursor(true);
            SceneUI.LockPointer();

            if(!isGrabbable) StartCoroutine(ReturnToPosition());

            else
            {
                isInspecting = false;

                player.GetComponent<Player_Interaction>().EnableInteract();

                OnGrab();
            }
        }

        IEnumerator MoveToInspectPosition()
        {
            isInspecting = true;

            originalPosition = transform.position;
            originalRotation = transform.rotation;

            col.isTrigger = true;
            if (rigid != null) rigid.isKinematic = true;

            player.GetComponent<Player_Move>().SetMoveLock(true);

            player.GetComponent<Player_Interaction>().DisableInteract();

            Vector3 targetPos = mainCamera.transform.position + mainCamera.transform.forward * inspectDistance;
            Quaternion targetRot = Quaternion.LookRotation(mainCamera.transform.forward);

            Vector3 startPos = originalPosition;
            Quaternion startRot = originalRotation;

            float t = 0f;

            while (t < targetTime)
            {
                t += Time.deltaTime;
                float tp = t / targetTime;

                transform.position = Vector3.Lerp(startPos, targetPos, tp);
                transform.rotation = Quaternion.Lerp(startRot, targetRot, tp);

                yield return null;
            }

            if (usePanel)
            {
                SceneUI.ChangeText(0, disc);
                SceneUI.SetActivePanel(2, true);
            }

            SceneUI.SetActiveCursor(false);
            SceneUI.UnlockPointer();

            isDragRotateEnabled = true;

            col.isTrigger = false;

            examine.Enable();
            exit.performed += ctx => Exit();
            exit.Enable();
        }

        IEnumerator ReturnToPosition()
        {
            col.isTrigger = true;

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;

            Vector3 targetPos = originalPosition;
            Quaternion targetRot = originalRotation;

            float t = 0f;

            while (t < targetTime)
            {
                t += Time.deltaTime;
                float tp = t / targetTime;

                transform.position = Vector3.Lerp(startPos, targetPos, tp);
                transform.rotation = Quaternion.Lerp(startRot, targetRot, tp);

                yield return null;
            }

            col.isTrigger = false;

            isInspecting = false;
            player.GetComponent<Player_Move>().SetMoveLock(false);

            player.GetComponent<Player_Interaction>().EnableInteract();
        }
    }
}

using UnityEngine;
using Cursor = UnityEngine.Cursor;
using UnityEngine.InputSystem;

namespace EscapeRoomKit
{
    [RequireComponent(typeof(CharacterController))]
    public class Player_Move : MonoBehaviour
    {
        CharacterController Character;

        [Header("Camera")]
        public Transform CameraPivot;
        public float mouseSensitivity;
        float yaw;
        float pitch;

        [Header("Body")]
        public Transform bodyMesh;
        public float crouchMeshScaleY = 0.5f;

        [Header("Move")]
        public float walkSpeed = 1.5f;
        public float runSpeed = 4.0f;
        public float crouchSpeed = 0.8f;
        public float jumpHeight = 1f;
        public float gravity = -9.81f;
        public float friction = 0.9f;

        [Header("Crouch")]
        public float crouchHeight = 1.0f;
        public float crouchCameraY = 0.5f;
        private float originalHeight;
        private Vector3 originalCameraPos;
        private Vector3 originalMeshScale;

        [Header("Head Bobbing")]
        public Animator cameraAnimator;

        [Header("Audio")]
        private Play_Audio audioSource;
        public AudioClip[] walkSounds;

        Vector3 Velocity;
        Vector2 MoveInput;
        Vector2 LookInput;

        InputActionMap player_input;
        InputAction lookAction;
        InputAction moveAction;
        InputAction runAction;
        InputAction hideAction;
        InputAction jumpAction;

        bool moveLocked = false;
        bool isRunning = false;
        bool isHiding = false;

        void Awake()
        {
            yaw = 90;

            Character = GetComponent<CharacterController>();

            audioSource = GetComponent<Play_Audio>();

            originalHeight = Character.height;

            if (CameraPivot == null)
                CameraPivot = transform.Find("CameraRig/CameraPivot");

            if (CameraPivot != null)
                originalCameraPos = CameraPivot.localPosition;

            if (bodyMesh != null)
                originalMeshScale = bodyMesh.localScale;

            player_input = InputSystem.actions.FindActionMap("PC_Player");
            lookAction = InputSystem.actions.FindAction("Look");
            moveAction = InputSystem.actions.FindAction("Move");
            runAction = InputSystem.actions.FindAction("Run");
            hideAction = InputSystem.actions.FindAction("Hide");
            jumpAction = InputSystem.actions.FindAction("Jump");
        }

        private void OnEnable()
        {
            player_input.Enable();
        }

        private void OnDisable()
        {
            player_input.Disable();
        }

        void Update()
        {
            if (moveLocked) return;

            Look();

            Move();
            Character.Move(Velocity * Time.deltaTime);

            if (lookAction != null) LookInput = lookAction.ReadValue<Vector2>();
            if (moveAction != null) MoveInput = moveAction.ReadValue<Vector2>();
        }

        void Look()
        {
            yaw += LookInput.x * mouseSensitivity;
            pitch -= LookInput.y * mouseSensitivity;

            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.rotation = Quaternion.Euler(0, yaw, 0);
            if (CameraPivot != null)
                CameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
        }

        void Move()
        {
            isRunning = (runAction != null && runAction.IsPressed());
            isHiding = (hideAction != null && hideAction.IsPressed());
            bool isJumping = (jumpAction != null && jumpAction.triggered);

            float currentSpeed = walkSpeed;
            float targetHeight = originalHeight;
            float targetCameraY = originalCameraPos.y;
            float targetMeshScaleY = originalMeshScale.y;

            if (isHiding)
            {
                currentSpeed = crouchSpeed;
                targetHeight = crouchHeight;
                targetCameraY = crouchCameraY;
                targetMeshScaleY = crouchMeshScaleY;
            }
            else if (isRunning)
            {
                currentSpeed = runSpeed;
            }

            Character.height = Mathf.Lerp(Character.height, targetHeight, Time.deltaTime * 10f);
            Character.center = new Vector3(0, Character.height / 2f, 0);

            if (CameraPivot != null)
            {
                Vector3 camPos = CameraPivot.localPosition;
                camPos.y = Mathf.Lerp(camPos.y, targetCameraY, Time.deltaTime * 10f);
                CameraPivot.localPosition = camPos;
            }

            if (bodyMesh != null)
            {
                Vector3 meshScale = bodyMesh.localScale;
                meshScale.y = Mathf.Lerp(meshScale.y, targetMeshScaleY, Time.deltaTime * 10f);
                bodyMesh.localScale = meshScale;
                bodyMesh.localPosition = new Vector3(0, meshScale.y, 0);
            }

            Vector3 inputDir = transform.forward * MoveInput.y + transform.right * MoveInput.x;

            if (inputDir.sqrMagnitude > 1)
                inputDir.Normalize();

            if (inputDir.magnitude > 0)
            {
                Velocity.x = inputDir.x * currentSpeed;
                Velocity.z = inputDir.z * currentSpeed;
            }
            else
            {
                Velocity.x *= friction;
                Velocity.z *= friction;
            }

            if (Character.isGrounded && Velocity.y < 0)
            {
                Velocity.y = -2f;
            }

            if (!isHiding && isJumping && Character.isGrounded)
            {
                Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            Velocity.y += gravity * Time.deltaTime;

            if (cameraAnimator != null)
            {
                float currentMoveSpeed = Character.isGrounded ? new Vector2(Velocity.x, Velocity.z).magnitude : 0f;

                cameraAnimator.SetFloat("MoveSpeed", currentMoveSpeed, 0.1f, Time.deltaTime);
            }
        }

        public void SwitchMoveLock()
        {
            if (moveLocked) SetMoveLock(false);
            else SetMoveLock(true);
        }

        public void SetMoveLock(bool locked)
        {
            moveLocked = locked;

            if (cameraAnimator != null && locked)
            {
                cameraAnimator.SetFloat("MoveSpeed", 0);
            }
        }

        public void PlayFootStepSound()
        {
            if (audioSource == null) return;
            if (!Character.isGrounded || isHiding) return;

            if (walkSounds.Length > 0)
            {
                int random_id = Random.Range(0, walkSounds.Length);
                audioSource.PlayAudio(walkSounds[random_id]);
            }
        }
    }
}

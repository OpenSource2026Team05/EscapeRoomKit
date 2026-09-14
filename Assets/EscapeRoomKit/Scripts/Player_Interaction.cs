using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace EscapeRoomKit
{
    [RequireComponent(typeof(CharacterController))]
    public class Player_Interaction : MonoBehaviour
    {
        [Header("상호작용 최대 거리")]
        public float dist;

        [Header("화면 고정")]
        public Player_FixCamera fix;

        [Header("물건 잡기")]
        public Player_Grab grab;

        [Header("UI")]
        public Scene_UI_Manager SceneUI;

        InputAction interact;

        GameObject CurrentTarget;

        void Start()
        {
            CurrentTarget = null;
        }

        private void OnEnable()
        {
            interact = InputSystem.actions.FindAction("Interact");

            EnableInteract();
        }

        private void OnDisable()
        {
            DisableInteract();
        }

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, dist))
            {
                Debug.DrawLine(ray.origin, hit.point);

                var Interactable = hit.collider.gameObject;

                if (Interactable.GetComponent<IRayInteractable>() != null)
                {
                    if (CurrentTarget != Interactable)
                    {
                        CurrentTarget?.GetComponent<IRayInteractable>().OnRayExit();

                        CurrentTarget = Interactable;
                        CurrentTarget.GetComponent<IRayInteractable>().OnRayEnter();
                        SceneUI.SwitchCursor(true);
                    }

                    CurrentTarget.GetComponent<IRayInteractable>().OnRayStay();

                }
                else
                {
                    if (CurrentTarget != null)
                    {
                        CurrentTarget.GetComponent<IRayInteractable>().OnRayExit();
                        CurrentTarget = null;
                        SceneUI.SwitchCursor(false);
                    }
                }
            }
            else
            {
                if (CurrentTarget != null)
                {
                    CurrentTarget.GetComponent<IRayInteractable>().OnRayExit();
                    CurrentTarget = null;
                    SceneUI.SwitchCursor(false);
                }
            }
        }

        public void DisableInteract()
        {
            interact.performed -= ctx => Interact();

            interact.Disable();
        }

        public void EnableInteract()
        {
            interact.performed -= ctx => Interact();
            interact.performed += ctx => Interact();

            interact.Enable();
        }

        void Interact()
        {
            //if (fix.isPlayerFix())
            //{
            //    fix.fixObject.UnFixCamera();
            //    return;
            //}

            if (CurrentTarget != null)
            {
                CurrentTarget?.GetComponent<IRayInteractable>().OnRayClick();
            }
            else
            {
                grab.Release();
            }
        }
    }
}

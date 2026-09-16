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
                        if (CurrentTarget != null && CurrentTarget.GetComponent<IRayEnterExit>() != null)
                        {
                            CurrentTarget.GetComponent<IRayEnterExit>().OnRayExit();
                        }

                        CurrentTarget = Interactable;
                        if (CurrentTarget.GetComponent<IRayPlayer>() != null) CurrentTarget.GetComponent<IRayPlayer>().AllocPlayer(gameObject.GetComponent<IPlayer>());
                        if (CurrentTarget.GetComponent<IRayEnterExit>() != null) CurrentTarget.GetComponent<IRayEnterExit>().OnRayEnter();
                        SceneUI.SwitchCursor(true);
                    }

                }
                else
                {
                    if (CurrentTarget != null)
                    {
                        if (CurrentTarget.GetComponent<IRayEnterExit>() != null) CurrentTarget.GetComponent<IRayEnterExit>().OnRayExit();
                        if (CurrentTarget.GetComponent<IRayPlayer>() != null) CurrentTarget.GetComponent<IRayPlayer>().ClearPlayer();
                        CurrentTarget = null;
                        SceneUI.SwitchCursor(false);
                    }
                }
            }
            else
            {
                if (CurrentTarget != null)
                {
                    if (CurrentTarget.GetComponent<IRayEnterExit>() != null) CurrentTarget.GetComponent<IRayEnterExit>().OnRayExit();
                    if (CurrentTarget.GetComponent<IRayPlayer>() != null) CurrentTarget.GetComponent<IRayPlayer>().ClearPlayer();
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
            if (CurrentTarget != null)
            {
                CurrentTarget?.GetComponent<IRayInteractable>().OnRayClick();
            }
            else
            {
                GetComponent<IPlayer>().Release();
            }
        }
    }
}

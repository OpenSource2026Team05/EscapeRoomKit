using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EscapeRoomKit
{
    public abstract class Keypad: Object_FixCamera
    {
        [Header("Buttons")]
        [SerializeField] protected List<Object_Keypad_Button> buttons;
        [SerializeField] protected Object_Keypad_Button submit_button;
        [SerializeField] protected LayerMask button_layer;

        [Header("Unlock Event")]
        [SerializeField] protected UnityEvent UnlockEvent;

        [Header("Fail Event")]
        [SerializeField] protected UnityEvent FailEvent;

        protected bool isActive;

        protected InputAction click;

        protected Collider col;

        [Header("Sounds")]
        [SerializeField] protected List<AudioClip> clips;
        protected Play_Audio audio_player;
        public void InitButtons()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Init(i);
            }
            submit_button.Init(-1);
        }

        public void SetActiveInput(bool active)
        {
            isActive = active;

            if (active)
            {
                click.performed += ctx => OnClick();
                click.Enable();
            }
            else
            {
                click.performed -= ctx => OnClick();
                click.Disable();
            }
        }

        public void OnClick()
        {
            if (!isActive) return;

            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 0.5f, button_layer))
            {
                var button = hit.collider.GetComponent<KeypadButton>();

                if (button != null)
                {
                    PressButton(button);
                }
            }
        }

        public abstract void PressButton(KeypadButton button);

        public abstract void CheckAnswer();
    }

    public abstract class KeypadButton: MonoBehaviour
    {
        [Header("Materials")]
        [SerializeField] protected Material[] materials;

        protected MeshRenderer mesh;
        protected Collider col;

        protected int id;

        [Header("Pressed Event")]
        [SerializeField] protected UnityEvent OnPressed;

        public void Init(int id)
        {
            this.id = id;

            if (mesh == null) mesh = GetComponent<MeshRenderer>();
            mesh.material = materials[0];

            SetActive(false);
        }

        public int GetId() { return id; }

        public void SetActive(bool active)
        {
            if (col == null) col = GetComponent<Collider>();

            col.enabled = active;
        }

        public abstract void Pressed();

        public abstract void Released();

    }
}

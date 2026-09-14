using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EscapeRoomKit
{
    public class Object_Keypad_2 : Object_FixCamera
    {
        [Header("Buttons")]
        [SerializeField] List<Object_Keypad_Button> buttons;
        [SerializeField] Object_Keypad_Button submit_button;
        [SerializeField] LayerMask button_layer;

        [Header("Answer")]
        [SerializeField] List<int> answer_list;

        HashSet<int> answer;

        HashSet<int> input;

        [Header("Unlock Event")]
        [SerializeField] UnityEvent UnlockEvent;

        bool isActive;

        InputAction click;

        Collider col;

        [Header("Sounds")]
        [SerializeField] List<AudioClip> clips;
        Play_Audio audio_player;

        void Start()
        {
            click = InputSystem.actions.FindAction("Click");
            click.Disable();

            isActive = false;

            InitButtons();

            col = GetComponent<Collider>();

            audio_player = GetComponent<Play_Audio>();

            answer = new HashSet<int>();
            for(int i=0;i<answer_list.Count;i++)
            {
                answer.Add(answer_list[i]);
            }

            input = new HashSet<int>();
        }

        void InitButtons()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Init(i);
            }
            submit_button.Init(-1);
        }

        public override void OnFixed()
        {
            SetActiveInput(true);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetActive(true);
            }
            submit_button.SetActive(true);

            input.Clear();
        }

        public override void OnUnFixed()
        {
            SetActiveInput(false);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetActive(false);
            }
            submit_button.SetActive(false);

            input.Clear();
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
                var button = hit.collider.GetComponent<Object_Keypad_Button>();

                if (button != null)
                {
                    PressButton(button);
                }
            }
        }

        void PressButton(Object_Keypad_Button button)
        {
            int n = button.GetId();

            if (n >= 0 && n < clips.Count) audio_player?.PlayAudio(clips[n]);

            if (n == -1)
            {
                CheckAnswer();

                button.Pressed();
            }
            else
            {
                if (!input.Contains(n))
                {
                    input.Add(n);

                    button.Pressed();
                }
            }
        }

        protected virtual void CheckAnswer()
        {
            if (input.SetEquals(answer)) UnlockEvent?.Invoke();
            else input.Clear();

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Released();
            }
            submit_button.Released();
        }
    }
}

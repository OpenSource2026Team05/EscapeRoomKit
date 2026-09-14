using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace EscapeRoomKit
{
    public class Object_Keypad : Object_FixCamera
    {
        [Header("Input Panel")]
        [SerializeField] bool usePanel;
        [SerializeField] TextMeshPro text_input;

        [Header("Input Setting")]
        [SerializeField] int max_input_size = 4;

        [Header("Buttons")]
        [SerializeField] List<Object_Keypad_Button> buttons;
        [SerializeField] Object_Keypad_Button submit_button;
        [SerializeField] LayerMask button_layer;

        [Header("Answer")]
        [SerializeField] List<int> answer;

        List<int> input;

        [Header("Unlock Event")]
        [SerializeField] UnityEvent UnlockEvent;

        [Header("Fail Event")]
        [SerializeField] UnityEvent FailEvent;

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

            input = new List<int>(max_input_size);

            if (usePanel) text_input.text = "";

            InitButtons();

            col = GetComponent<Collider>();

            audio_player = GetComponent<Play_Audio>();
        }

        void InitButtons()
        {
            for(int i=0;i<buttons.Count;i++)
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

            if (usePanel) text_input.text = "";
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

            if (usePanel) text_input.text = "";
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

            button.Pressed();

            if(n >=0 && n < clips.Count) audio_player?.PlayAudio(clips[n]);

            if (n == -1)
            {
                CheckAnswer();
            }
            else
            {
                if (input.Count >= max_input_size) return;

                input.Add(n);

                if (usePanel) text_input.text += n.ToString();
            }
        }

        protected virtual void CheckAnswer()
        {
            if(answer.SequenceEqual(input))
            {
                UnlockEvent?.Invoke();

                player.GetComponent<Player_FixCamera>().UnFixCamera();
            }
            else
            {
                FailEvent?.Invoke();

                input.Clear();

                if (usePanel) text_input.text = "";
            }
        }
    }
}

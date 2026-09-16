using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace EscapeRoomKit
{
    public class Object_Keypad : Keypad
    {
        [Header("Input Panel")]
        [SerializeField] bool usePanel;
        [SerializeField] TextMeshPro text_input;

        [Header("Input Setting")]
        [SerializeField] int max_input_size = 4;


        [Header("Answer")]
        [SerializeField] List<int> answer;

        List<int> input;

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

        public override void PressButton(KeypadButton button)
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

        public override void CheckAnswer()
        {
            if(answer.SequenceEqual(input))
            {
                UnlockEvent?.Invoke();

                player.UnFixCamera();
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

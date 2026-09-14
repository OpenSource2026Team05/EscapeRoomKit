using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EscapeRoomKit
{
    public class Object_Keypad_2 : Keypad
    {

        [Header("Answer")]
        [SerializeField] protected List<int> answer_list;

        HashSet<int> answer;

        HashSet<int> input;

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

        public override void PressButton(KeypadButton button)
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
                else
                {
                    input.Remove(n);

                    button.Released();
                }
            }
        }

        public override void CheckAnswer()
        {
            if (input.SetEquals(answer))
            {
                UnlockEvent?.Invoke();

                player.GetComponent<Player_FixCamera>().UnFixCamera();
            }
            else
            {
                FailEvent?.Invoke();

                input.Clear();
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Released();
            }
            submit_button.Released();
        }
    }
}

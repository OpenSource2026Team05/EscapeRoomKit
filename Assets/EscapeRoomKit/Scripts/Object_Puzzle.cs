using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace EscapeRoomKit
{
    public class Object_Puzzle : MonoBehaviour
    {
        [Header("Piece")]
        [SerializeField] List<Object_Puzzle_Piece> piece;

        List<bool> inputs;

        [Header("해제 이벤트")]
        public UnityEvent UnlockEvent;

        void Start()
        {
            Init();
        }

        void Init()
        {
            inputs = new List<bool>(piece.Count);

            for (int i=0;i<piece.Count;i++)
            {
                piece[i].Init(i, this);
                inputs.Add(false);
            }
        }

        public void UpdateInput(int id, bool value)
        {
            Debug.Log(id + " " + value);
            inputs[id] = value;
            CheckResult();
        }

        void CheckResult()
        {
            for(int i=0;i<inputs.Count;i++)
            {
                if (inputs[i] == false) return;
            }

            UnlockEvent?.Invoke();
        }
    }
}

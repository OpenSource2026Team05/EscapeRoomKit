using UnityEngine;

namespace EscapeRoomKit
{
    public class Object_Puzzle_Piece : Object_PutOn
    {
        [Header("Key")]
        [SerializeField] GameObject answer;

        int id;
        Object_Puzzle puzzle;

        public override void OnPutDown()
        {
            puzzle.UpdateInput(id, current == answer);
        }

        public override void OnPickUp()
        {
            puzzle.UpdateInput(id, false);
        }

        public void Init(int id, Object_Puzzle puzzle)
        {
            this.id = id;
            this.puzzle = puzzle;
        }
    }
}

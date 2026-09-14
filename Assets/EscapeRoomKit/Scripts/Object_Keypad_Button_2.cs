using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace EscapeRoomKit
{
    public class Object_Keypad_Button_2 : KeypadButton
    {
        public override void Pressed()
        {
            OnPressed?.Invoke();

            if (mesh == null) mesh = GetComponent<MeshRenderer>();
            mesh.material = materials[1];
        }

        public override void Released()
        {
            if (mesh == null) mesh = GetComponent<MeshRenderer>();
            mesh.material = materials[0];
        }
    }
}

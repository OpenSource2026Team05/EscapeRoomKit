using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomKit
{
    public class Object_Keypad_Button : KeypadButton
    {
        public override void Pressed()
        {
            OnPressed?.Invoke();

            StartCoroutine(ChangeMaterial());
        }

        public override void Released() { }

        IEnumerator ChangeMaterial()
        {
            if (mesh == null) mesh = GetComponent<MeshRenderer>();
            if (col == null) col = GetComponent<Collider>();

            mesh.material = materials[1];

            col.enabled = false;

            yield return new WaitForSeconds(0.5f);

            mesh.material = materials[0];

            col.enabled = true;
        }
    }
}

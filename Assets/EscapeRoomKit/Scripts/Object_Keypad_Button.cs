using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomKit
{
    public class Object_Keypad_Button : MonoBehaviour
    {
        [Header("Materials")]
        [SerializeField] protected Material[] materials;

        protected MeshRenderer mesh;
        Collider col;

        protected int id;

        [Header("Pressed Event")]
        [SerializeField] protected UnityEvent OnPressed;

        public virtual void Pressed()
        {
            OnPressed?.Invoke();

            StartCoroutine(ChangeMaterial());
        }

        public virtual void Released() { }

        public void Init(int id)
        {
            this.id = id;

            if (mesh == null) mesh = GetComponent<MeshRenderer>();
            mesh.material = materials[0];

            SetActive(false);
        }

        public int GetId() { return  id; }

        public void SetActive(bool active)
        {
            if (col == null) col = GetComponent<Collider>();

            col.enabled = active;
        }

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

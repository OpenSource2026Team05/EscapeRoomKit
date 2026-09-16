using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EscapeRoomKit
{
    public class Player_Grab : MonoBehaviour
    {
        [Header("Grab")]
        public Transform Hand;
        public float targetTime = 1f;

        [Header("Release")]
        public float throwForce;

        bool isGrabbing;
        private GameObject GrabbingObject;

        Vector3 originalPos;
        Quaternion originalRot;
        Vector3 releasePos;

        void Start()
        {
            isGrabbing = false;
            GrabbingObject = null;
        }

        public bool isGrab()
        {
            return isGrabbing;
        }

        public GameObject GetGrabbing()
        {
            return GrabbingObject;
        }

        public void UseItem(GameObject target)
        {
            if (GrabbingObject == null || target == null) return;

            GrabbingObject.GetComponent<Object_Item>().UseItem(target);
        }

        public void RemoveGrabbing()
        {
            if (GrabbingObject == null) return;

            isGrabbing = false;

            GrabbingObject.transform.parent = null;
            GrabbingObject = null;
        }

        public void Grab(GameObject grab)
        {
            if (isGrabbing) return;

            isGrabbing = true;
            GrabbingObject = grab;

            GrabbingObject.transform.parent = Hand;

            StartCoroutine(MoveToTargetPos(Hand, GrabbingObject));
        }

        public void Release()
        {
            if (!isGrabbing) return;

            releasePos = transform.position + new Vector3(0, 1.5f, 0) + 0.4f * transform.forward;

            GrabbingObject.transform.position = releasePos;
            GrabbingObject.transform.parent = null;
            GrabbingObject.transform.GetComponent<Collider>().enabled = true;
            GrabbingObject.transform.GetComponent<Rigidbody>().isKinematic = false;

            StartCoroutine(CollisionMode(GrabbingObject.gameObject));

            GrabbingObject.transform.GetComponent<Rigidbody>().AddForce(Hand.forward * throwForce, ForceMode.Impulse);

            isGrabbing = false;
            GrabbingObject = null;
        }

        public GameObject PutDown(Transform targetPos)
        {
            if (!isGrabbing) return null;

            GrabbingObject.transform.parent = null;
            isGrabbing = false;
            GameObject targetObj = GrabbingObject;
            GrabbingObject = null;

            StartCoroutine(MoveToTargetPos(targetPos, targetObj));

            return targetObj;
        }

        IEnumerator CollisionMode(GameObject grab)
        {
            grab.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            yield return new WaitForSeconds(3.0f);

            grab.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
        }

        IEnumerator MoveToTargetPos(Transform targetPos, GameObject targetObj)
        {
            GetComponent<IPlayer>().SetMoveLock(true);

            targetObj.transform.GetComponent<Collider>().enabled = false;
            targetObj.transform.GetComponent<Rigidbody>().isKinematic = true;

            originalPos = targetObj.transform.position;
            originalRot = targetObj.transform.rotation;

            Vector3 targetPosition = targetPos.position;
            Quaternion targetRotation = targetPos.rotation;

            float t = 0f;

            while (t < targetTime)
            {
                t += Time.deltaTime;
                float tp = t / targetTime;

                targetObj.transform.position = Vector3.Lerp(originalPos, targetPosition, tp);

                targetObj.transform.rotation = Quaternion.Lerp(originalRot, targetRotation, tp);

                yield return null;
            }

            GetComponent<IPlayer>().SetMoveLock(false);
        }
    }
}

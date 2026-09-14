using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;


namespace Virtuademy.SDK.Tasks.Detectors
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Simple component that fires events when a rigidbody enters or exits a
    /// trigger
    /// </summary>
    public class TriggerDetector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("")]
        private Rigidbody[] enteringRigidBodies = default;

        [SerializeField, Tooltip("")]
        private Collider trigger = default;

        [Header("Settings")]
        [SerializeField, Tooltip("")]
        private float holdTime = 0.0f;

        [Header("Events")]
        [SerializeField, Tooltip("")]
        private UnityEvent<Collider> onTriggerEnter = default;

        [SerializeField, Tooltip("")]
        private UnityEvent onTriggerExit = default;


        private TriggerProxy triggerProxy;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            triggerProxy = trigger.gameObject.AddComponent<TriggerProxy>();
            triggerProxy.detector = this;
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            if (triggerProxy != null)
                Destroy(triggerProxy);
        }

        ///////////////////////////////////////////////////////////////////////////
        private class TriggerProxy : MonoBehaviour
        {
            public TriggerDetector detector;
            private Coroutine holdingCoroutine;

            ///////////////////////////////////////////////////////////////////////////
            private void OnTriggerEnter(Collider other)
            {
                if (detector.enteringRigidBodies.Any(rb => rb == other.attachedRigidbody))
                {
                    if (holdingCoroutine != null)
                        StopCoroutine(holdingCoroutine);

                    IEnumerator coroutine()
                    {
                        yield return new WaitForSeconds(detector.holdTime);
                        detector.onTriggerEnter.Invoke(other);
                    }
                    holdingCoroutine = StartCoroutine(coroutine());
                }
            }

            ///////////////////////////////////////////////////////////////////////////
            private void OnTriggerExit(Collider other)
            {
                if (detector.enteringRigidBodies.Any(rb => rb == other.attachedRigidbody))
                    detector.onTriggerExit.Invoke();
                if (holdingCoroutine != null)
                    StopCoroutine(holdingCoroutine);
            }
        }
    }
}
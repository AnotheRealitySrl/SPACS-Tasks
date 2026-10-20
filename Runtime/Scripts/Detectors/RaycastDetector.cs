using System.Collections;

using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.Detectors
{
    ///////////////////////////////////////////////////////////////////////////
    public class RaycastDetector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("")]
        private Collider[] castTargets = default;

        [SerializeField, Tooltip("")]
        private Transform castPivot;

        [SerializeField, Tooltip("if set to true and castPivot set to none then use the main camera as cast pivot")]
        private bool useCamera;

        [SerializeField, Tooltip("")]
        private Transform laser;

        [Header("Settings")]
        [SerializeField, Tooltip("")]
        private float holdTime = 0.0f;

        [Header("Events")]
        [SerializeField, Tooltip("")]
        private UnityEvent onRaycastHit = default;

        [SerializeField, Tooltip("")]
        private UnityEvent onRaycastHitAfterDelay = default;

        [SerializeField, Tooltip("")]
        private UnityEvent onRaycastExit = default;

        private bool routineStarted = false;

        private void OnDisable()
        {
            if (laser)
                laser.gameObject.SetActive(false);
            routineStarted = false;
            StopAllCoroutines();
        }

        private const float castDistance = 2f;

        // Reused every frame, so the cast allocates nothing.
        private readonly RaycastHit[] hitBuffer = new RaycastHit[16];

        void Update()
        {
            if (castPivot == null)
            {
                if (!useCamera || Camera.main == null)
                    return;
                castPivot = Camera.main.transform;
            }

            int hitCount = Physics.RaycastNonAlloc(castPivot.position, castPivot.forward, hitBuffer, castDistance);

            bool targetHit = false;
            for (int i = 0; i < hitCount; i++)
            {
                if (!IsCastTarget(hitBuffer[i].collider))
                    continue;

                targetHit = true;
                if (laser)
                    laser.transform.position = hitBuffer[i].point;

                if (!routineStarted)
                {
                    onRaycastHit?.Invoke();
                    if (laser)
                        laser.gameObject.SetActive(true);
                    routineStarted = true;
                    StartCoroutine(CountDown());
                }
                break;
            }

            // Exit as soon as the ray stops hitting a target, also when it hits something else.
            if (!targetHit && routineStarted)
            {
                onRaycastExit?.Invoke();
                if (laser)
                    laser.gameObject.SetActive(false);
                routineStarted = false;
                StopAllCoroutines();
            }
        }

        private void Start()
        {
            if (castPivot == null && useCamera && Camera.main != null)
            {
                castPivot = Camera.main.transform;
            }
        }

        private bool IsCastTarget(Collider hitCollider)
        {
            if (castTargets == null)
                return false;
            for (int i = 0; i < castTargets.Length; i++)
            {
                if (castTargets[i] == hitCollider)
                    return true;
            }
            return false;
        }

        private IEnumerator CountDown()
        {
            yield return new WaitForSeconds(holdTime);
            onRaycastHitAfterDelay?.Invoke();
        }
    }
}

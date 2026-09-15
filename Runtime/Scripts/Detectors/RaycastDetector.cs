using System.Collections;
using System.Linq;

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

        void Update()
        {
            RaycastHit[] hits = Physics.RaycastAll(castPivot.position, castPivot.forward, 2);

            Debug.DrawRay(castPivot.position, castPivot.forward, Color.magenta);

            foreach (var hit in hits)
            {
                if (castTargets.Any(collider => collider == hit.collider))
                {
                    if (laser)
                        laser.transform.position = hit.point;

                    if (!routineStarted)
                    {
                        onRaycastHit?.Invoke();
                        if (laser)
                            laser.gameObject.SetActive(true);
                        routineStarted = true;
                        StartCoroutine(CountDown());
                    }
                }
            }
            if (hits.Length == 0 && routineStarted)
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
            if (castPivot == null && useCamera)
            {
                castPivot = Camera.main.transform;
            }
        }

        private IEnumerator CountDown()
        {
            yield return new WaitForSeconds(holdTime);
            onRaycastHitAfterDelay?.Invoke();
        }
    }
}

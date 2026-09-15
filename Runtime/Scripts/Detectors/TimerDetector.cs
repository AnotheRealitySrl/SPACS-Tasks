using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SPACS.Tasks.Detectors
{
    public class TimerDetector : MonoBehaviour
    {

        [SerializeField] private float timeToWait = 1f;
        [SerializeField] private UnityEvent onFinishedWaiting = default;

        // Start is called before the first frame update
        private void OnEnable()
        {
            StartCoroutine(WaitForTime());

        }

        private IEnumerator WaitForTime()
        {
            yield return new WaitForSeconds(timeToWait);
            onFinishedWaiting.Invoke();
        }
    }
}

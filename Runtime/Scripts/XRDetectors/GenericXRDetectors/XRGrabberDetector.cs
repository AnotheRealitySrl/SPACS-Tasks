using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.XRDetectors
{
    public class XRGrabberDetector : XRDetector
    {
        [Header("References")]
        public GameObject[] controllerInteractors = default;

        public GameObject[] grabbables = default;

        [Header("Events")]
        public UnityEvent OnGrabStart = default;

        public UnityEvent OnGrabEnd = default;

        public UnityEvent OnHoverStart = default;

        public UnityEvent OnHoverEnd = default;

        protected bool isGrabbing = false;

        protected void Init()
        {
            XRGrabberDetector genericDetector = null;
            foreach (var detector in GetComponents<XRGrabberDetector>())
            {
                if (detector != this)
                {
                    genericDetector = detector;
                    break;
                }
            }

            int lenght = genericDetector.controllerInteractors.Length;
            controllerInteractors = new GameObject[lenght];
            for (int i = 0; i < lenght; i++)
                controllerInteractors[i] = genericDetector.controllerInteractors[i].gameObject;

            lenght = genericDetector.grabbables.Length;
            grabbables = new GameObject[lenght];
            for (int i = 0; i < lenght; i++)
                grabbables[i] = genericDetector.grabbables[i].gameObject;

            controllerInteractors = genericDetector.controllerInteractors;
            grabbables = genericDetector.grabbables;
            OnGrabStart = genericDetector.OnGrabStart;
            OnGrabEnd = genericDetector.OnGrabEnd;
            OnHoverStart = genericDetector.OnHoverStart;
            OnHoverEnd = genericDetector.OnHoverEnd;
            initialized = true;
            Destroy(genericDetector);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.XRDetectors
{
    public class XRActivationDetector : XRDetector
    {
        [Header("References")]
        public GameObject activator = default;
        public GameObject[] activatables = default;

        [Header("Events")]
        public UnityEvent OnActivationStart = default;
        public UnityEvent OnActivationEnd = default;
        public UnityEvent OnHoverStart = default;
        public UnityEvent OnHoverEnd = default;

        protected void Init()
        {
            XRActivationDetector genericDetector = null;
            foreach (var detector in GetComponents<XRActivationDetector>())
            {
                if (detector != this)
                {
                    genericDetector = detector;
                    break;
                }
            }

            activator = genericDetector.activator;

            int lenght = genericDetector.activatables.Length;
            activatables = new GameObject[lenght];
            for (int i = 0; i < lenght; i++)
                activatables[i] = genericDetector.activatables[i];

            OnActivationStart = genericDetector.OnActivationStart;
            OnActivationEnd = genericDetector.OnActivationEnd;
            OnHoverStart = genericDetector.OnHoverStart;
            OnHoverEnd = genericDetector.OnHoverEnd;
            initialized = true;
            Destroy(genericDetector);
        }
    }
}

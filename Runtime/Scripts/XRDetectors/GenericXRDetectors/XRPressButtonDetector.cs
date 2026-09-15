using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.XRDetectors
{
    public class XRPressButtonDetector : XRDetector
    {
        public GameObject button;

        public UnityEvent OnPressed;
        public UnityEvent OnUnpressed;

        protected void Init()
        {
            XRPressButtonDetector genericDetector = null;
            foreach (var detector in GetComponents<XRPressButtonDetector>())
            {
                if (detector != this)
                {
                    genericDetector = detector;
                    break;
                }
            }
            button = genericDetector.button;
            OnPressed = genericDetector.OnPressed;
            OnUnpressed = genericDetector.OnUnpressed;
            initialized = true;
            Destroy(genericDetector);
        }
    }
}

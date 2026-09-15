using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.XRDetectors
{
    public class XRSliderDetector : XRDetector
    {
        public GameObject slider;

        public UnityEvent OnMin;
        public UnityEvent OnMid;
        public UnityEvent OnMax;

        protected void Init()
        {
            XRSliderDetector genericDetector = null;
            foreach (var detector in GetComponents<XRSliderDetector>())
            {
                if (detector != this)
                {
                    genericDetector = detector;
                    break;
                }
            }
            slider = genericDetector.slider;
            OnMin = genericDetector.OnMin;
            OnMid = genericDetector.OnMid;
            OnMax = genericDetector.OnMax;
            initialized = true;
            Destroy(genericDetector);
        }
    }
}

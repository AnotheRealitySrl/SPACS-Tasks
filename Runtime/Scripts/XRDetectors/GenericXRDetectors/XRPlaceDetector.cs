using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.XRDetectors
{
    public class XRPlaceDetector : XRDetector
    {
        [Header("References")]
        public GameObject placePoint = default;

        public GameObject grabbable = default;

        [Header("Events")]
        public UnityEvent OnPlace = default;

        public UnityEvent OnRemove = default;

        protected void Init()
        {
            XRPlaceDetector genericDetector = null;
            foreach (var detector in GetComponents<XRPlaceDetector>())
            {
                if (detector != this)
                {
                    genericDetector = detector;
                    break;
                }
            }
            placePoint = genericDetector.placePoint;
            grabbable = genericDetector.grabbable;
            OnPlace = genericDetector.OnPlace;
            OnRemove = genericDetector.OnRemove;
            initialized = true;
            Destroy(genericDetector);
        }
    }
}

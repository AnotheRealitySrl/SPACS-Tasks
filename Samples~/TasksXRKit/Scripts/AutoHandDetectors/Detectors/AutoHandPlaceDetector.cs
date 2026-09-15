using Autohand;

using SPACS.Tasks.XRDetectors;

namespace SPACS.TasksXRKit.AutoHandDetectors
{
    public class AutoHandPlaceDetector : XRPlaceDetector
    {
        private PlacePoint autoHandPlacePoint = default;
        private Grabbable autoHandGrabbable = default;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            if (!initialized)
            {
                Init();
                autoHandPlacePoint = placePoint.GetComponent<PlacePoint>();
                autoHandGrabbable = grabbable.GetComponent<Grabbable>();
            }

            autoHandPlacePoint.OnPlaceEvent += Place;
            autoHandPlacePoint.OnRemoveEvent += Remove;
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            autoHandPlacePoint.OnPlaceEvent -= Place;
            autoHandPlacePoint.OnRemoveEvent -= Remove;
        }

        ///////////////////////////////////////////////////////////////////////////
        private void Place(PlacePoint point, Grabbable obj)
        {
            if (obj == autoHandGrabbable)
                base.OnPlace?.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void Remove(PlacePoint point, Grabbable obj)
        {
            if (obj == autoHandGrabbable)
                base.OnRemove?.Invoke();
        }
    }
}

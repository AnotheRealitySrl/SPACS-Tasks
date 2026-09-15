using Autohand;

using SPACS.Tasks.XRDetectors;

using System.Linq;

namespace SPACS.TasksXRKit.AutoHandDetectors
{
    public class AutoHandGrabberDetector : XRGrabberDetector
    {
        private Hand[] autoHandControllerInteractors = default;
        private Grabbable[] autoHandGrabbables = default;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            if (!initialized)
            {
                Init();

                int lenght = controllerInteractors.Length;
                autoHandControllerInteractors = new Hand[lenght];
                for (int i = 0; i < lenght; i++)
                    autoHandControllerInteractors[i] = controllerInteractors[i].gameObject.GetComponent<Hand>();

                lenght = grabbables.Length;
                autoHandGrabbables = new Grabbable[lenght];
                for (int i = 0; i < lenght; i++)
                    autoHandGrabbables[i] = grabbables[i].gameObject.GetComponent<Grabbable>();
            }

            foreach (var controller in autoHandControllerInteractors)
            {
                controller.OnBeforeGrabbed += GrabStart;
                controller.OnBeforeReleased += GrabEnd;
                controller.OnHighlight += HoverStart;
                controller.OnStopHighlight += HoverEnd;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            foreach (var controller in autoHandControllerInteractors)
            {
                controller.OnBeforeGrabbed -= GrabStart;
                controller.OnBeforeReleased -= GrabEnd;
                controller.OnHighlight -= HoverStart;
                controller.OnStopHighlight -= HoverEnd;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void GrabStart(Hand hand, Grabbable obj)
        {

            if (grabbables.Length > 0 && !autoHandGrabbables.Contains(obj))
                return;

            isGrabbing = true;
            OnGrabStart.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void GrabEnd(Hand hand, Grabbable obj)
        {
            if (grabbables.Length > 0 && !autoHandGrabbables.Contains(obj))
                return;

            isGrabbing = false;
            OnGrabEnd.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void HoverStart(Hand hand, Grabbable obj)
        {
            if (grabbables.Length > 0 && !autoHandGrabbables.Contains(obj))
                return;

            if (isGrabbing)
                return;

            OnHoverStart.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void HoverEnd(Hand hand, Grabbable obj)
        {
            if (grabbables.Length > 0 && !autoHandGrabbables.Contains(obj))
                return;

            if (isGrabbing)
                return;

            OnHoverEnd.Invoke();
        }
    }
}

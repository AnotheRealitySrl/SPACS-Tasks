using SPACS.Tasks.XRDetectors;

using System.Linq;

using UnityEngine.XR.Interaction.Toolkit;

namespace SPACS.TasksXRKit.XRKitDetectors
{
    ///////////////////////////////////////////////////////////////////////////
    ///<summary>
    ///Component that detect an interaction with a specific innteractor, like one of the two hands
    ///</summary>
    public class XRKitGrabberDetector : XRGrabberDetector
    {
        private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor[] xrKitControllerInteractors = default;
        private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable[] xrKitGrabbables = default;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            if (!initialized)
            {
                Init();

                int lenght = controllerInteractors.Length;
                xrKitControllerInteractors = new UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor[lenght];
                for (int i = 0; i < lenght; i++)
                    xrKitControllerInteractors[i] = controllerInteractors[i].gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor>();

                lenght = grabbables.Length;
                xrKitGrabbables = new UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable[lenght];
                for (int i = 0; i < lenght; i++)
                    xrKitGrabbables[i] = grabbables[i].gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            }

            foreach (var controller in xrKitControllerInteractors)
            {
                controller.selectEntered.AddListener(GrabStart);
                controller.selectExited.AddListener(GrabEnd);
                controller.hoverEntered.AddListener(HoverStart);
                controller.hoverExited.AddListener(HoverEnd);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            foreach (var controller in xrKitControllerInteractors)
            {
                controller.selectEntered.RemoveListener(GrabStart);
                controller.selectExited.RemoveListener(GrabEnd);
                controller.hoverEntered.RemoveListener(HoverStart);
                controller.hoverExited.RemoveListener(HoverEnd);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void GrabStart(BaseInteractionEventArgs arg0)
        {

            if (grabbables.Length > 0 && !xrKitGrabbables.Contains(arg0.interactableObject))
                return;

            isGrabbing = true;
            OnGrabStart.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void GrabEnd(BaseInteractionEventArgs arg0)
        {
            if (grabbables.Length > 0 && !xrKitGrabbables.Contains(arg0.interactableObject))
                return;

            isGrabbing = false;
            OnGrabEnd.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void HoverStart(BaseInteractionEventArgs arg0)
        {

            if (grabbables.Length > 0 && !xrKitGrabbables.Contains(arg0.interactableObject))
                return;

            if (isGrabbing)
                return;

            OnHoverStart.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void HoverEnd(BaseInteractionEventArgs arg0)
        {
            if (grabbables.Length > 0 && !xrKitGrabbables.Contains(arg0.interactableObject))
                return;

            if (isGrabbing)
                return;

            OnHoverEnd.Invoke();
        }
    }
}
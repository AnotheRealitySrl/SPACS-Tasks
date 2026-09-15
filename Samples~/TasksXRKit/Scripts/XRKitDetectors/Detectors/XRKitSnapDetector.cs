using SPACS.Tasks.XRDetectors;
using Virtuademy.SDK.XRKit;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SPACS.TasksXRKit.XRKitDetectors
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Utility component that fires events when an object is snapped on a snap point
    /// </summary>
    public class XRKitSnapDetector : XRPlaceDetector
    {
        private XRSnapInteractor xrKitSnapPoint = default;
        private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable xrKitGrabbable = default;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            if (!initialized)
            {
                Init();
                xrKitSnapPoint = placePoint.GetComponent<XRSnapInteractor>();
                xrKitGrabbable = grabbable.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            }

            xrKitSnapPoint.selectEntered.AddListener(OnSnap);
            xrKitSnapPoint.selectExited.AddListener(OnUnsnap);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            xrKitSnapPoint.selectEntered.RemoveListener(OnSnap);
            xrKitSnapPoint.selectExited.RemoveListener(OnUnsnap);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnSnap(SelectEnterEventArgs arg)
        {
            if ((Object)arg.interactableObject == xrKitGrabbable)
                OnPlace.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnUnsnap(SelectExitEventArgs arg)
        {
            if ((Object)arg.interactableObject == xrKitGrabbable)
                OnRemove.Invoke();
        }
    }
}
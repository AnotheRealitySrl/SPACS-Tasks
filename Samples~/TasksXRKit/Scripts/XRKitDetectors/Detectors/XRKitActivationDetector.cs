using SPACS.Tasks.XRDetectors;

using System.Linq;

using Unity.XR.CoreUtils;

using UnityEngine.XR.Interaction.Toolkit;

namespace SPACS.TasksXRKit.XRKitDetectors
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Simple component that fires events when a rigidbody enters or exits a
    /// trigger
    /// </summary>
    public class XRKitActivationDetector : XRActivationDetector
    {
        private XROrigin xrKitActivator = default;
        private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable[] xrKitActivatables = default;
        private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor[] controllerInteractors = default;
        private bool isSelected = false;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            if (!initialized)
            {
                Init();

                xrKitActivator = activator.GetComponent<XROrigin>();

                int lenght = activatables.Length;
                xrKitActivatables = new UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable[lenght];
                for (int i = 0; i < lenght; i++)
                    xrKitActivatables[i] = activatables[i].GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

                controllerInteractors = xrKitActivator.GetComponentsInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor>(true);
            }

            foreach (var controller in controllerInteractors)
            {
                controller.selectEntered.AddListener(SelectStart);
                controller.selectExited.AddListener(SelectEnd);
                controller.hoverEntered.AddListener(HoverStart);
                controller.hoverExited.AddListener(HoverEnd);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            foreach (var controller in controllerInteractors)
            {
                controller.selectEntered.RemoveListener(SelectStart);
                controller.selectExited.RemoveListener(SelectEnd);
                controller.hoverEntered.RemoveListener(HoverStart);
                controller.hoverExited.RemoveListener(HoverEnd);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void SelectStart(BaseInteractionEventArgs arg0)
        {
            if (xrKitActivatables.Length > 0 && !xrKitActivatables.Contains(arg0.interactableObject))
                return;

            isSelected = true;
            OnActivationStart.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void SelectEnd(BaseInteractionEventArgs arg0)
        {
            if (xrKitActivatables.Length > 0 && !xrKitActivatables.Contains(arg0.interactableObject))
                return;

            isSelected = false;
            OnActivationEnd.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void HoverStart(BaseInteractionEventArgs arg0)
        {

            if (xrKitActivatables.Length > 0 && !xrKitActivatables.Contains(arg0.interactableObject))
                return;

            if (isSelected)
                return;

            OnHoverStart.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void HoverEnd(BaseInteractionEventArgs arg0)
        {
            if (xrKitActivatables.Length > 0 && !xrKitActivatables.Contains(arg0.interactableObject))
                return;

            if (isSelected)
                return;

            OnHoverEnd.Invoke();
        }
    }
}
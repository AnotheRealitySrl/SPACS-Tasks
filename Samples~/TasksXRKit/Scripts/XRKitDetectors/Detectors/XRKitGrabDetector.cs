using SPACS.Tasks.XRDetectors;

using UnityEngine.XR.Interaction.Toolkit;

namespace SPACS.TasksXRKit.XRKitDetectors
{
    ///////////////////////////////////////////////////////////////////////////
    public class XRKitGrabDetector : XRGrabDetector
    {
        private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable xrKitgrabbable = default;


        private bool isGrabbed = false;
        private bool isHovered = false;

        ///////////////////////////////////////////////////////////////////////////
        public bool IsGrabbed
        {
            get => isGrabbed;
            private set
            {
                if (value != isGrabbed)
                {
                    isGrabbed = value;
                    (isGrabbed ? OnGrabStart : OnGrabEnd).Invoke();
                }
            }
        }

        public bool IsHovered
        {
            get => isHovered;
            private set
            {
                if (value != isHovered && !isGrabbed)
                {
                    isHovered = value;
                    (isHovered ? OnHoverStart : OnHoverEnd).Invoke();
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            if (!initialized)
            {
                Init();
                xrKitgrabbable = grabbable.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            }

            xrKitgrabbable.selectEntered.AddListener(OnGrabChanged);
            xrKitgrabbable.selectExited.AddListener(OnGrabChanged);
            xrKitgrabbable.hoverEntered.AddListener(OnHoverChanged);
            xrKitgrabbable.hoverExited.AddListener(OnHoverChanged);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            xrKitgrabbable.selectEntered.RemoveListener(OnGrabChanged);
            xrKitgrabbable.selectExited.RemoveListener(OnGrabChanged);
            xrKitgrabbable.hoverEntered.RemoveListener(OnHoverChanged);
            xrKitgrabbable.hoverExited.RemoveListener(OnHoverChanged);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnGrabChanged(BaseInteractionEventArgs arg0)
        {
            var grabbed = arg0 is SelectEnterEventArgs;
            IsGrabbed = grabbed;
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnHoverChanged(BaseInteractionEventArgs arg0)
        {
            var hovered = arg0 is HoverEnterEventArgs;
            IsHovered = hovered;
        }
    }
}
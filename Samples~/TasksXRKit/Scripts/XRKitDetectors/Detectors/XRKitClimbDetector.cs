using SPACS.Tasks.XRDetectors;
using Virtuademy.SDK.XRKit;

using System.Linq;

using UnityEngine.XR.Interaction.Toolkit;
using ClimbInteractable = Virtuademy.SDK.XRKit.ClimbInteractable;

namespace SPACS.TasksXRKit.XRKitDetectors
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Utility component that fires events when climbing points contained in
    /// a climbing group are grabbed.
    /// </summary>
    public class XRKitClimbDetector : XRClimbDetector
    {
        private ClimbGroupManager xrKitClimbingPointsGroup = default;
        private ClimbInteractable[] xrKitHighestPoints = default;
        private ClimbInteractable[] climbingPoints = default;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            if (!initialized)
            {
                Init();

                xrKitClimbingPointsGroup = climbingPointsGroup.GetComponent<ClimbGroupManager>();

                int lenght = highestPoints.Length;
                xrKitHighestPoints = new ClimbInteractable[lenght];
                for (int i = 0; i < lenght; i++)
                    xrKitHighestPoints[i] = highestPoints[i].GetComponent<ClimbInteractable>();

                if (highestHaveOnClimb)
                {
                    climbingPoints = xrKitClimbingPointsGroup.GetComponentsInChildren<ClimbInteractable>();
                }
                else
                {
                    var tempPoints = xrKitClimbingPointsGroup.GetComponentsInChildren<ClimbInteractable>();
                    climbingPoints = tempPoints.Except(xrKitHighestPoints).ToArray();
                }
            }

            foreach (var point in climbingPoints)
                point.selectEntered.AddListener(OnClimbableGrabStart);

            foreach (var point in xrKitHighestPoints)
                point.selectEntered.AddListener(OnHighestClimbableGrabStart);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            foreach (var point in climbingPoints)
                point.selectEntered.RemoveListener(OnClimbableGrabStart);

            foreach (var point in xrKitHighestPoints)
                point.selectEntered.RemoveListener(OnHighestClimbableGrabStart);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnClimbableGrabStart(SelectEnterEventArgs arg0)
        {
            OnClimb.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnHighestClimbableGrabStart(SelectEnterEventArgs arg0)
        {
            OnHighestPointClimb.Invoke();
        }
    }
}
using Virtuademy.SDK.Tasks.XRDetectors;

using UnityEngine;


namespace Virtuademy.SDK.TasksXRKit.XRKitDetectors
{
    [CreateAssetMenu(menuName = "Virtuademy/SDK-TasksXRKit/XRKitDetectorsConfig", fileName = "XRKitDetectorsConfig")]
    public class XRKitDetectorsSystemConfig : DetectorsSystemConfig
    {
        public override void AddActivation(GameObject detector)
        {
            detector.AddComponent<XRKitActivationDetector>();
        }

        public override void AddClimb(GameObject detector)
        {
            detector.AddComponent<XRKitClimbDetector>();
        }

        public override void AddGrab(GameObject detector)
        {
            detector.AddComponent<XRKitGrabDetector>();
        }

        public override void AddGrabber(GameObject detector)
        {
            detector.AddComponent<XRKitGrabberDetector>();
        }

        public override void AddLever(GameObject detector)
        {
            throw new System.NotImplementedException();
        }

        public override void AddPlace(GameObject detector)
        {
            detector.AddComponent<XRKitSnapDetector>();
        }

        public override void AddPressButton(GameObject detector)
        {
            throw new System.NotImplementedException();
        }

        public override void AddRotation(GameObject detector)
        {
            detector.AddComponent<XRKitRotationDetector>();
        }

        public override void AddSlider(GameObject detector)
        {
            throw new System.NotImplementedException();
        }

        public override void AddTeleport(GameObject detector)
        {
            detector.AddComponent<XRKitTeleportDetector>();
        }
    }
}

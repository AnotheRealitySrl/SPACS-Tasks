using SPACS.Tasks.XRDetectors;

using UnityEngine;


namespace SPACS.TasksXRKit.AutoHandDetectors
{
    [CreateAssetMenu(menuName = "Virtuademy/SDK-TasksXRKit/AutoHandDetectorsConfig", fileName = "AutoHandDetectorsConfig")]
    public class AutoHandDetectorsSystemConfig : DetectorsSystemConfig
    {
        public override void AddActivation(GameObject detector)
        {
            throw new System.NotImplementedException();
        }

        public override void AddClimb(GameObject detector)
        {
            throw new System.NotImplementedException();
        }

        public override void AddGrab(GameObject detector)
        {
            detector.AddComponent<AutoHandGrabDetector>();
        }

        public override void AddGrabber(GameObject detector)
        {
            detector.AddComponent<AutoHandGrabberDetector>();
        }

        public override void AddLever(GameObject detector)
        {
            detector.AddComponent<AutoHandLeverDetector>();
        }

        public override void AddPlace(GameObject detector)
        {
            detector.AddComponent<AutoHandPlaceDetector>();
        }

        public override void AddPressButton(GameObject detector)
        {
            detector.AddComponent<AutoHandPressButtonDetector>();
        }

        public override void AddRotation(GameObject detector)
        {
            detector.AddComponent<AutoHandRotationDetector>();
        }

        public override void AddSlider(GameObject detector)
        {
            detector.AddComponent<AutoHandSliderDetector>();
        }

        public override void AddTeleport(GameObject detector)
        {
            throw new System.NotImplementedException();
        }
    }
}


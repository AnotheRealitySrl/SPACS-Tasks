using UnityEngine;

namespace SPACS.Tasks.XRDetectors
{
    public class DetectorsSystem : MonoBehaviour
    {
        public static DetectorsSystem Instance { get; set; }

        public DetectorsSystemConfig activationDetectorConfig;
        public DetectorsSystemConfig climbDetectorConfig;
        public DetectorsSystemConfig grabberDetectorConfig;
        public DetectorsSystemConfig grabDetectorConfig;
        public DetectorsSystemConfig leverDetectorConfig;
        public DetectorsSystemConfig placeDetectorConfig;
        public DetectorsSystemConfig pressButtonDetectorConfig;
        public DetectorsSystemConfig rotationDetectorConfig;
        public DetectorsSystemConfig sliderDetectorConfig;
        public DetectorsSystemConfig teleportDetectorConfig;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            foreach (var activation in FindObjectsOfType<XRActivationDetector>())
            {
                activationDetectorConfig.AddActivation(activation.gameObject);
            }

            foreach (var climb in FindObjectsOfType<XRClimbDetector>())
            {
                climbDetectorConfig.AddClimb(climb.gameObject);
            }

            foreach (var grabber in FindObjectsOfType<XRGrabberDetector>())
            {
                grabberDetectorConfig.AddGrabber(grabber.gameObject);
            }

            foreach (var grab in FindObjectsOfType<XRGrabDetector>())
            {
                grabDetectorConfig.AddGrab(grab.gameObject);
            }

            foreach (var lever in FindObjectsOfType<XRLeverDetector>())
            {
                leverDetectorConfig.AddLever(lever.gameObject);
            }

            foreach (var place in FindObjectsOfType<XRPlaceDetector>())
            {
                placeDetectorConfig.AddPlace(place.gameObject);
            }

            foreach (var pressButton in FindObjectsOfType<XRPressButtonDetector>())
            {
                pressButtonDetectorConfig.AddPressButton(pressButton.gameObject);
            }

            foreach (var rotation in FindObjectsOfType<XRRotationDetector>())
            {
                rotationDetectorConfig.AddRotation(rotation.gameObject);
            }

            foreach (var slider in FindObjectsOfType<XRSliderDetector>())
            {
                sliderDetectorConfig.AddSlider(slider.gameObject);
            }

            foreach (var teleport in FindObjectsOfType<XRTeleportDetector>())
            {
                teleportDetectorConfig.AddTeleport(teleport.gameObject);
            }
        }
    }
}

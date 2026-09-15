using Autohand;

using SPACS.Tasks.XRDetectors;

namespace SPACS.TasksXRKit.AutoHandDetectors
{
    public class AutoHandSliderDetector : XRSliderDetector
    {
        private PhysicsGadgetSlider autoHandSlider;

        private void OnEnable()
        {
            if (!initialized)
            {
                Init();
                autoHandSlider = slider.GetComponent<PhysicsGadgetSlider>();
            }

            autoHandSlider.OnMin.AddListener(OnMin.Invoke);
            autoHandSlider.OnMid.AddListener(OnMid.Invoke);
            autoHandSlider.OnMax.AddListener(OnMax.Invoke);
        }

        private void OnDisable()
        {
            autoHandSlider.OnMin.RemoveListener(OnMin.Invoke);
            autoHandSlider.OnMid.RemoveListener(OnMid.Invoke);
            autoHandSlider.OnMax.RemoveListener(OnMax.Invoke);
        }
    }
}

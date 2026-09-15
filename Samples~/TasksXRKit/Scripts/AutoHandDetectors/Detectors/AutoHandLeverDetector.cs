using Autohand;

using SPACS.Tasks.XRDetectors;

namespace SPACS.TasksXRKit.AutoHandDetectors
{
    public class AutoHandLeverDetector : XRLeverDetector
    {
        private PhysicsGadgetLever autoHandLever;

        private void OnEnable()
        {
            if (!initialized)
            {
                Init();
                autoHandLever = lever.GetComponent<PhysicsGadgetLever>();
            }

            autoHandLever.OnMin.AddListener(OnMin.Invoke);
            autoHandLever.OnMid.AddListener(OnMid.Invoke);
            autoHandLever.OnMax.AddListener(OnMax.Invoke);
        }

        private void OnDisable()
        {
            autoHandLever.OnMin.RemoveListener(OnMin.Invoke);
            autoHandLever.OnMid.RemoveListener(OnMid.Invoke);
            autoHandLever.OnMax.RemoveListener(OnMax.Invoke);
        }
    }
}

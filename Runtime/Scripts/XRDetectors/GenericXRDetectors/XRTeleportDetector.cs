using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.XRDetectors
{
    public class XRTeleportDetector : XRDetector
    {
        [Tooltip("The teleporters to check")]
        public GameObject[] teleporters = default;

        [Tooltip("The event invoked when one of the monitored teleporters is activated")]
        public UnityEvent OnTeleport = default;

        protected void Init()
        {
            XRTeleportDetector genericDetector = null;
            foreach (var detector in GetComponents<XRTeleportDetector>())
            {
                if (detector != this)
                {
                    genericDetector = detector;
                    break;
                }
            }

            int lenght = genericDetector.teleporters.Length;
            teleporters = new GameObject[lenght];
            for (int i = 0; i < lenght; i++)
                teleporters[i] = genericDetector.teleporters[i].gameObject;

            OnTeleport = genericDetector.OnTeleport;
            initialized = true;
            Destroy(genericDetector);
        }
    }
}

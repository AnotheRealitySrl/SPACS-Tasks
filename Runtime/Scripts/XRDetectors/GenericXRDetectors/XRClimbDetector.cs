using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.XRDetectors
{
    public class XRClimbDetector : XRDetector
    {
        [Header("References")]
        public GameObject climbingPointsGroup = default;
        public GameObject[] highestPoints = default;

        [Header("Booleans")]
        [Tooltip("If true highestPoints have onClimb event too")]
        public bool highestHaveOnClimb = true;

        [Header("Events")]
        public UnityEvent OnClimb = default;
        public UnityEvent OnHighestPointClimb = default;

        protected void Init()
        {
            XRClimbDetector genericDetector = null;
            foreach (var detector in GetComponents<XRClimbDetector>())
            {
                if (detector != this)
                {
                    genericDetector = detector;
                    break;
                }
            }

            climbingPointsGroup = genericDetector.climbingPointsGroup;

            int lenght = genericDetector.highestPoints.Length;
            highestPoints = new GameObject[lenght];
            for (int i = 0; i < lenght; i++)
                highestPoints[i] = genericDetector.highestPoints[i].gameObject;

            OnClimb = genericDetector.OnClimb;
            OnHighestPointClimb = genericDetector.OnHighestPointClimb;
            initialized = true;
            Destroy(genericDetector);
        }
    }
}

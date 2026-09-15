using System;

using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.XRDetectors
{
    /// <summary>
    /// Component that fires an event when an object like a lever rotate and reach a certain point.
    /// It fires also events when the object rotate in the crorrect or incorrect orientation
    /// </summary>
    public class XRRotationDetector : XRDetector
    {
        [Header("References")]
        public GameObject handle = default;

        [Header("Settings")]
        public RotationSettings rotationSettings = default;

        [Header("Events")]
        public UnityEvent onRotateInCorrectDirection = default;

        public UnityEvent onRotateInWrongDirection = default;

        public UnityEvent onStopRotation = default;

        public UnityEvent onTargetReached = default;

        private float targetAngle = 0.0f;
        private float currentDeltaAngle = 0.0f;
        private float previousFrameHandleOrientation = 0.0f;
        protected bool isGrabbed = false;
        private bool rotateInCorrectDirection = false;
        private bool rotateInWrongDirection = false;
        private bool targetReached = false;

        ///////////////////////////////////////////////////////////////////////////
        //Enums
        public enum RotationType
        {
            Degrees,
            Turns
        }

        public enum RotationAxis
        {
            x,
            y,
            z
        }

        ///////////////////////////////////////////////////////////////////////////
        //Structs
        [Serializable]
        public struct RotationSettings
        {
            [Tooltip("Unit of measure for rotationTarget")]
            public RotationType rotationType;
            [Tooltip("The otation axis of the object")]
            public RotationAxis rotationAxis;
            [Tooltip("The target to reach with the rotation. Could be in degrees or in turns")]
            public float rotationTarget;
            [Tooltip("Choose if the object have to turn clockwise or not")]
            public bool clockwise;
            [Tooltip("If true, the object behaviour will be like a crick, so doesn't lose delta rotation when go back")]
            public bool crickMode;
        }

        ///////////////////////////////////////////////////////////////////////////
        protected virtual void Awake()
        {
            if (rotationSettings.rotationType == RotationType.Degrees)
                targetAngle = rotationSettings.rotationTarget;
            else
                targetAngle = rotationSettings.rotationTarget * 360;
        }

        ///////////////////////////////////////////////////////////////////////////
        protected void Init()
        {
            XRRotationDetector genericDetector = null;
            foreach (var detector in GetComponents<XRRotationDetector>())
            {
                if (detector != this)
                {
                    genericDetector = detector;
                    break;
                }
            }
            handle = genericDetector.handle;
            rotationSettings = genericDetector.rotationSettings;
            onRotateInCorrectDirection = genericDetector.onRotateInCorrectDirection;
            onRotateInWrongDirection = genericDetector.onRotateInWrongDirection;
            onStopRotation = genericDetector.onStopRotation;
            onTargetReached = genericDetector.onTargetReached;
            initialized = true;
            Destroy(genericDetector);
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check when the object rotate and reach the target
        /// </summary>
        private void Update()
        {
            //Based on user choice, assign the correct eulerAngle
            float currentHandleOrientation = 0;
            switch (rotationSettings.rotationAxis)
            {
                case RotationAxis.x:
                    currentHandleOrientation = handle.transform.eulerAngles.x;
                    break;
                case RotationAxis.y:
                    currentHandleOrientation = handle.transform.eulerAngles.y;
                    break;
                case RotationAxis.z:
                    currentHandleOrientation = handle.transform.eulerAngles.z;
                    break;
            }

            if (isGrabbed)
            {
                //Calculate delta angle
                float deltaAngle = Mathf.DeltaAngle(previousFrameHandleOrientation, currentHandleOrientation);
                float orientedDeltaAngle = deltaAngle * (rotationSettings.clockwise ? -1.0f : 1.0f);

                //Check rotaion events
                if (orientedDeltaAngle > 0.25f && !rotateInCorrectDirection)
                {
                    rotateInCorrectDirection = true;
                    rotateInWrongDirection = false;
                    onRotateInCorrectDirection?.Invoke();
                }
                else if (orientedDeltaAngle < -0.25f && !rotateInWrongDirection)
                {
                    rotateInWrongDirection = true;
                    rotateInCorrectDirection = false;
                    onRotateInWrongDirection?.Invoke();
                }

                if (rotationSettings.crickMode)
                {
                    bool isTightening = orientedDeltaAngle > 0.0f;
                    if (isTightening)
                    {
                        IncreaseAngle(orientedDeltaAngle);
                    }
                }
                else
                {
                    IncreaseAngle(orientedDeltaAngle);
                }

                previousFrameHandleOrientation = currentHandleOrientation;
            }
            else if (rotateInCorrectDirection || rotateInWrongDirection)
            {
                rotateInCorrectDirection = false;
                rotateInWrongDirection = false;
                onStopRotation?.Invoke();
            }

            previousFrameHandleOrientation = currentHandleOrientation;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Increase the currentDeltaAngle and check if the object reach the target
        /// </summary>
        /// <param name="orientedDeltaAngle">oriented delta angle of this frame</param>
        private void IncreaseAngle(float orientedDeltaAngle)
        {
            currentDeltaAngle += orientedDeltaAngle;
            //When the object reach the target, invoke the end event
            if (currentDeltaAngle >= targetAngle && !targetReached)
            {
                onTargetReached.Invoke();
                targetReached = true;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reset needed value
        /// </summary>
        public void ResetTool()
        {
            currentDeltaAngle = 0.0f;
            rotateInCorrectDirection = false;
            rotateInWrongDirection = false;
            targetReached = false;
        }
    }
}

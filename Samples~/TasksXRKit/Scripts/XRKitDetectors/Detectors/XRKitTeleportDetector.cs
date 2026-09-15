using SPACS.Tasks.XRDetectors;

using System;

using UnityEngine.XR.Interaction.Toolkit;

namespace SPACS.TasksXRKit.XRKitDetectors
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Simple component that fires an event when one of the listed teleporters
    /// is used to perform a teleport
    /// </summary>
    public class XRKitTeleportDetector : XRTeleportDetector
    {
        private UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider[] xrKitTeleporters = default;
        private Action<LocomotionSystem>[] callbacks;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            if (!initialized)
            {
                Init();

                int lenght = teleporters.Length;
                xrKitTeleporters = new UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider[lenght];
                for (int i = 0; i < lenght; i++)
                    xrKitTeleporters[i] = teleporters[i].GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
            }

            callbacks = new Action<LocomotionSystem>[xrKitTeleporters.Length];
            for (int i = 0; i < xrKitTeleporters.Length; i++)
            {
                UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleporter = xrKitTeleporters[i];
                callbacks[i] = destination => OnTeleport.Invoke();
                xrKitTeleporters[i].endLocomotion += callbacks[i];
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            for (int i = 0; i < xrKitTeleporters.Length; i++)
                xrKitTeleporters[i].endLocomotion -= callbacks[i];
        }
    }
}
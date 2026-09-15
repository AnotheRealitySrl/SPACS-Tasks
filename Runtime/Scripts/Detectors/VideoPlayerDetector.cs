using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace SPACS.Tasks
{
    public class VideoPlayerDetector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private VideoPlayer videoPlayer;

        [Header("Events")]
        [SerializeField, Tooltip("")]
        private UnityEvent<VideoPlayer> OnPlayerPlay = default;

        [SerializeField, Tooltip("")]
        private UnityEvent<VideoPlayer> OnPlayerEnd = default;

        void Start()
        {
            videoPlayer.started += (vp) => OnPlayerPlay?.Invoke(vp);
            videoPlayer.loopPointReached += (vp) => OnPlayerEnd?.Invoke(vp);
        }
    }
}

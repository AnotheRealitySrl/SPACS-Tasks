using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks
{
    public class AnimatorReverseDetector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Animator animator;

        [SerializeField] private UnityEvent onAnimationFinished = default;
        [SerializeField] private UnityEvent onAnimationStarted = default;

        private bool isReversed = false;

        private void Start()
        {
            isReversed = false;
        }

        //function called by the animation when it ends. It only works if the animation has being played in the correct time direction (is not reversed). Once it's called set reversed to true.
        public void FinishedAnimationEvent()
        {
            if (!isReversed)
            {
                onAnimationFinished.Invoke();
                isReversed = true;
            }
        }

        //function called by the animation. It checks whether or not the animation is reversed (with a speed of -1), if it is then call the event, otherwise do nothing
        public void StartAnimatioEvent()
        {
            if (isReversed)
            {
                onAnimationStarted.Invoke();
                isReversed = false;
            }
        }

    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks
{
    public class AnimatorDetector : MonoBehaviour
    {
        [SerializeField] private List<UnityEvent> onAnimationEvent = default; //set by the user. Called by the animation via animation event

        //function called by the animation when it ends. It only works if the animation has being played in the correct time direction (is not reversed). Once it's called set reversed to true.
        public void AnimationEventCallback(int eventPos)
        {
            onAnimationEvent[eventPos].Invoke();
        }

    }
}

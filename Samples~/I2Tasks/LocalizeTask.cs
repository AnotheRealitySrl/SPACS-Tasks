using I2.Loc;

using UnityEngine;
using UnityEngine.Video;

namespace SPACS.Tasks.Utils
{
    public class LocalizeTask : MonoBehaviour
    {
        [SerializeField, Tooltip("Enable description localization")]
        private bool localizeDescription = false;
        [SerializeField, Tooltip("Enable image localization")]
        private bool localizeImage = false;
        [SerializeField, Tooltip("Enable videoClip localization")]
        private bool localizeVideo = false;

        [Space]
        [SerializeField, Tooltip("I2 key relative to the task's description")]
        private LocalizedString descriptionKey;
        [SerializeField, Tooltip("I2 key relative to the task's sprite")]
        private LocalizedString imageKey;
        [SerializeField, Tooltip("I2 key relative to the task's video")]
        private LocalizedString videoKey;


        void OnEnable()
        {
            Task task = GetComponent<Task>();

            //Localize task description
            if (localizeDescription)
                task.Node.Description = descriptionKey;

            //Localize task image
            if (localizeImage)
                task.Node.Image = LocalizationManager.GetTranslatedObjectByTermName<Sprite>(imageKey.mTerm);

            //Localize task video
            if (localizeVideo)
                task.Node.VideoClip = LocalizationManager.GetTranslatedObjectByTermName<VideoClip>(videoKey.mTerm);
        }
    }
}

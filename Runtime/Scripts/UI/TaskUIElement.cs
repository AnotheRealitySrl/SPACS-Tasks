using TMPro;

using UnityEngine;
using UnityEngine.Events;

using FontStyles = TMPro.FontStyles;

namespace SPACS.Tasks.UI
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Controller for an element that represents a task in the UI
    /// </summary>
    public class TaskUIElement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Text taskName = default;

        [Header("Settings")]
        [SerializeField]
        private TextSettings lockedSettings;
        [SerializeField]
        private TextSettings toDoSettings;
        [SerializeField]
        private TextSettings completedSettings;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onTaskLocked = default;

        [SerializeField]
        private UnityEvent onTaskUnlocked = default;

        [SerializeField]
        private UnityEvent onTaskCompleted = default;

        [System.Serializable]
        public class TextSettings
        {
            public int fontSize;
            public Color textColor;
            public FontStyles fontStyle;
            public TextAlignmentOptions alignmentOptions;
            public TMP_FontAsset font;

            public void SetText(TMP_Text text)
            {
                text.fontSize = fontSize;
                text.color = textColor;
                text.fontStyle = fontStyle;
                text.alignment = alignmentOptions;
                text.font = font;
            }
        }


        ///////////////////////////////////////////////////////////////////////////
        private TaskNode task;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The task represented by this UI element
        /// </summary>
        public TaskNode Task
        {
            get => task;
            set
            {
                task = value;
                taskName.text = task.Description;
                switch (task.Status)
                {
                    case TaskNode.TaskStatus.Locked:
                        lockedSettings.SetText(taskName);
                        onTaskLocked.Invoke();
                        break;
                    case TaskNode.TaskStatus.Todo:
                        toDoSettings.SetText(taskName);
                        onTaskUnlocked.Invoke();
                        break;
                    case TaskNode.TaskStatus.Completed:
                        completedSettings.SetText(taskName);
                        onTaskCompleted.Invoke();
                        break;
                }
            }
        }
    }
}

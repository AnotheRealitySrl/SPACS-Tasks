using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.Detectors
{
    /// <summary>
    /// Utility class listens and fires events related to a task
    /// </summary>
    public class TaskListener : MonoBehaviour
    {
        [SerializeField, Tooltip("Invoked when the task is unlocked (status ToDo)")]
        private UnityEvent onTaskUnlocked = default;

        [SerializeField, Tooltip("Invoked when the task is completed (status Completed)")]
        private UnityEvent<bool> onTaskCompleted = default;


        private Task task = default;

        ///////////////////////////////////////////////////////////////////////////
        private void Awake()
        {
            task = GetComponentInParent<Task>();
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            task.OnTaskUnlocked.AddListener(onTaskUnlocked.Invoke);
            task.OnTaskCompleted.AddListener(onTaskCompleted.Invoke);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            task.OnTaskUnlocked.RemoveListener(onTaskUnlocked.Invoke);
            task.OnTaskCompleted.RemoveListener(onTaskCompleted.Invoke);
        }
    }
}
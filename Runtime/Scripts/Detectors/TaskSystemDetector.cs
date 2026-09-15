using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks.Detectors
{
    public class TaskSystemDetector : MonoBehaviour
    {
        [SerializeField]
        private TaskSystem taskSystem = default;

        [SerializeField]
        private UnityEvent onTaskCompleted = default;

        [SerializeField]
        private UnityEvent onSubtaskCompleted = default;

        private List<(UnityEvent<bool>, UnityAction<bool>)> callbacks = new List<(UnityEvent<bool>, UnityAction<bool>)>();

        ///////////////////////////////////////////////////////////////////////////
        private void Start()
        {
            taskSystem.OnTaskCompleted += TaskCompleted;
        }

        ///////////////////////////////////////////////////////////////////////////
        private void TaskCompleted(TaskNode task)
        {
            if (task.TasksDependingOnThis.Count == 0)
            {
                onTaskCompleted?.Invoke();
            }
            else
            {
                onSubtaskCompleted?.Invoke();
            }
        }
    }
}

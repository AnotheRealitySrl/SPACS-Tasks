using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace SPACS.Tasks.UI
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manager of the UI that shows currently active tasks
    /// </summary>
    public class TaskUIManager : MonoBehaviour
    {
        [Serializable]
        public class SpriteUnityEvent : UnityEvent<Sprite> { }

        [Serializable]
        public class VideoClipUnityEvent : UnityEvent<VideoClip> { }

        public enum TaskType
        {
            Macrotask,
            Subtask
        }


        [Header("References")]
        [SerializeField, Tooltip("The task system instance")]
        private TaskSystem taskSystem = default;

        [SerializeField, Tooltip("The parent task element")]
        private TaskUIElement parentTaskElement = default;

        [SerializeField, Tooltip("The UI container of the tasks elements")]
        private RectTransform tasksContainer = default;

        [SerializeField, Tooltip("The slider representing the tasks progress")]
        private Slider tasksProgressSlider = default;

        [SerializeField, Tooltip("The label indicating the count of completed tasks")]
        private TMP_Text stepIndexLabel = default;

        [SerializeField, Tooltip("The label containing the total task counter")]
        private TMP_Text totalTasksCountLabel = default;

        [SerializeField, Tooltip("The panel of training finished")]
        private GameObject finishPanel = default;


        [Header("Prefabs")]
        [SerializeField, Tooltip("The task element UI prefab")]
        private TaskUIElement taskUIElementPrefab = default;


        [Header("Settings")]
        [SerializeField, Tooltip("Adds a delay to the disappearance of the tasks element after completion")]
        private float timeDelay = 1.0f;

        [SerializeField, Tooltip("The max depth of tasks to consider")]
        private int maxTasksDepth = 1;

        [SerializeField, Tooltip("The type of task to use for the slider progress")]
        private TaskType sliderTasksType = TaskType.Subtask;

        [SerializeField, Tooltip("The type of task to use for the steps counter")]
        private TaskType stepsTasksType = TaskType.Macrotask;


        [Header("Events")]
        [SerializeField, Tooltip("Invoked when the last task of the task system changes to completed")]
        public UnityEvent onTrainingEnd = default;

        [SerializeField]
        private UnityEvent onTaskWithoutMedia = default;

        [SerializeField]
        private SpriteUnityEvent onTaskWithImage = default;

        [SerializeField]
        private VideoClipUnityEvent onTaskWithVideo = default;


        private IEnumerable<TaskNode> allTasks, macroTasks, subTasks;
        private int numberOfMacroTasks;
        private int numberOfSubTasks;

        ///////////////////////////////////////////////////////////////////////////
        //private void Awake()
        //{
        //    taskSystem.Prepare();
        //    DoAfterTaskPrepare();
        //}

        ///////////////////////////////////////////////////////////////////////////
        public void DoAfterTaskPrepare()
        {
            taskSystem.tasksChanged.AddListener(RebuildUI);
            taskSystem.lastTaskCompleted.AddListener(OnTrainingEnd);

            allTasks = taskSystem.Tasks
                .Where(t => t.Depth <= maxTasksDepth)
                .Where(t => t.ShowInUI);

            macroTasks = allTasks
                .Where(t => t.Dependencies.Count > 0 && t.Depth < maxTasksDepth);

            numberOfMacroTasks = macroTasks.Count();

            subTasks = allTasks
               .Where(t => t.Dependencies.Count == 0 || t.Depth == maxTasksDepth);

            numberOfSubTasks = subTasks.Count();

            RebuildUIImmediately();
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Destroys all current task elements and create new one from currently active tasks
        private void RebuildUI()
        {
            RunWithDelay(timeDelay, RebuildUIImmediately);
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Destroys all current task elements and create new one from currently active tasks
        public void RebuildUIImmediately()
        {
            // Activate the GameObject no matter what
            gameObject.SetActive(true);

            // Destroy current tasks elements
            foreach (TaskUIElement taskElement in tasksContainer.GetComponentsInChildren<TaskUIElement>())
                DestroyImmediate(taskElement.gameObject);

            if (allTasks == null)
                return;

            // Find the first task to do
            TaskNode firstTaskToDo = allTasks
                .Where(t => t.Status == TaskNode.TaskStatus.Todo)
                .FirstOrDefault();

            if (firstTaskToDo != null)
            {
                TaskNode parentTask = firstTaskToDo;

                // Get the parent task
                if (firstTaskToDo.TasksDependingOnThis.Count != 0)
                {
                    parentTask = firstTaskToDo.TasksDependingOnThis.Where(t => t.TasksDependingOnThis.Count == 0).FirstOrDefault();
                }

                // Going deep if the maxTasksDepth parameter allows it
                var subtasksToDo = firstTaskToDo.Dependencies.Where(t => t.Status == TaskNode.TaskStatus.Todo);
                int depth = firstTaskToDo.Depth; //Da correggere
                while (subtasksToDo.Count() > 0 && depth < maxTasksDepth)
                {
                    parentTask = firstTaskToDo;
                    firstTaskToDo = subtasksToDo.FirstOrDefault();
                    subtasksToDo = firstTaskToDo.Dependencies.Where(t => t.Status == TaskNode.TaskStatus.Todo);
                    depth++;
                }

                // No parent? Use the first task to do as only task
                if (parentTask == null)
                    parentTaskElement.Task = firstTaskToDo;

                else
                {
                    // Initialize the parent task element
                    parentTaskElement.Task = parentTask;

                    // Invoke the media events
                    if (parentTask.Image != null)
                        onTaskWithImage.Invoke(parentTask.Image);
                    else if (parentTask.VideoClip != null)
                        onTaskWithVideo.Invoke(parentTask.VideoClip);
                    else
                        onTaskWithoutMedia.Invoke();

                    // Create new task elements
                    foreach (TaskNode subtask in parentTask.Dependencies)
                    {
                        if (!subtask.ShowInUI)
                            continue;
                        TaskUIElement taskElement = Instantiate(taskUIElementPrefab, tasksContainer);
                        taskElement.Task = subtask;
                    }
                }
            }
            else return;

            // Update the slider progress
            IEnumerable<TaskNode> sliderTasks = (sliderTasksType == TaskType.Macrotask ? macroTasks : subTasks);
            int sliderProgress = sliderTasks
                .Where(t => t.Status == TaskNode.TaskStatus.Completed)
                .Count();
            tasksProgressSlider.value = ((float)sliderProgress) / sliderTasks.Count();

            // Update the steps label
            IEnumerable<TaskNode> stepsTasks = stepsTasksType == TaskType.Macrotask ? macroTasks : subTasks;
            int stepIndex = stepsTasks
                .Where(t => t.Status == TaskNode.TaskStatus.Completed)
                .Count() + 1;
            stepIndexLabel.text = stepIndex.ToString().PadLeft(2, '0');

            //totalTasksCountLabel.text = stepsTasks.Count().ToString().PadLeft(2, '0');
            if (stepsTasksType == TaskType.Macrotask)
            {
                totalTasksCountLabel.text = numberOfMacroTasks.ToString().PadLeft(2, '0');
            }
            else
            {
                totalTasksCountLabel.text = numberOfSubTasks.ToString().PadLeft(2, '0');
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Utility method to run things after a delay
        private void RunWithDelay(float delay, Action action)
        {
            IEnumerator coroutine()
            {
                yield return new WaitForSeconds(delay);
                action();
            }
            StartCoroutine(coroutine());
        }

        ///////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// Switch the current panel with the training end one
        ///</summary>
        private void OnTrainingEnd()
        {
            taskSystem.tasksChanged.RemoveListener(RebuildUI);
            StopAllCoroutines();
            onTrainingEnd?.Invoke();
            if (finishPanel != null) finishPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
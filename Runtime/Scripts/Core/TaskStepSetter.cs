using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SPACS.Tasks
{
    public class TaskStepSetter : MonoBehaviour
    {
        [System.Serializable]
        public class TrainingStep
        {
            [SerializeField] private Task task = default;
            [SerializeField] private UnityEvent onBeforeStepStart = default;

            public Task Task => task;
            public UnityEvent OnBeforeStepStart => onBeforeStepStart;
        }

        public static int StepIndex { get; set; } = 0;

        [Header("Scene Elements References")]
        [SerializeField] protected TaskSystem taskSystem = default;
        [Header("Training Steps")]
        [SerializeField] private TrainingStep[] trainingSteps = default;

        public TrainingStep[] TrainingSteps => trainingSteps;

        /// <summary>
        /// Set the starting task index
        /// </summary>
        /// <param name="stepIndex"> index where we want to start </param>
        public void SetStepIndex(int stepIndex)
        {
            StepIndex = stepIndex;
            Debug.Log($"New step selected! {StepIndex + 1}");
        }

        /// <summary>
        /// SetUp the training starting from selected Task
        /// </summary>
        public void InitDefaultStep()
        {
            if (taskSystem == null || trainingSteps == null || trainingSteps.Length == 0)
            {
                // Nothing to do.
                return;
            }

            TrainingStep trainingStep = trainingSteps[Mathf.Clamp(StepIndex, 0, trainingSteps.Length - 1)];

            // Ordered list of tasks. I assign the state "Complete" until I find the node I want.
            IReadOnlyCollection<TaskNode> allNodes = taskSystem.Tasks;
            foreach (TaskNode node in allNodes)
            {
                if (CompleteTaskRecursive(node, trainingStep.Task.Node))
                {
                    // Target reached. Nothing else to do in this loop.
                    break;
                }
            }
        }

        /// <summary>
        /// Complete all tasks and dependencies until we reach the point we want start from
        /// </summary>
        /// <param name="parentTask"> Starting task </param>
        /// <param name="targetNode"> Final task </param>
        /// <returns></returns>
        protected bool CompleteTaskRecursive(TaskNode parentTask, TaskNode targetNode)
        {
            if (parentTask == null)
            {
                // No task in input.
                return false;
            }


            // If the current task to check is a task used as a step start, then
            // its callback "before start" is executed.
            if (trainingSteps == null)
            {

            }
            else
            {
                TrainingStep checkStep = trainingSteps.FirstOrDefault(x => x.Task.Node == parentTask);
                if (checkStep != null)
                {
                    checkStep.OnBeforeStepStart?.Invoke();
                }

            }

            if (parentTask == targetNode)
            {
                // Reached my node! This would be in ToDo, not to be completed.
                return true;
            }



            if (parentTask.Dependencies.Count == 0)
            {
                // Leaf task, that means that I can complete it.
                if (parentTask.Status == TaskNode.TaskStatus.Todo)
                    parentTask.Status = TaskNode.TaskStatus.Completed;

                //If the task has a next task, we jump into that
                //So we can complete the task in the correct order
                if (parentTask.Next != null)
                {
                    if (CompleteTaskRecursive(parentTask.Next, targetNode))
                    {
                        return true;
                    }
                }
            }
            else
            {
                //While we have uncompleted task, we cycle into tasks to complete all of those
                while (true)
                {
                    var task = parentTask.Dependencies.Where((task) => task.Status == TaskNode.TaskStatus.Todo && task.Previous == null).FirstOrDefault();

                    if (task == null) break;

                    if (CompleteTaskRecursive(task, targetNode))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TaskStepSetter))]
    public class TaskStepSetterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TaskStepSetter obj = (TaskStepSetter)target;
            GUI.enabled = Application.isPlaying;
            for (int i = 0; i < obj.TrainingSteps.Length; i++)
            {
                if (GUILayout.Button($"Setup Phase {i + 1}"))
                {
                    obj.SetStepIndex(i);
                    obj.InitDefaultStep();
                }
            }
            GUI.enabled = true;
        }
    }
#endif
}
using SPACS.Graphs;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using static Virtuademy.SDK.Tasks.TaskNode;

namespace Virtuademy.SDK.Tasks
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manager class that controls the tasks flow
    /// </summary>
    public class TaskSystem : MonoBehaviour
    {
        #region Inspector Info

        [Header("References")]
        [SerializeField, Tooltip("The scene component or project asset that contains a valid graph")]
        protected Object graphContainer = default;
        [Space]
        [SerializeField] public bool startImmediately = true;

        [SerializeField, Tooltip("The hierarchy components to be reverted in case of task system reset")]
        protected List<TaskObjectReverterBase> reverters = new List<TaskObjectReverterBase>();

        [Header("Events")]
        [SerializeField, Tooltip("Invoked when the task system is ready to go")]
        protected UnityEvent taskSystemReady = default;

        [SerializeField, Tooltip("Invoked when any task of the task system changes its status")]
        public UnityEvent tasksChanged = default;

        [SerializeField, Tooltip("Invoked when the last task of the task system changes to completed")]
        public UnityEvent lastTaskCompleted = default;

        #endregion

        public delegate void TaskCompleted(TaskNode node);
        public event TaskCompleted OnTaskCompleted;

        protected IGraph graph = null;
        protected bool isPrepared = false;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The graph used to build the task system</summary>
        public IGraph Graph
        {
            get
            {
                if (graph == null)
                {
                    if (graphContainer is IContainer<IGraph> container)
                        graph = container.Value;
                }
                return graph;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of all tasks contained in the graph</summary>
        public IReadOnlyCollection<TaskNode> Tasks => Graph.GetNodes<TaskNode>();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of all root tasks contained in the graph.
        /// A task is considered a "root task" if no other task precedes it or
        /// depends on it</summary>
        public IReadOnlyCollection<TaskNode> RootTasks => Tasks
            .Where(t => t.Inputs.Count == 0)
            .ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of all tasks contained in the graph currently
        /// having the status set to ToDo</summary>
        public IReadOnlyCollection<TaskNode> ToDoTasks => Tasks
            .Where(t => t.Status == TaskStatus.Todo)
            .Where(t => t.Dependencies.Count == 0)
            .ToList();


        ///////////////////////////////////////////////////////////////////////////
        protected virtual void Awake()
        {
            Prepare();
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Call this method to make sure that the task system has been
        /// correctly initialized</summary>
        public void Prepare()
        {
            if (isPrepared)
                return;
            isPrepared = true;

            // Prepare all the reverters
            foreach (TaskObjectReverterBase reverter in reverters)
            {
                if (reverter != null)
                {
                    reverter.Prepare(this);
                }
            }

            // Register the global callback to the statuses changes events
            IReadOnlyCollection<TaskNode> allNodes = Tasks;
            foreach (TaskNode node in allNodes)
                node.onStatusChanged.AddListener(oldStatus => OnTaskStatusChanged(node, oldStatus));

            // Lock all tasks by default
            foreach (TaskNode node in allNodes)
                node.Status = TaskStatus.Locked;

            // Set all root tasks status to ToDo
            foreach (TaskNode node in RootTasks)
                node.Status = TaskStatus.Todo;

            //Skip one frame to adjust execution order in build
            IEnumerator DelayedEvent()
            {
                yield return null;
                // Invoke the "task system ready" event
                taskSystemReady.Invoke();
            }
            StartCoroutine(DelayedEvent());
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Call this method to make the task system reset</summary>
        public virtual void Revert()
        {
            // Revert all the reverters
            foreach (TaskObjectReverterBase reverter in reverters)
            {
                if (reverter != null)
                {
                    reverter.Revert();
                }
            }

            // Lock all tasks by default
            foreach (TaskNode node in Tasks)
                node.Status = TaskStatus.Locked;

            // Set all root tasks status to ToDo
            foreach (TaskNode node in RootTasks)
                node.Status = TaskStatus.Todo;

            //Skip one frame to adjust execution order in build
            IEnumerator DelayedEvent()
            {
                yield return null;
                // Invoke the "task system ready" event
                taskSystemReady.Invoke();
            }
            StartCoroutine(DelayedEvent());
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Called when a task changed its status
        protected void OnTaskStatusChanged(TaskNode task, TaskStatus _)
        {
            // Has the task been completed?
            if (task.Status == TaskStatus.Completed)
            {
                OnTaskCompleted?.Invoke(task);

                // Unlock the next task (if it exists)
                TaskNode nextTask = task.Next;
                if (nextTask != null && nextTask.Status == TaskStatus.Locked)
                    nextTask.Status = TaskStatus.Todo;

                // Complete any task that depends on this task IF it is the last task to be completed
                // in the collection of dependencies
                foreach (var taskDependingOnThis in task.TasksDependingOnThis)
                    if (taskDependingOnThis.Dependencies.All(dep => dep.Status == TaskStatus.Completed))
                        taskDependingOnThis.Status = TaskStatus.Completed;

                if (task == Tasks.LastOrDefault())
                {
                    lastTaskCompleted?.Invoke();
                }
            }
            // Has the task been unlocked?
            else if (task.Status == TaskStatus.Todo)
            {
                foreach (var dep in task.Dependencies.Where(t => t.Previous.Count == 0))
                    dep.Status = TaskStatus.Todo;
            }

            // Invoke the tasksChanged event
            tasksChanged.Invoke();
        }
    }
}
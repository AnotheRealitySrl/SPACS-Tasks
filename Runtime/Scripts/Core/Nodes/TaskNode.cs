using SPACS.Graphs;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace SPACS.Tasks
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A Graph Node used to represent a task for the Task System
    /// </summary>
    public class TaskNode : Node
    {

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The possible task statuses</summary>
        public enum TaskStatus
        {
            Locked,
            Todo,
            InProgress,
            Completed,
            Failed
        }

        ///////////////////////////////////////////////////////////////////////////
        [Header("Ports")]
        [SerializeField, HideInInspector, PortLabel("")]
        protected MultiInputPort<TaskNode> input = default;

        [SerializeField, HideInInspector]
        protected OutputPort<TaskNode> next = default;

        [SerializeField, HideInInspector]
        protected MultiOutputPort<TaskNode> dependencies = default;


        [Header("Data")]
        [SerializeField, Tooltip("The task description")]
        protected string description;

        [SerializeField, TextArea(1, 4), Tooltip("Hint shown when the user asks for one (procedures). Empty: no hint")]
        protected string hint;

        [SerializeField, TextArea(1, 4), Tooltip("Shown when the task is done right (procedures). Empty: a generic text")]
        protected string rightFeedback;

        [SerializeField, TextArea(1, 4), Tooltip("Shown for a wrong attempt on the task (procedures). Empty: the reason, or a generic text")]
        protected string wrongFeedback;

        [SerializeField, Tooltip("Image media file attached to the task")]
        protected Sprite image = default;

        [SerializeField, Tooltip("Video media file attached to the task")]
        protected VideoClip videoClip = default;

        [SerializeField, NodeData, Tooltip("The current task status")]
        protected TaskStatus status = TaskStatus.Locked;

        [Header("Settings")]
        [SerializeField, Tooltip("Is this task going to be shown in UI?")]
        protected bool showInUI = true;

        [Header("Events")]
        [SerializeField, Tooltip("Event called when the task status changes")]
        public UnityEvent<TaskStatus> onStatusChanged = default;



        ///////////////////////////////////////////////////////////////////////////
        /// <summary></summary>The collection of tasks that precede or depends on
        /// this task</summary>
        public IReadOnlyCollection<TaskNode> Inputs => input.LinkedNodes;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The next task</summary>
        public TaskNode Next => next.LinkedNodes.FirstOrDefault();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The previous task</summary>
        public IReadOnlyCollection<TaskNode> Previous => Inputs
            .Where(i => i.Next == this)
            .ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of tasks that depend on this task</summary>
        public IReadOnlyCollection<TaskNode> TasksDependingOnThis => Inputs
            .Where(i => i.Dependencies.Contains(this))
            .ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of tasks which this task depends on</summary>
        public IReadOnlyCollection<TaskNode> Dependencies => dependencies.LinkedNodes;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The task description</summary>
        public string Description
        {
            get => description;
            set => description = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The hint text, as written on the node (null in graphs saved before it existed)</summary>
        public string Hint
        {
            get => hint ?? string.Empty;
            set => hint = value;
        }

        /// <summary>The text for a right completion, as written on the node (null in older graphs)</summary>
        public string RightFeedback
        {
            get => rightFeedback ?? string.Empty;
            set => rightFeedback = value;
        }

        /// <summary>The text for a wrong attempt, as written on the node (null in older graphs)</summary>
        public string WrongFeedback
        {
            get => wrongFeedback ?? string.Empty;
            set => wrongFeedback = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The image media file attached to the task</summary>
        public Sprite Image
        {
            get => image;
            set => image = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The videoClip media file attached to the task</summary>
        public VideoClip VideoClip
        {
            get => videoClip;
            set => videoClip = value;
        }

        /// <summary>Is this task going to be shown in UI?</summary>
        public bool ShowInUI
        {
            get => showInUI;
            set => showInUI = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The current task status</summary>
        public TaskStatus Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    var oldStatus = status;
                    status = value;

                    onStatusChanged.Invoke(oldStatus);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the hierarchy depth of this task</summary>
        public int Depth
        {
            get
            {
                TaskNode pointer = this;
                int depth = 0;
                while (pointer.TasksDependingOnThis.Count != 0 && pointer.Status != TaskStatus.Todo)
                {
                    pointer = pointer.TasksDependingOnThis.First();
                    depth++;
                }
                return depth;
            }
        }


        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR

        /// This dictionary maps a status to a color used in the editor's graph
        /// to highlight nodes depending on their statuses
        protected static readonly Dictionary<TaskStatus, Color> nodeColors = new Dictionary<TaskStatus, Color>()
        {
            { TaskStatus.Completed, Color.green },
            { TaskStatus.Failed, Color.red },
            { TaskStatus.InProgress, Color.yellow },
            { TaskStatus.Todo, Color.gray },
        };

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Called when a node needs to be visually rendered in the
        /// graph panel</summary>
        /// <param name="nodeElement">The visual element of the node</param>
        public override void OnDrawNodeElementInEditor(VisualElement nodeElement)
        {
            if (Application.isPlaying)
            {
                Color originalColor = nodeElement.style.backgroundColor.value;

                void updateColor()
                {
                    if (!nodeColors.TryGetValue(Status, out Color newNodeColor))
                        newNodeColor = originalColor;
                    nodeElement.style.backgroundColor = newNodeColor;
                }

                updateColor();

                // The panel redraws a node many times: keep one listener per node, the one of
                // the element drawn last, instead of adding one on every redraw.
                if (editorColorListener != null)
                    onStatusChanged.RemoveListener(editorColorListener);
                editorColorListener = oldStatus => updateColor();
                onStatusChanged.AddListener(editorColorListener);
            }
        }

        [System.NonSerialized]
        private UnityAction<TaskStatus> editorColorListener;
#endif
    }
}
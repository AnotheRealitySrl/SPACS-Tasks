using SPACS.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SPACS.TasksNetworked
{
    public class NetworkedTaskStepSetter : TaskStepSetter
    {
        private TasksRPCManager taskRPCManager;
        private void Start()
        {
            taskRPCManager = GetComponent<TasksRPCManager>();
            taskRPCManager.OnRoomJoined += Init;
        }

        private void Init(int id)
        {
            //calculate last node
            var tasks = FindObjectsOfType<NetworkedTask>();
            TaskNode targetNode = null;
            foreach (var task in tasks)
            {
                if (task.TaskID == id)
                {
                    targetNode = task.Node;
                    break;
                }
            }

            TaskNode newNode = targetNode;
            if (targetNode.Dependencies.Count != 0 && targetNode.Next != null)
            {
                newNode = targetNode.Next;
            }

            // Ordered list of tasks. I assign the state "Complete" until I find the node I want.
            IReadOnlyCollection<TaskNode> allNodes = taskSystem.Tasks;
            foreach (TaskNode node in allNodes)
            {
                if (CompleteTaskRecursive(node, newNode))
                {
                    // Target reached. Nothing else to do in this loop.
                    if (targetNode.Status == TaskNode.TaskStatus.Todo)
                        targetNode.Status = TaskNode.TaskStatus.Completed;
                    break;
                }
            }
        }
    }
}

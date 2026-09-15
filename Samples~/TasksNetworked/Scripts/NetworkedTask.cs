using SPACS.Tasks;
using static SPACS.Tasks.TaskNode;
using UnityEngine;

namespace SPACS.TasksNetworked
{
    public class NetworkedTask : Task
    {
        private bool forceCompleted = false;
        [SerializeField]
        private int taskID;

        public int TaskID { get => taskID; }

        [HideInInspector] public TasksRPCManager rpcManager;

        private void Start()
        {
            rpcManager = GetComponentInParent<TasksRPCManager>();
            rpcManager.OnTaskCompleted += ForceTaskComplete;
            forceCompleted = false;
        }

        protected override void OnStatusChanged(TaskStatus oldStatus)
        {
            if (Node.Status == TaskStatus.Completed && !forceCompleted)
            {
                rpcManager.UpdateTasksID(taskID);
                rpcManager.photonView.RPC(nameof(rpcManager.RPC_TaskStatusChanged), Photon.Pun.RpcTarget.Others, taskID);
            }

            forceCompleted = false;

            base.OnStatusChanged(oldStatus);
        }

        public void ForceTaskComplete(int id)
        {
            if (taskID != id)
                return;

            forceCompleted = true;
            CompleteTask();
        }

        //Auto-assign random id when task is created from graph
        private void Reset()
        {
            taskID = Mathf.Abs(gameObject.GetInstanceID());
        }
    }
}

using Photon.Pun;
using Photon.Realtime;
using SPACS.Tasks;
using UnityEngine;
using static SPACS.Tasks.ITasksRPCManager;
using PhHashtable = ExitGames.Client.Photon.Hashtable;

namespace SPACS.TasksNetworked
{
    public class TasksRPCManager : MonoBehaviourPunCallbacks, ITasksRPCManager
    {
        // ROOM CUSTOM PROPERTIES
        public string CURRENT_TASKS_ID = "tid";

        // The room property of this task system: "tid" and the view id, built once. OnJoinedRoom runs again for every
        // AddJoinRoomEvent, and appending the view id each time read the wrong key on rejoin.
        private string tasksIdKey;
        private string TasksIdKey
        {
            get
            {
                if (tasksIdKey == null)
                {
                    int viewId = GetComponent<PhotonView>().ViewID;
                    if (viewId == 0)
                    {
                        // Not allocated yet: do not keep a key that names no view.
                        return CURRENT_TASKS_ID + viewId;
                    }
                    tasksIdKey = CURRENT_TASKS_ID + viewId;
                }
                return tasksIdKey;
            }
        }

        public delegate void OnJoinRoom(int id);
        public event OnJoinRoom OnRoomJoined;
        public event ITasksRPCManager.OnTaskComplete OnTaskCompleted;
        public event ITasksRPCManager.OnRevertSystem OnRevertedSystem;
        public event ITasksRPCManager.InitRoom OnInitRoom;

        //public static TasksRPCManager Instance { get; private set; }

        private void Start()
        {
            //Todo add photon view, fattelo siegare da Ema
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            Room room = PhotonNetwork.CurrentRoom;
            PhHashtable roomCustomProps = room.CustomProperties;
            PhHashtable newRoomProperties = new PhHashtable();

            if (roomCustomProps.TryGetValue(TasksIdKey, out var taskID))
            {
                if ((int)taskID != -1)
                {
                    Debug.Log($"[TasksRPCManager] Restoring the room's task state: last completed task {(int)taskID}");
                    OnRoomJoined?.Invoke((int)taskID);
                    OnInitRoom?.Invoke((int)taskID);
                }
            }
            else
            {
                newRoomProperties.Add(TasksIdKey, -1);
                // Saves new custom properties
                room.SetCustomProperties(newRoomProperties);
            }
        }

        public void UpdateTasksID(int id)
        {
            //Update tasks ID property when task is completed
            Room room = PhotonNetwork.CurrentRoom;
            PhHashtable newRoomProperties = new PhHashtable
            {
                { TasksIdKey, id }
            };
            room.SetCustomProperties(newRoomProperties);
        }

        public void SendRPCTaskStatusChange(int taskID)
        {
            photonView.RPC(nameof(RPC_TaskStatusChanged), Photon.Pun.RpcTarget.Others, taskID);
        }

        public void SendRPCTaskRevert()
        {
            photonView.RPC(nameof(RPC_TaskSystemRevert), Photon.Pun.RpcTarget.Others);
        }


        [PunRPC]
        public void RPC_TaskStatusChanged(int taskID)
        {
            //OnTaskCompleted?.Invoke(taskID);
            OnTaskCompleted?.Invoke(taskID);
        }

        [PunRPC]
        public void RPC_TaskSystemRevert()
        {
            //OnTaskCompleted?.Invoke(taskID);
            OnRevertedSystem?.Invoke();
        }

        public void SetOnTaskCompleted(ITasksRPCManager.OnTaskComplete forceTaskComplete)
        {
            OnTaskCompleted += forceTaskComplete;
        }

        public void SetOnRevert(ITasksRPCManager.OnRevertSystem forceRevert)
        {
            OnRevertedSystem += forceRevert;
        }


        public void AddJoinRoomEvent(ITasksRPCManager.InitRoom initRoom)
        {
            OnInitRoom += initRoom;
            OnJoinedRoom();
        }

    }
}

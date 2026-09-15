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

            CURRENT_TASKS_ID = CURRENT_TASKS_ID + GetComponent<PhotonView>().ViewID;

            if (roomCustomProps.TryGetValue(CURRENT_TASKS_ID, out var taskID))
            {
                Debug.LogError("Exist");
                if ((int)taskID != -1)
                {
                    Debug.LogError($"TaskID current: {(int)taskID}");
                    OnRoomJoined?.Invoke((int)taskID);
                    OnInitRoom?.Invoke((int)taskID);
                }
            }
            else
            {
                Debug.LogError("Not Exist, add it");
                newRoomProperties.Add(CURRENT_TASKS_ID, -1);
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
                { CURRENT_TASKS_ID, id }
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

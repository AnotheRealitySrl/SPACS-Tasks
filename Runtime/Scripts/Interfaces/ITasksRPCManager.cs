namespace SPACS.Tasks
{

    public delegate void OnTaskComplete();
    public interface ITasksRPCManager
    {
        public delegate void OnTaskComplete(int id);
        public delegate void OnRevertSystem();
        public delegate void InitRoom(int id);

        public event OnTaskComplete OnTaskCompleted;
        public event OnRevertSystem OnRevertedSystem;
        public event InitRoom OnInitRoom;


        public void UpdateTasksID(int id);
        public void RPC_TaskStatusChanged(int taskID);
        public void SendRPCTaskStatusChange(int taskID);
        public void SendRPCTaskRevert();
        public void SetOnTaskCompleted(OnTaskComplete forceTaskComplete);
        public void SetOnRevert(OnRevertSystem forceRevert);
        public void AddJoinRoomEvent(InitRoom initRoom);
    }
}

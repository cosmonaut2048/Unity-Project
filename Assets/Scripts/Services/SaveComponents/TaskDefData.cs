using Content;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class TaskDefData
    {
        public string taskName;
    
        public void SetDataFromTaskDef(TaskDef task)
        {
            taskName = task ? task.name : "";
        }
    }
}